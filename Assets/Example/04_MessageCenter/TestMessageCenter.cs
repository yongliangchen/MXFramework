using UnityEngine;
using UnityEngine.UI;

namespace Mx.Example
{
    /// <summary>测试消息中心</summary>
    public class TestMessageCenter : MonoBehaviour
    {
        private int m_CombatEffectiveness;
        private GameObject m_StorePanel;
        private Button m_BtnStore;

        private void Awake()
        {
            m_StorePanel = transform.Find("StorePanel").gameObject;
            m_StorePanel.SetActive(false);
            m_BtnStore = transform.Find("BtnStore").GetComponent<Button>();
            m_BtnStore.onClick.AddListener(() => { m_StorePanel.SetActive(true); });

            InvokeRepeating("AddCombatEffectiveness", 1f, 1f);
        }

        //测试增加战斗力
        private void AddCombatEffectiveness()
        {
            m_CombatEffectiveness++;
            TestUserDatas.CombatEffectiveness = m_CombatEffectiveness;
        }
    }
}