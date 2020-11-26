using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.Utils;
using Mx.UI;

public class Test : MonoBehaviour
{
    private void Awake()
    {
       
    }

    int i = 0;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            i++;
            Toast.Show("点击返回按钮退出应用"+i);
        }
    }

   
}
