using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Contrib;

namespace Bonbon.Client
{
    public class BonbonClient
    {
        public readonly string AppId;

        public readonly string AppSecret;

        public readonly string BaseUrl;

        private RestClient _client;
        private string _manageCardPath = "manage/cards/{cardNumber}";
        private string _manageBonusDeposite = "manage/cards/deposit/{cardNumber}";
        private string _manageBonusWithdraw = "manage/cards/withdraw/{cardNumber}";

        public BonbonClient(string appId, string appSecret) : this(null, appId, appSecret)
        {
            BaseUrl = "https://api.bonbon.gift/v1";
        }

        public BonbonClient(string baseUrl, string appId, string appSecret)
        {
            AppSecret = appSecret;
            BaseUrl = baseUrl;
            AppId = appId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public IRestResponse<CardDetails> GetCardDetails(string cardNumber)
        {
            var cardRequest = new RestRequest(_manageCardPath, Method.GET);
            cardRequest.AddUrlSegment("cardNumber", cardNumber);
            
            var cardDetails = Client.Execute<CardDetails>(cardRequest);

            return cardDetails;
        }

        public async Task<IRestResponse<CardDetails>> GetCardDetailsAsync(string cardNumber)
        {
            var cardRequest = new RestRequest(_manageCardPath, Method.GET);
            cardRequest.AddUrlSegment("cardNumber", cardNumber);
            Thread.CurrentThread.Join(2000);
            return await Client.ExecuteTaskAsync<CardDetails>(cardRequest);

        }

        public CardDetails ChangeBonus(string cardNumber, double amount)
        {
            var changeBonusRequest = new RestRequest(_manageCardPath, Method.POST);
            changeBonusRequest.RequestFormat = DataFormat.Json;
            changeBonusRequest.AddUrlSegment("cardNumber", cardNumber);
            changeBonusRequest.AddBody(new {cardNumber, amount});
            
            var response = Client.Execute<CardDetails>(changeBonusRequest);
            return response.Data;
        }

        public IRestResponse<CardManageResonse> BonusWithdraw(string cardNumber, double amount)
        {
            var changeBonusRequest = new RestRequest(_manageBonusWithdraw, Method.POST);
            changeBonusRequest.RequestFormat = DataFormat.Json;
            changeBonusRequest.AddUrlSegment("cardNumber", cardNumber);
            changeBonusRequest.AddBody(new { cardNumber, amount });

            var response = Client.Execute<CardManageResonse>(changeBonusRequest);
            return response;
        }

        public IRestResponse<CardManageResonse> BonusDeposit(string cardNumber, double amount)
        {
            var changeBonusRequest = new RestRequest(_manageBonusDeposite, Method.POST);
            changeBonusRequest.RequestFormat = DataFormat.Json;
            changeBonusRequest.AddUrlSegment("cardNumber", cardNumber);
            changeBonusRequest.AddBody(new { cardNumber, amount });

            var response = Client.Execute<CardManageResonse>(changeBonusRequest);
            return response;
        }

        private RestClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new RestClient(BaseUrl);
                    _client.Authenticator = new HMACAuthenticator(AppId, AppSecret);
                }

                return _client;
            }
        }

    }

    class HMACAuthenticator : IAuthenticator
    {
        private readonly string _appId;

        private readonly string _appSecret;

        public HMACAuthenticator(string appId, string appSecret)
        {
            _appId = appId;
            _appSecret = appSecret;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            //string requestContentBase64String = string.Empty;
 
            //Calculate UNIX time
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();
 
            //create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");
 
            //Checking if the request contains body, usually will be null wiht HTTP GET and DELETE

            //var body = request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);
            //if (body != null)
            //{
                
            //    byte[] content = Encoding.UTF8.GetBytes(body.Value.ToString());
            //    MD5 md5 = MD5.Create();
            //    //Hashing the request body, any change in request body will result in different hash, we'll incure message integrity
            //    byte[] requestContentHash = md5.ComputeHash(content);
            //    requestContentBase64String = Convert.ToBase64String(requestContentHash);
            //}
 
            //Creating the raw signature string
            string signatureRawData = String.Format("{0}:{1}:{2}", _appId, requestTimeStamp, nonce);

            var secretKeyByteArray = Convert.FromBase64String(_appSecret);
            
            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);
 
            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                //Setting the values in the Authorization header using custom scheme (amx)
                request.AddHeader("Authorization", string.Format("bb {0}:{1}:{2}:{3}", _appId, requestSignatureBase64String, nonce, requestTimeStamp));
            }
        }
    }
}
