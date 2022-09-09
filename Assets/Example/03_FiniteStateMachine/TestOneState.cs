using UnityEngine;

namespace Mx.Example
{
    /// <summary>测试状态1</summary>
    public class TestOneState : IState
    {
        public void OnEnter(string prevState)
        {
            Debug.Log(GetType() + "/OnEnter()/prevState:"+ prevState);
        }

        public void OnExit(string nextState)
        {
            Debug.Log(GetType() + "/OnExit()/nextState:"+ nextState);
        }

        public void OnUpdate()
        {

        }
    }
}