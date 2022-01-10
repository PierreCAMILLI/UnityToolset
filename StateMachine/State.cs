using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolset
{
    namespace HFSM
    {
        public class State : IState
        {
            private System.Action _onInit;
            private System.Action _onEnter;
            private System.Action _onUpdate;
            private System.Action _onFixedUpdate;
            private System.Action _onExit;

            public State(System.Action onUpdate)
            {
                _onInit = null;
                _onEnter = null;
                _onUpdate = onUpdate;
                _onFixedUpdate = null;
                _onExit = null;
            }

            public State(System.Action onEnter, System.Action onExit)
            {
                _onInit = null;
                _onEnter = onEnter;
                _onUpdate = null;
                _onFixedUpdate = null;
                _onExit = onExit;
            }

            public State(System.Action onInit, System.Action onEnter, System.Action onUpdate, System.Action onFixedUpdate, System.Action onExit)
            {
                _onInit = onInit;
                _onEnter = onEnter;
                _onUpdate = onUpdate;
                _onFixedUpdate = onFixedUpdate;
                _onExit = onExit;
            }

            public void OnEnter()
            {
                if (_onEnter != null)
                {
                    _onEnter.Invoke();
                }
            }

            public void OnExit()
            {
                if (_onExit != null)
                {
                    _onExit.Invoke();
                }
            }

            public void OnFixedUpdate()
            {
                if (_onFixedUpdate != null)
                {
                    _onFixedUpdate.Invoke();
                }
            }

            public void OnInit()
            {
                if (_onInit != null)
                {
                    _onInit.Invoke();
                }
            }

            public void OnUpdate()
            {
                if (_onUpdate != null)
                {
                    _onUpdate.Invoke();
                }
            }
        }

#if UNITY_EDITOR
        public class DebugState : AState
        {
            private string _messageOnEnter;
            private string _messageOnExit;

            public DebugState(string messageOnEnter = null, string messageOnExit = null)
            {
                _messageOnEnter = messageOnEnter;
                _messageOnExit = messageOnExit;
            }

            public override void OnEnter()
            {
                if (_messageOnEnter != null)
                {
                    Debug.Log(_messageOnEnter);
                }
            }

            public override void OnExit()
            {
                if (_messageOnExit != null)
                {
                    Debug.Log(_messageOnExit);
                }
            }
        }
#endif
    }
}