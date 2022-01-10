using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Toolset
{
    namespace HFSM
    {
        public partial class StateMachine
        {

            public class Builder<E>
            {
                private IDictionary<E, int> _stateNames;
                private List<StateConfiguration> _stateConfigurations;
                private List<List<StateTransitionWithName>> _stateTransitions;
                private List<IState> _stateImplementations;
                private List<ITransition> _transitions;

                private int _configuringStateID;
                private int _firstStateID;

                private class StateTransitionWithName
                {
                    public bool checkTransitionOnLink;
                    public E nextStateName;
                    public StateTransition stateTransition;

                    public void SetNextStateInfos(int nextStateID, int commonParentID)
                    {
                        stateTransition.nextStateID = nextStateID;
                        stateTransition.commonParentID = commonParentID;
                    }
                }

                private Builder()
                {
                    _stateNames = new Dictionary<E, int>();
                    _stateConfigurations = new List<StateConfiguration>();
                    _stateTransitions = new List<List<StateTransitionWithName>>();
                    _stateImplementations = new List<IState>();
                    _transitions = new List<ITransition>();

                    _configuringStateID = -1;
                    _firstStateID = 0;
                }

                /// <summary>
                /// Start the creation of a new State Machine
                /// </summary>
                /// <returns></returns>
                public static Builder<E> CreateStateMachine()
                {
                    return new Builder<E>();
                }

                /// <summary>
                /// Add a new state to the State Machine
                /// </summary>
                /// <param name="stateName">Name of the state</param>
                /// <returns></returns>
                public Builder<E> AddState(E stateName)
                {
                    if (_stateNames.ContainsKey(stateName))
                    {
                        Debug.LogError("State " + stateName.ToString() + " is already in StateMachine");
                    }

                    // Check if we opened a sub-state machine
                    if (_configuringStateID >= 0)
                    {
                        StateConfiguration configuration = _stateConfigurations[_configuringStateID];
                        if (configuration.subStateMachineID < 0)
                        {
                            configuration.subStateMachineID = _stateConfigurations.Count;
                            _stateConfigurations[_configuringStateID] = configuration;
                        }
                    }

                    int parentStateID = _configuringStateID;

                    _stateNames.Add(stateName, _stateConfigurations.Count);
                    _configuringStateID = _stateConfigurations.Count;
                    _stateConfigurations.Add(new StateConfiguration
                    {
#if UNITY_EDITOR
                        stateName = stateName.ToString(),
#endif
                        stateImplementationID = -1,
                        subStateMachineID = -1,
                        parentStateMachineID = parentStateID //_configuringParentStateMachineID
                    });
                    _stateTransitions.Add(new List<StateTransitionWithName>());
                    return this;
                }

                /// <summary>
                /// Add a sub-State Machine from another State Machine
                /// </summary>
                /// <param name="subStateMachine"></param>
                /// <returns></returns>
                public Builder<E> AddSubStateMachine(StateMachine subStateMachine)
                {
                    if (subStateMachine._stateConfigurations.Count() == 0)
                    {
                        return this;
                    }
                    StateConfiguration configuration = _stateConfigurations[_configuringStateID];
                    configuration.subStateMachineID = _configuringStateID + 1;
                    _stateConfigurations[_configuringStateID] = configuration;

                    _stateTransitions.AddRange(subStateMachine._stateConfigurations.Select(sc =>
                    {
                        List<StateTransitionWithName> stateTransitionWithNames = subStateMachine._stateTransitions
                            .Skip(sc.transitionsID).Take(sc.transitionsSize)
                            .Select(st => new StateTransitionWithName
                            {
                                checkTransitionOnLink = false,
                                nextStateName = default(E),
                                stateTransition = new StateTransition
                                {
#if UNITY_EDITOR
                                previousStateName = st.previousStateName,
                                    nextStateName = st.nextStateName,
#endif
                                nextStateID = _stateConfigurations.Count + st.nextStateID,
                                    commonParentID = _stateConfigurations.Count + st.commonParentID,
                                    transitionID = _transitions.Count + st.transitionID,
                                }
                            }).ToList();
                        return stateTransitionWithNames;

                    }));
                    _stateConfigurations.AddRange(subStateMachine._stateConfigurations.Select(sc => {
                        return new StateConfiguration
                        {
#if UNITY_EDITOR
                            stateName = sc.stateName,
#endif
                            parentStateMachineID = sc.parentStateMachineID < 0 ? _configuringStateID : _configuringStateID + sc.parentStateMachineID,
                            subStateMachineID = sc.subStateMachineID < 0 ? -1 : _configuringStateID + sc.subStateMachineID,
                            stateImplementationID = sc.stateImplementationID < 0 ? -1 : sc.stateImplementationID + _stateImplementations.Count,
                            transitionsID = sc.transitionsID + _transitions.Count,
                            transitionsSize = sc.transitionsSize,
                        };
                    }));
                    _stateImplementations.AddRange(subStateMachine._stateImplementations);
                    _transitions.AddRange(subStateMachine._transitions);
                    return this;
                }

                /// <summary>
                /// Define an implementation of the currently handled state
                /// </summary>
                /// <param name="state"></param>
                /// <returns></returns>
                public Builder<E> SetState(IState state)
                {
                    StateConfiguration configuration = _stateConfigurations[_configuringStateID];
                    configuration.stateImplementationID = _stateImplementations.Count;
                    _stateConfigurations[_configuringStateID] = configuration;
                    _stateImplementations.Add(state);
                    return this;
                }

                /// <summary>
                /// Add a transition to the currently handled state
                /// </summary>
                /// <param name="nextStateName"></param>
                /// <param name="transition"></param>
                /// <returns></returns>
                public Builder<E> AddTransition(E nextStateName, ITransition transition)
                {
                    int nextStateID = -1;
                    if (_stateNames.ContainsKey(nextStateName))
                    {
                        nextStateID = _stateNames[nextStateName];
                    }
                    IList<StateTransitionWithName> stateTransitions = _stateTransitions[_configuringStateID];
                    stateTransitions.Add(new StateTransitionWithName
                    {
                        checkTransitionOnLink = true,
                        nextStateName = nextStateName,
                        stateTransition = new StateTransition
                        {
#if UNITY_EDITOR
                            previousStateName = _stateConfigurations[_configuringStateID].stateName,
                            nextStateName = nextStateName.ToString(),
#endif
                            nextStateID = nextStateID,
                            transitionID = _transitions.Count
                        }
                    });
                    _transitions.Add(transition);
                    return this;
                }

                /// <summary>
                /// Finish the handling of the current state
                /// </summary>
                /// <returns></returns>
                public Builder<E> EndState()
                {
                    _configuringStateID = _stateConfigurations[_configuringStateID].parentStateMachineID;
                    return this;
                }

                /// <summary>
                /// Set currently handled state as start state from the parent state
                /// </summary>
                /// <returns></returns>
                public Builder<E> SetAsStartState()
                {
                    int parentStateID = _stateConfigurations[_configuringStateID].parentStateMachineID;
                    if (parentStateID >= 0)
                    {
                        StateConfiguration parentConfiguration = _stateConfigurations[parentStateID];
                        parentConfiguration.subStateMachineID = _configuringStateID;
                        _stateConfigurations[parentStateID] = parentConfiguration;
                    }
                    else
                    {
                        _firstStateID = _configuringStateID;
                    }
                    return this;
                }

                private void LinkTransitions()
                {
                    int lastTransitionID = 0;
                    for (int i = 0; i < _stateTransitions.Count; ++i)
                    {
                        StateConfiguration configuration = _stateConfigurations[i];
                        configuration.transitionsID = lastTransitionID;
                        configuration.transitionsSize = _stateTransitions[i].Count;
                        _stateConfigurations[i] = configuration;
                        lastTransitionID += _stateTransitions[i].Count;

                        for (int j = 0; j < _stateTransitions[i].Count; ++j)
                        {
                            E stateName = _stateTransitions[i][j].nextStateName;
                            if (_stateTransitions[i][j].checkTransitionOnLink && _stateNames.ContainsKey(stateName))
                            {
                                if (_stateNames.ContainsKey(stateName))
                                {
                                    int nextStateID = FindLastChildOf(_stateNames[stateName]);
                                    int commonParentID = FindCommonParent(i, nextStateID);
                                    _stateTransitions[i][j].SetNextStateInfos(nextStateID, commonParentID);
                                }
#if UNITY_EDITOR
                                else
                                {
                                    Debug.LogError("State " + _stateTransitions[i][j].stateTransition.nextStateName + " doesn't exists in StateMachine.");
                                }
#endif
                            }
                        }

                    }
                }

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
                        if (configurationA.parentStateMachineID >= 0)
                        {
                            configurationA = _stateConfigurations[configurationA.parentStateMachineID];
                        }
                    } while (configurationA.parentStateMachineID >= 0);
                    return -1;
                }

                private int FindLastChildOf(int stateID)
                {
                    StateConfiguration configuration = _stateConfigurations[stateID];
                    while (configuration.subStateMachineID >= 0)
                    {
                        stateID = configuration.subStateMachineID;
                        configuration = _stateConfigurations[stateID];
                    }
                    return stateID;
                }

                /// <summary>
                /// Finish the creation and return the State Machine
                /// </summary>
                /// <returns></returns>
                public StateMachine End()
                {
                    LinkTransitions();

                    StateMachine stateMachine = new StateMachine();
                    stateMachine._stateConfigurations = _stateConfigurations.ToArray();
                    stateMachine._stateImplementations = _stateImplementations.ToArray();
                    stateMachine._transitions = _transitions.ToArray();

                    IEnumerable<StateTransitionWithName> stateTransitionWithNames = new List<StateTransitionWithName>();
                    stateMachine._stateTransitions = _stateTransitions
                                                    .Aggregate(stateTransitionWithNames, (current, s) => current.Concat(s))
                                                    .Select(st => st.stateTransition).ToArray();
                    stateMachine._currentStateID = _firstStateID;
#if UNITY_EDITOR
                    stateMachine._arborescence = stateMachine.FetchArborescence();
#endif
                    return stateMachine;
                }
            }
        }
    }
}
