using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Startup
{
    static Startup()
    {
        Debug.Log("Unity 启动了！");
    }
}