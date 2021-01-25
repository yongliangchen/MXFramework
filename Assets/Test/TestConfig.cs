using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.Config;

public class TestConfig : MonoBehaviour
{
    private UIConfigDatabase uIConfig;

    private void Awake()
    {
        uIConfig = ConfigManager.Instance.GetDatabase<UIConfigDatabase>();

        Debug.Log(uIConfig.GetCount());

      foreach(var info in uIConfig.GetAllDataArray())
        {
            Debug.Log(info.Name);
        }


    }
}

  
