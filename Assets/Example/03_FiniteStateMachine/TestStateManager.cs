using UnityEngine;

namespace Mx.Example
{
    /// <summary>测试有限状态机</summary>
    public class TestStateManager : MonoBehaviour
    {
        private FiniteStateMachine fsm;
        void Awake()
        {
            fsm = new FiniteStateMachine();
            this.FSMRegister();
            fsm.EntryPoint("One");
        }

        void Update()
        {
            fsm.Update();
        }

        void FSMRegister()
        {
            //需要先注册状态
            fsm.Register("One", new TestOneState());
            fsm.Register("Two", new TesTwoState());

            //限制状态直接的切换
            fsm.State("One").On("Two").Enter("Two");
            fsm.State("Two").On("One").Enter("One");
        }

        public void Trigger(string eventName) => fsm.Trigger(eventName);
        public void Trigger(string eventName, object param1) => fsm.Trigger(eventName, param1);
        public void Trigger(string eventName, object param1, object param2) => fsm.Trigger(eventName, param1, param2);
        public void Trigger(string eventName, object param1, object param2, object param3) => fsm.Trigger(eventName, param1, param2, param3);
    }
}