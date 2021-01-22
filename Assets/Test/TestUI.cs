using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.UI;

public class TestUI : MonoBehaviour
{
    private void Awake()
    {
        UIControl.Instance.Init();
    }
}
