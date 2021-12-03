using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HFSM
{
    public class StateMachineTest
    {


        private StateMachine GenerateSimpleStateMachine()
        {
            return StateMachine.Builder<string>.CreateStateMachine()
                .AddState("A")
                    .EndState()
                .AddState("B")
                    .SetAsStartState()
                    .EndState()
                .AddState("C")
                    .EndState()
                .End();
        }

        // A Test behaves as an ordinary method
        [Test]
        public void StateMachineTest_Transitions()
        {
            // Use the Assert class to test conditions
            bool A_B = false;
            bool B_A = false;
            bool B_C = false;
            bool C_A = false;

            StateMachine stateMachine = StateMachine.Builder<string>.CreateStateMachine()
                .AddState("A")
                    .AddTransition("B", new HFSM.Transition(() => A_B, () => { A_B = false; }))
                    .EndState()
                .AddState("B")
                    .AddTransition("A", new HFSM.Transition(() => B_A, () => { B_A = false; }))
                    .AddTransition("C", new HFSM.Transition(() => B_C, () => { B_C = false; }))
                    .EndState()
                .AddState("C")
                    .AddTransition("A", new HFSM.Transition(() => C_A, () => { C_A = false; }))
                    .EndState()
                .End();

            Assert.AreEqual("A", stateMachine.GetArborescence());
            
            A_B = true;
            stateMachine.Update();

            Assert.AreEqual("B", stateMachine.GetArborescence());

            B_C = true;
            Assert.AreEqual("B", stateMachine.GetArborescence());
            stateMachine.Update();

            Assert.AreEqual("C", stateMachine.GetArborescence());

            C_A = true;
            A_B = true;
            stateMachine.Update();

            Assert.AreEqual("B", stateMachine.GetArborescence());

            // Test for transition priority
            B_A = true;
            B_C = true;
            stateMachine.Update();

            Assert.AreEqual("A", stateMachine.GetArborescence());
        }

        [Test]
        public void StateMachineTest_StartState()
        {
            StateMachine stateMachine = GenerateSimpleStateMachine();

            Assert.AreEqual("B", stateMachine.GetArborescence());
        }
    }
}
