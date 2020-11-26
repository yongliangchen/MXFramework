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
                UIManager.Instance.SendGlobalUIFormMsg("ChangeCoin", coin);
            }

        } 
    }
}