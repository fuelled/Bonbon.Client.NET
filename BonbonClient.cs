using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonbon.Client.NET
{
    class BonbonClient
    {
        private string appid;
        private string appsecret;
        private string url = "https://api.bonbon.gift/{0}/manage/cards";

        public BonbonClient(string appid, string appsecret, string version)
        {
            url = String.Format(url, version);
        }
        public Card GetCardInfo(string cardNumber)
        {
            return null;
        }

        public Card UpdateCard(string cardNumber, decimal amount)
        {
            return null;
        }
    }
}
