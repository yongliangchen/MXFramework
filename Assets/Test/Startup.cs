using UnityEngine;
using Mx.UI;

public class ExampleClass : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeRuntimeInitializeOnLoadMethod()
    {
        Debug.Log("BeforeRuntimeInitializeOnLoadMethod");
        UIRoot.Instance.Init();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void OnRuntimeMethodLoad()
    {
        Debug.Log("RuntimeInitializeOnLoadMethod");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AfterRuntimeInitializeOnLoadMethod()
    {
        Debug.Log("AfterRuntimeInitializeOnLoadMethod");
    }
}