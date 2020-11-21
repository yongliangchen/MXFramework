using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.Util;

public class Test : MonoBehaviour
{
  
    void Start()
    {
		string time = "/MXFramework4.1/Assets/ImportPlugins/MXFramework4.1/Core/Util/DirectoryEx.cs";
        Debug.Log(MD5Util.GetStringMd5(time));
        

    }


}
