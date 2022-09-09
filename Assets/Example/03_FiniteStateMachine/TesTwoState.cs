using UnityEngine;

/// <summary>测试状态2</summary>
public class TesTwoState : IState
{
    public void OnEnter(string prevState)
    {
        Debug.Log(GetType() + "/OnEnter()/prevState:" + prevState);
    }

    public void OnExit(string nextState)
    {
        Debug.Log(GetType() + "/OnExit()/nextState:" + nextState);
    }

    public void OnUpdate()
    {
       
    }
}
