using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.UI;

namespace Mx.Example
{
    public class TestUI : MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance.OpenUIForms(UIFormNames.LOGIN_UIFORM);
        }
    }
}