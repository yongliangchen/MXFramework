using UnityEngine;
using UnityEngine.UI;

namespace Mx.Example
{
    /// <summary>测试有限状态机</summary>
    public class TestFiniteStateMachine : MonoBehaviour
    {
        public Button m_BtnSwitch;

        private TestStateManager m_TestStateManager;
        private string m_StateName="One";

        private void Awake()
        {
            m_TestStateManager = FindObjectOfType<TestStateManager>();
            if (m_TestStateManager == null) m_TestStateManager = gameObject.AddComponent<TestStateManager>();

            m_BtnSwitch.onClick.AddListener(onClickSwitchButton);
        }

        /// <summary>点击切换状态按钮</summary>
        private void onClickSwitchButton()
        {
            switchState();
        }

        /// <summary>状态</summary>
        private void switchState()
        {
            if (m_StateName.Equals("One")) m_StateName = "Two";
            else m_StateName = "One";
            m_BtnSwitch.transform.Find("Text").GetComponent<Text>().text = (m_StateName.Equals("One")? "切换状态Two" : "切换状态One");

            m_TestStateManager.Trigger(m_StateName);
        }

    }
}