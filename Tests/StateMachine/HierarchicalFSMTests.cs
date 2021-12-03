using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HFSM
{
    public class HierarchicalFSMTests
    {
        private List<string> _callstack;
        private bool _AAAtoABAA = false;

        public class CallstackState : AState
        {
            private IList<string> _callstack;
            private string _stateName;

            public CallstackState(IList<string> callstack, string stateName) : base()
            {
                _callstack = callstack;
                _stateName = stateName;
            }

            public override void OnEnter() => _callstack.Add("Enter:" + _stateName);

            public override void OnExit() => _callstack.Add("Exit:" + _stateName);
        }

        private StateMachine GenerateStateMachine()
        {
            return StateMachine.Builder<string>.CreateStateMachine()
                .AddState("A").SetState(new CallstackState(_callstack, "A"))
                    .AddTransition("ABAA", TransitionMessage.OnMessage("e"))
                    .AddState("AA").SetState(new CallstackState(_callstack, "AA"))
                        .AddTransition("AA", TransitionMessage.OnMessage("a"))
                        .AddTransition("AAA", TransitionMessage.OnMessage("b"))
                        .AddTransition("AB", TransitionMessage.OnMessage("c"))
                        .AddTransition("A", TransitionMessage.OnMessage("d"))
                        .AddTransition("ABAA", TransitionMessage.OnMessage("f"))
                        .AddTransition("ABAA", new Transition(() => _AAAtoABAA))
                        .AddState("AAA").SetState(new CallstackState(_callstack, "AAA"))
                            .AddTransition("ABAA", TransitionMessage.OnMessage("g"))
                            .EndState()
                        .EndState()
                    .AddState("AB").SetState(new CallstackState(_callstack, "AB"))
                        .AddTransition("AA", TransitionMessage.OnMessage("c"))
                        .AddTransition("AAA", TransitionMessage.OnMessage("f"))
                        .AddState("ABA").SetState(new CallstackState(_callstack, "ABA"))
                            .AddTransition("ABAA", TransitionMessage.OnMessage("b"))
                            .AddState("ABAA").SetState(new CallstackState(_callstack, "ABAA"))
                                .AddTransition("ABA", TransitionMessage.OnMessage("d"))
                                .EndState()
                            .EndState()
                        .EndState()
                    .EndState()
                .End();
        }

        // A Test behaves as an ordinary method
        [Test]
        public void HierarchicalFSMTests_Transitions()
        {
            // Use the Assert class to test conditions
            StateMachine stateMachine = GenerateStateMachine();

            stateMachine.Update();
            Assert.AreEqual("A.AA.AAA", stateMachine.GetArborescence());

            TransitionMessage.SendMessage("g");

            stateMachine.Update();
            Assert.AreEqual("A.AB.ABA.ABAA", stateMachine.GetArborescence());

            TransitionMessage.SendMessage("d");

            stateMachine.Update();
            Assert.AreEqual("A.AB.ABA.ABAA", stateMachine.GetArborescence());

            TransitionMessage.SendMessage("f");

            stateMachine.Update();
            Assert.AreEqual("A.AA.AAA", stateMachine.GetArborescence());

            TransitionMessage.SendMessage("e");

            stateMachine.Update();
            Assert.AreEqual("A.AB.ABA.ABAA", stateMachine.GetArborescence());
        }

        // A Test behaves as an ordinary method
        [Test]
        public void HierarchicalFSMTests_Callstack()
        {
            // Use the Assert class to test conditions
            _callstack = new List<string>();
            StateMachine stateMachine = GenerateStateMachine();

            stateMachine.Update();
            CollectionAssert.AreEqual(new[] {
                "Enter:A",
                "Enter:AA",
                "Enter:AAA"
            }, _callstack.ToArray());
            _callstack.Clear();

            TransitionMessage.SendMessage("g");

            stateMachine.Update();
            CollectionAssert.AreEqual(new[] {
                "Exit:AAA",
                "Exit:AA",
                "Enter:AB",
                "Enter:ABA",
                "Enter:ABAA"
            }, _callstack.ToArray());
            _callstack.Clear();

            TransitionMessage.SendMessage("d");

            stateMachine.Update();
            CollectionAssert.AreEqual(new string[] {
            }, _callstack.ToArray());
            _callstack.Clear();

            TransitionMessage.SendMessage("c"); // ABAA => AAA
            _AAAtoABAA = true;

            stateMachine.Update();
            CollectionAssert.AreEqual(new[] {
                "Exit:ABAA",
                "Exit:ABA",
                "Exit:AB",
                "Enter:AA",
                "Enter:AAA",
                "Exit:AAA",
                "Exit:AA",
                "Enter:AB",
                "Enter:ABA",
                "Enter:ABAA"
            }, _callstack.ToArray());
            _callstack.Clear();

            _AAAtoABAA = false;
            TransitionMessage.SendMessage("e");

            stateMachine.Update();
            CollectionAssert.AreEqual(new string[] {
            }, _callstack.ToArray());
            _callstack.Clear();
        }
    }
}
