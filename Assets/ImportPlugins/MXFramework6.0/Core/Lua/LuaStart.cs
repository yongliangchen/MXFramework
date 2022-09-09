using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mx.Lua
{
    public class LuaStart : MonoBehaviour
    {

        private void Start()
        {
            LuaHelper.Instance.RunScripts("StartGame");
        }

    }
}