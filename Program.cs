using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bonbon.Client.NET
{
    class Card
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Bonus { get; set; }
    }

    class UpdateCard
    {
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
    }

    class Program
    {
        private const string API_URL = "https://api.bonbon.gift/";

        // Во время релиза нужно поменять на 
        // v1/manage/cards
        // TEST123456780000 является номером тестовой карты
        private const string TEST_CARD_ENDPOINT = "v1/test/manage/cards/TEST123456780000";

        //APPID нужно заменить 
        private const string APPID = "YW55IGNhcm5hbCBwbGVhcw";

        //APPSECRET нужно заменить
        private const string APPSECRET = "Jx893KSAx5hgZWFzdXJlLg";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting the console client...");

            Console.WriteLine("\nGet the card information...");
            GetCardInfoAsync().Wait();

            Console.WriteLine("\nUpdate the card bonus...");
            UpdateCardInfoAsync().Wait();
        }

        // Функция для запроса информации об карточке
        static async Task GetCardInfoAsync()
        {
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var endpoint = String.Format("{0}?appid={1}&appsecret={2}", TEST_CARD_ENDPOINT, APPID, APPSECRET);

                HttpResponseMessage response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var card = await response.Content.ReadAsAsync<Card>();

                    Console.WriteLine("Card number: {0}", card.CardNumber);
                    Console.WriteLine("Card holder name: {0}", card.CardHolderName);
                    Console.WriteLine("Bonus: {0}", card.Bonus);
                    Console.WriteLine("Created date: {0}", card.CreatedDate);
                    Console.WriteLine("Expiration date: {0}", card.ExpirationDate);
                    Console.WriteLine("Last activity date: {0}", card.LastActivityDate);
                }

            }
        }

        // Функция для обновления бонуса на карточке. Если "amount" 
        // может быть положительным и отрицательным в зависимости от операции добавления или снятия
        static async Task UpdateCardInfoAsync()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var endpoint = String.Format("{0}?appid={1}&appsecret={2}", TEST_CARD_ENDPOINT, APPID, APPSECRET);
                var updatedCard = new UpdateCard { Amount = -450, CardNumber = "TEST123456780000" };

                HttpResponseMessage response = await client.PostAsJsonAsync(endpoint, updatedCard);

                if (response.IsSuccessStatusCode)
                {
                    //var card = await response.Content.ReadAsAsync<Card>();

                    //Console.WriteLine("Card number: {0}", card.CardNumber);
                    //Console.WriteLine("Card holder name: {0}", card.CardHolderName);
                    //Console.WriteLine("Bonus: {0}", card.Bonus);
                    //Console.WriteLine("Created date: {0}", card.CreatedDate);
                    //Console.WriteLine("Expiration date: {0}", card.ExpirationDate);
                    //Console.WriteLine("Last activity date: {0}", card.LastActivityDate);
                    Console.WriteLine("Card successfully updated");
                }

            }
        }
    }
}
