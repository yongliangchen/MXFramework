using Mx.Msg;
using UnityEngine;
using UnityEngine.UI;

namespace Mx.Example
{
    /// <summary>测试消息中心(商店面板)</summary>
    public class StorePanel : MonoBehaviour
    {
        private Text m_CombatEffectiveness;
        private Text m_Gold;
        private Button m_BtnClose;
        private Button m_BtnBuy01;
        private Button m_BtnBuy02;

        private void Awake()
        {
            m_CombatEffectiveness = transform.Find("CombatEffectiveness").GetComponent<Text>();
            m_Gold = transform.Find("Gold").GetComponent<Text>();

            m_BtnClose = transform.Find("BtnClose").GetComponent<Button>();
            m_BtnClose.onClick.AddListener(() => { gameObject.SetActive(false); });

            m_BtnBuy01 = transform.Find("BtnBuy01").GetComponent<Button>();
            m_BtnBuy01.onClick.AddListener(() => { UserDatas.Gold -= 15; });

            m_BtnBuy02 = transform.Find("BtnBuy02").GetComponent<Button>();
            m_BtnBuy02.onClick.AddListener(() => { UserDatas.Gold -= 20; });

            UpdatCombatEffectiveness();
            UpdateGold();

            //注册事件>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            MessageMgr.AddMsgListener("TestMessageCenter", OnTestMessageCenterEvent);
        }

        private void OnDestroy()
        {
            //记得有注册就会有注销>>>>>>>>>>>>>>>>>>>>
            MessageMgr.RemoveMsgListener("TestMessageCenter", OnTestMessageCenterEvent);
        }

        /// <summary>
        /// 监听测试消息中心数据
        /// </summary>
        /// <param name="key">数据名称</param>
        /// <param name="values">具体的数值</param>
        private void OnTestMessageCenterEvent(string key, object values)
        {
            switch (key)
            {
                case "UpdatCombatEffectiveness":

                    int combatEffectiveness = (int)values;//更改后战斗力数值
                    Debug.Log(GetType() + "/OnTestMessageCenterEvent()/更新了战斗力！");

                    UpdatCombatEffectiveness();
                    break;
                case "UpdateGold":

                    int gold = (int)values;//更改后的金币数量
                    Debug.Log(GetType() + "/OnTestMessageCenterEvent()/更新了金币数据量!");
                    UpdateGold();
                    break;
            }
        }

        //更新战斗力显示
        private void UpdatCombatEffectiveness()
        {
            m_CombatEffectiveness.text = "战斗力：" + UserDatas.CombatEffectiveness;
        }

        //更新金币数量显示
        private void UpdateGold()
        {
            m_Gold.text = "金币：" + UserDatas.Gold;
        }
    }
}