using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;

namespace ConsoleApp5

{ public class InfoCoin
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Coin [] result { get; set; }
    }

    public class Coin
    {
        public string MarketName { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Volume { get; set; }
        public float Last { get; set; }
        public float BaseVolume { get; set; }
        public string TimeStamp { get; set; }
        public float Bid { get; set; }
        public float Ask { get; set; }
        public int OpenBuyOrders { get; set; }
        public int OpenSellOrders { get; set; }
        public float PrevDay { get; set; }
        public string Created { get; set; }
    }

       
    class Program
    {

        static void Main(string[] args)
        {   //указываем ссылку для запроса
            string urlRequest = "https://bittrex.com/api/v1.1/public/getmarketsummaries";
            
            Console.ReadLine();

        }
        //Method get for coin last price
        //Increased in % 
        //And return array value
        private static double [] CoinInit(string url, float rate)
        {   // формируем запрос
            WebRequest bittrexApi = WebRequest.Create(url);
            //получаем ответ в поток
            Stream streamBittrex = bittrexApi.GetResponse().GetResponseStream();
            //Создание класса для десериализации
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(InfoCoin));
            //Десериализация файла JSON. Открытие его, десериализация в переменную infoCoin 
            InfoCoin infoCoin = new InfoCoin();
            infoCoin = (InfoCoin)jsonSerializer.ReadObject(streamBittrex);
            double[] resultLastCost = new double [infoCoin.result.Length];
            for (int i=0;i<infoCoin.result.Length;i++)
               resultLastCost[i] = infoCoin.result[i].Last*rate+infoCoin.result[i].Last;
            return resultLastCost;//f
        }
        private static void FindCoinForBuy()
        {  Console.WriteLine("Наименование пары: {0}, значение: {1}", );
        }
        {

        }
    }
}
