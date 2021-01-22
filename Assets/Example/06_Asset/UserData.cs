using Mx.UI;

namespace Mx.Example
{
    public class UserData 
    {
        private static int coin = 40000;
        public static int Coin
        {
            get { return coin; }
            set
            {
                coin = value;
                if (coin <= 0) coin = 0;
                //UIManager.Instance.SendGlobalUIFormMsg("ChangeCoin", coin);
            }

        }

        /// <summary>战斗力</summary>
        public static int Capability { get; set; } = 25;
       
    }
}