using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFSM
{
    /// <summary>
    /// Interface for State Machine
    /// </summary>
    public interface IStateMachine
    {
        /// <summary>
        /// Update the State Machine
        /// </summary>
        public void Update();
        /// <summary>
        /// Update the State Machine at a framerate independant frequency
        /// </summary>
        public void FixedUpdate();

#if UNITY_EDITOR
        /// <summary>
        /// Return the arborescence of the State Machine
        /// </summary>
        /// <returns></returns>
        public string GetArborescence();
#endif
    }

    public class StateMachineTransitionsOverloadException : System.Exception
    {
        public StateMachineTransitionsOverloadException()
            : base("Too many transitions at once in the State Machine in one update.") { }
        public StateMachineTransitionsOverloadException(string previousState, string nextState)
            : base("Too many transitions at once in the State Machine in one update. Transitions check stopped at these: [" + previousState + " => " + nextState + "]") { }
    }

    public partial class StateMachine : IStateMachine
    {
        private struct StateConfiguration
        {
#if UNITY_EDITOR
            public string stateName;
#endif
            public int stateImplementationID;
            public int subStateMachineID;
            public int parentStateMachineID;

            public int transitionsID;
            public int transitionsSize;
        }

        private struct StateTransition
        {
#if UNITY_EDITOR
            public string previousStateName;
            public string nextStateName;
#endif
            public int nextStateID;
            public int commonParentID;
            public int transitionID;
        }

#if UNITY_EDITOR
        private static readonly int MAX_TRANSITIONS_UPDATE = 128;
#endif

        private StateConfiguration[] _stateConfigurations;
        private StateTransition[] _stateTransitions;
        private IState[] _stateImplementations;
        private ITransition[] _transitions;

        private bool _initiated;
        private int _currentStateID;

        private Stack<int> _stateIDsStack;
        private Stack<int> _enteringStatesIDsStack;

#if UNITY_EDITOR
        private string _arborescence;

        public string GetArborescence()
        {
            return _arborescence;
        }
#endif

        private StateMachine()
        {
            _initiated = false;
            _currentStateID = 0;

            _stateIDsStack = new Stack<int>();
            _enteringStatesIDsStack = new Stack<int>();
        }

        /// <summary>
        /// Initialize the State Machine
        /// </summary>
        public void Init()
        {
            foreach (IState state in _stateImplementations)
            {
                state.OnInit();
            }

            if (_currentStateID >= 0)
            {
                StateConfiguration configuration = _stateConfigurations[_currentStateID];

                if (configuration.parentStateMachineID < 0)
                {
                    int nextStateID = _currentStateID;
                    while (nextStateID >= 0)
                    {
                        _currentStateID = nextStateID;
                        configuration = _stateConfigurations[_currentStateID];
                        if (configuration.stateImplementationID >= 0)
                        {
                            _stateImplementations[configuration.stateImplementationID].OnEnter();
                        }
                        nextStateID = configuration.subStateMachineID;
                    }
#if UNITY_EDITOR
                    _arborescence = FetchArborescence();
#endif
                    _initiated = true;
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogError("State " + configuration.stateName + " set as start state but has a parent state.");
                }
#endif
            }
            else
            {
                Debug.LogError("No state set as start state.");
            }
        }

        /// <summary>
        /// Check transition between states during an update
        /// </summary>
        private void CheckTransition()
        {
#if UNITY_EDITOR
            int transitionsCount = 0;
#endif
            FillStatesStack(_stateIDsStack, _currentStateID);
            while (_stateIDsStack.Count > 0)
            {
                int stateID = _stateIDsStack.Pop();
                StateConfiguration stateConfiguration = _stateConfigurations[stateID];
                for (int i = 0; i < stateConfiguration.transitionsSize; ++i)
                {
                    StateTransition stateTransition = _stateTransitions[stateConfiguration.transitionsID + i];
                    ITransition transition = _transitions[stateTransition.transitionID];
                    if (transition.Evaluate())
                    {
#if UNITY_EDITOR
                        if (transitionsCount++ >= MAX_TRANSITIONS_UPDATE)
                        {
                            throw new StateMachineTransitionsOverloadException(stateTransition.previousStateName, stateTransition.nextStateName);
                        }
#endif
                        if (stateID == stateTransition.commonParentID)  // If state has a transition with one of his child
                        {
                            int commonParent = FindCommonParent(_currentStateID, stateTransition.nextStateID);
                            HandleTransition(_currentStateID, stateTransition.nextStateID, commonParent, transition);
                            FillStatesStack(_stateIDsStack, stateTransition.nextStateID, stateID);
                        }
                        else
                        {
                            HandleTransition(_currentStateID, stateTransition.nextStateID, stateTransition.commonParentID, transition);
                            FillStatesStack(_stateIDsStack, stateTransition.nextStateID, stateTransition.commonParentID);
                        }

                        break;
                    }
                }

            }

#if UNITY_EDITOR
            _arborescence = FetchArborescence();
#endif
        }

        /// <summary>
        /// Handle the transition between two specified states
        /// </summary>
        /// <param name="exitingStateID"></param>
        /// <param name="enteringStateID"></param>
        /// <param name="commonParentID"></param>
        /// <param name="transition"></param>
        private void HandleTransition(int exitingStateID, int enteringStateID, int commonParentID, ITransition transition)
        {
            _currentStateID = enteringStateID;
            while (exitingStateID != commonParentID)
            {
                StateConfiguration configuration = _stateConfigurations[exitingStateID];
                ExitState(exitingStateID);
                exitingStateID = configuration.parentStateMachineID;
            }

            transition.OnTransition();
            
            FillStatesStack(_enteringStatesIDsStack, enteringStateID, commonParentID);
            while (_enteringStatesIDsStack.Count > 0)
            {
                EnterState(_enteringStatesIDsStack.Pop());
            }
        }

        /// <summary>
        /// Fill the states stack from startStateID to stopStateID
        /// </summary>
        /// <param name="statesIDsStack"></param>
        /// <param name="startStateID"></param>
        /// <param name="stopStateID"></param>
        private void FillStatesStack(Stack<int> statesIDsStack, int startStateID, int stopStateID = -1)
        {
            statesIDsStack.Clear();
            for (int stateID = startStateID; stateID >= 0; stateID = _stateConfigurations[stateID].parentStateMachineID)
            {
                if (stateID == stopStateID)
                {
                    break;
                }
                statesIDsStack.Push(stateID);
            }
        }

        /// <summary>
        /// Find common parent between stateIDA and stateIDB
        /// </summary>
        /// <param name="stateIDA"></param>
        /// <param name="stateIDB"></param>
        /// <returns></returns>
        private int FindCommonParent(int stateIDA, int stateIDB)
        {
            if (stateIDA == stateIDB)
            {
                return stateIDA;
            }
            StateConfiguration configurationA = _stateConfigurations[stateIDA];
            do
            {
                StateConfiguration configurationB = _stateConfigurations[stateIDB];
                while (configurationB.parentStateMachineID >= 0)
                {
                    if (stateIDA == configurationB.parentStateMachineID)
                    {
                        return stateIDA;
                    }
                    if (stateIDB == configurationA.parentStateMachineID)
                    {
                        return stateIDB;
                    }
                    if (configurationA.parentStateMachineID == configurationB.parentStateMachineID)
                    {
                        return configurationA.parentStateMachineID;
                    }
                    configurationB = _stateConfigurations[configurationB.parentStateMachineID];

                }
                configurationA = _stateConfigurations[configurationA.parentStateMachineID];
            } while (configurationA.parentStateMachineID >= 0);
            return -1;
        }

        /// <summary>
        /// Update the State Machine
        /// </summary>
        public void Update()
        {
            if (!_initiated)
            {
                Init();
            }
            try
            {
                CheckTransition();
            }
            catch (StateMachineTransitionsOverloadException e)
            {
                Debug.LogError("ERROR: +" + e.Message);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            UpdateState(_currentStateID);
        }

        /// <summary>
        /// Update the State Machine at a framerate independant frequency
        /// </summary>
        public void FixedUpdate()
        {
            if (!_initiated)
            {
                Init();
            }
            try
            {
                CheckTransition();
            }
            catch (StateMachineTransitionsOverloadException e)
            {
                Debug.LogError("ERROR: +" + e.Message);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            FixedUpdateState(_currentStateID);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Return the arborescence of the current state from the State Machine
        /// </summary>
        /// <returns></returns>
        private string FetchArborescence()
        {
            string arborescence = null;
            int stateID = _currentStateID;
            while (stateID >= 0)
            {
                StateConfiguration configuration = _stateConfigurations[stateID];
                if (arborescence == null)
                {
                    arborescence = configuration.stateName;
                }
                else
                {
                    arborescence = configuration.stateName + '.' + arborescence;
                }
                stateID = configuration.parentStateMachineID;
            }
            return arborescence;
        }

        public void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 1000, 20), _arborescence);
        }
#endif

        /// <summary>
        /// Enter in specified state referenced by stateID
        /// </summary>
        /// <param name="stateID"></param>
        private void EnterState(int stateID)
        {
            StateConfiguration stateConfiguration = _stateConfigurations[stateID];
            if (stateConfiguration.stateImplementationID >= 0)
            {
                IState state = _stateImplementations[stateConfiguration.stateImplementationID];
                state.OnEnter();
            }

            for (int i = 0; i < stateConfiguration.transitionsSize; ++i)
            {
                StateTransition stateTransition = _stateTransitions[stateConfiguration.transitionsID + i];
                ITransition transition = _transitions[stateTransition.transitionID];
                transition.OnEnter();
            }

        }

        /// <summary>
        /// Update full arborescence of specified state referenced by stateID
        /// </summary>
        /// <param name="stateID"></param>
        private void UpdateState(int stateID)
        {
            FillStatesStack(_stateIDsStack, stateID);
            while (_stateIDsStack.Count > 0)
            {
                StateConfiguration configuration = _stateConfigurations[_stateIDsStack.Pop()];
                if (configuration.stateImplementationID >= 0)
                {
                    IState state = _stateImplementations[configuration.stateImplementationID];
                    state.OnUpdate();
                }
            }
        }

        /// <summary>
        /// Update full arborescence of specified state referenced by stateID at framerate independant frequency
        /// </summary>
        /// <param name="stateID"></param>
        private void FixedUpdateState(int stateID)
        {
            FillStatesStack(_stateIDsStack, stateID);
            while (_stateIDsStack.Count > 0)
            {
                StateConfiguration configuration = _stateConfigurations[_stateIDsStack.Pop()];
                if (configuration.stateImplementationID >= 0)
                {
                    IState state = _stateImplementations[configuration.stateImplementationID];
                    state.OnFixedUpdate();
                }
            }
        }

        /// <summary>
        /// Exit specified state referenced by stateID
        /// </summary>
        /// <param name="stateID"></param>
        private void ExitState(int stateID)
        {
            StateConfiguration stateConfiguration = _stateConfigurations[stateID];
            for (int i = 0; i < stateConfiguration.transitionsSize; ++i)
            {
                StateTransition stateTransition = _stateTransitions[stateConfiguration.transitionsID + i];
                ITransition transition = _transitions[stateTransition.transitionID];
                transition.OnExit();
            }

            if (stateConfiguration.stateImplementationID >= 0)
            {
                IState state = _stateImplementations[stateConfiguration.stateImplementationID];
                state.OnExit();
            }
        }
    }

    /// <summary>
    /// Interface defining a state implementation
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Called once when the State Machine is initialized
        /// </summary>
        public void OnInit();
        
        /// <summary>
        /// Called when the State Machine enters the state
        /// </summary>
        public void OnEnter();
        
        /// <summary>
        /// Called when the State Machine performs an update with this state
        /// </summary>
        public void OnUpdate();

        /// <summary>
        /// Called when the State Machine performs a framerate independant update with this state
        /// </summary>
        public void OnFixedUpdate();

        /// <summary>
        /// Called when the State Machine exits the state
        /// </summary>
        public void OnExit();
    }

    /// <summary>
    /// Abstract class for state implementation
    /// </summary>
    public abstract class AState : IState
    {
        public virtual void OnInit() { }
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnExit() { }
    }

    /// <summary>
    /// Interface for transition implementation
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        /// Called when the State Machine enters the state linked with this transition
        /// </summary>
        public void OnEnter();
        
        /// <summary>
        /// Evaluates the transition in order to transitate to another state
        /// </summary>
        public bool Evaluate();
        
        /// <summary>
        /// Called when the State Machine exits the state linked with this transition
        /// </summary>
        public void OnExit();

        /// <summary>
        /// Called if the transition if evaluated as true
        /// </summary>
        public void OnTransition();
    }

    /// <summary>
    /// Abstract class for transition implementation
    /// </summary>
    public abstract class ATransition : ITransition
    {
        public virtual void OnEnter() { }
        public abstract bool Evaluate();
        public virtual void OnExit() { }
        public virtual void OnTransition() { }
    }
}

