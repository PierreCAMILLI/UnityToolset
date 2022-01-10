using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolset
{
    namespace HFSM
    {
        public class Transition : ITransition
        {
            private System.Action _onEnter;
            private System.Func<bool> _evaluation;
            private System.Action _onExit;
            private System.Action _onTransition;

            public Transition(System.Func<bool> evaluation)
            {
                _onEnter = null;
                _evaluation = evaluation;
                _onExit = null;
                _onTransition = null;
            }

            public Transition(System.Func<bool> evaluation, System.Action onTransition)
            {
                _onEnter = null;
                _evaluation = evaluation;
                _onExit = null;
                _onTransition = onTransition;
            }

            public Transition(System.Func<bool> evaluation, System.Action onTransition, System.Action onEnter, System.Action onExit)
            {
                _onEnter = onEnter;
                _evaluation = evaluation;
                _onExit = onExit;
                _onTransition = onTransition;
            }

            public virtual void OnEnter()
            {
                _onEnter?.Invoke();
            }

            public bool Evaluate()
            {
                if (_evaluation != null)
                {
                    return _evaluation.Invoke();
                }
                return false;
            }

            public virtual void OnExit()
            {
                _onExit?.Invoke();
            }

            public virtual void OnTransition()
            {
                _onTransition?.Invoke();
            }
        }

        public sealed class TransitionMessage : ITransition
        {
            private static ISet<string> _messages;

            private string _expectedMessage;

            private System.Action _onTransition;

            private TransitionMessage(string message, System.Action onTransition)
            {
                if (_messages == null)
                {
                    _messages = new HashSet<string>();
                }

                if (message != null)
                {
                    _expectedMessage = message;
                }
                else
                {
                    Debug.LogError("No message specified for TransitionMessage.");
                }

                _onTransition = onTransition;
            }

            public static TransitionMessage OnMessage(string message) => new TransitionMessage(message, null);
            public static TransitionMessage OnMessage(string message, System.Action onTransition) => new TransitionMessage(message, onTransition);

            public static void SendMessage(string message)
            {
                if (message != null)
                {
                    if (_messages == null)
                    {
                        _messages = new HashSet<string>();
                    }
                    _messages.Add(message);
                }
            }

            public bool Evaluate() => _messages.Remove(_expectedMessage);

            public void OnEnter() => _messages.Remove(_expectedMessage);
            public void OnExit() => _messages.Remove(_expectedMessage);
            public void OnTransition()
            {
                if (_onTransition != null)
                {
                    _onTransition.Invoke();
                }
            }
        }
    }
}
