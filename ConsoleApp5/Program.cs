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
        public Coin[] result { get; set; }
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
            Console.WriteLine("Введите %, который будет использоваться для подсчета");
            //method for create array of last rate and increased in rate
            int arrayLength;
            CoinInit(urlRequest, float.Parse(Console.ReadLine()), out arrayLength);
            double [] basisArray =  new double [arrayLength];
            basisArray = CoinInit(urlRequest, float.Parse(Console.ReadLine()), out arrayLength);

            //Find coin, which increased more than 0,5%

            FindCoinForBuy(urlRequest, CoinInit(urlRequest, float.Parse(Console.ReadLine()), out arrayLength));

        }
        //Method get for coin last price
        
        private static double[] CoinInit(string url, float rate, out int lengthArray)
        {   // формируем запрос
            WebRequest bittrexApi = WebRequest.Create(url);
            //получаем ответ в поток
            Stream streamBittrex = bittrexApi.GetResponse().GetResponseStream();
            //Создание класса для десериализации
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(InfoCoin));
            //Десериализация файла JSON. Открытие его, десериализация в переменную infoCoin 
            InfoCoin infoCoin = new InfoCoin();
            infoCoin = (InfoCoin)jsonSerializer.ReadObject(streamBittrex);
            double[] resultLastCost = new double[infoCoin.result.Length];
            for (int i = 0; i < infoCoin.result.Length; i++)
                resultLastCost[i] = infoCoin.result[i].Last * rate/100 + infoCoin.result[i].Last;
            lengthArray = infoCoin.result.Length;
            return resultLastCost;
        }
        private static void FindCoinForBuy(string url, double [] basisArray)

        {
            WebRequest bittrexApi = WebRequest.Create(url);
            //получаем ответ в поток
            Stream streamBittrex = bittrexApi.GetResponse().GetResponseStream();
            //Создание класса для десериализации
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(InfoCoin));
            //Десериализация файла JSON. Открытие его, десериализация в переменную infoCoin 
            InfoCoin infoCoin = new InfoCoin();
            infoCoin = (InfoCoin)jsonSerializer.ReadObject(streamBittrex);
            for (int i=0; i<basisArray.Length; i++)
            if (infoCoin.result[i].Last < basisArray [i])
            Console.WriteLine("Наименование пары: {0}, значение: {1}", infoCoin.result[i].Last, infoCoin.result[i].MarketName);

        }
    }
}
