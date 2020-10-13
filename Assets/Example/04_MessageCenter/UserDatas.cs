using Mx.Msg;

namespace Mx.Example
{
    /// <summary>测试消息中心(用户数据)</summary>
    public class UserDatas
    {
        /// <summary>用户名称</summary>
        public static string Name { get; set; } = "张三";

        private static int combatEffectiveness = 0;
        /// <summary>用户战斗力</summary>
        public static int CombatEffectiveness
        {
            get
            {
                return combatEffectiveness;
            }
            set
            {
                combatEffectiveness = value;
                MessageMgr.SendMessage("TestMessageCenter", "UpdatCombatEffectiveness", combatEffectiveness);
            }
        }


        private static int gold = 3000;
        /// <summary>用户金币数量</summary>
        public static int Gold
        {
            get
            {
                return gold;
            }
            set
            {
                gold = value;
                if (gold < 0) gold = 0;//限制出现负数情况
                MessageMgr.SendMessage("TestMessageCenter", "UpdateGold", gold);
            }
        }
    }
}