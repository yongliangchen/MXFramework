using UnityEngine;
using Mx.UI;
using Mx.Log;

public class ExampleClass : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeRuntimeInitializeOnLoadMethod()
    {
        //Debug.Log("Init Application!  dataPath:"+ Application.persistentDataPath);

        DebugManager.Instance.Init();
        UIRoot.Instance.Init();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void OnRuntimeMethodLoad()
    {
        //Debug.Log("RuntimeInitializeOnLoadMethod");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AfterRuntimeInitializeOnLoadMethod()
    {
        //Debug.Log("AfterRuntimeInitializeOnLoadMethod");
    }
}