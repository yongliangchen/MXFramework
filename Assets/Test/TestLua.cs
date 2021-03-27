using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.Lua;

public class TestLua : MonoBehaviour
{
    private void Awake()
    {
        // LuaHelper.Instance.DoString();

        LuaHelper.Instance.DoString("testLua");
        //LuaHelper.Instance.DoString("require 'testLua'");
    }
}
