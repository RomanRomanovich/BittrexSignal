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
            CoinInit (urlRequest, out int arrayLength);
            double [] basisArray =  new double [arrayLength];
            basisArray = CoinInit(urlRequest, float.Parse(Console.ReadLine()));
            ConsoleKeyInfo key = Console.ReadKey();
            

            while (key.Key != ConsoleKey.Escape)
            { 
                Thread.Sleep(3000);
                FindCoinForBuy(urlRequest, basisArray);
                key = Console.ReadKey();

            }
            //Find coin, which increased more than x%

            Console.ReadLine();

        }
        //Method get for coin last price
        private static double[] CoinInit(string url, float rate)
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
        return resultLastCost;
        }

        private static double[] CoinInit(string url, out int lengthArray)
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
            int colCoin = 0;
           for (int i=0;i<basisArray.Length;++i)
             if (infoCoin.result[i].Last > basisArray[i])
                { 
                    Console.WriteLine("=>Coin {0} grow up. Last: {1}, Curent: {2}!", infoCoin.result[i].MarketName, basisArray[i], infoCoin.result[i].Last);
                    colCoin = ++colCoin;
                }
            Console.WriteLine("Count coin: {0}", colCoin);
            Console.WriteLine(basisArray.Length);
        }
    }
}
