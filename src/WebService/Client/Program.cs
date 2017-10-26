using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.DatabaseService;
using WcfSoapLogger;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start client.");
            Console.ReadKey();

            SpawnThreads();
            Console.ReadLine();
        }

        private static void SpawnThreads()
        {
            int threadCount = 5;

            for (int i = 0; i < threadCount; i++)
            {
                int id = i;
                Task.Factory.StartNew(() => SendMultipleRequests(id));
            }
        }

        private static void SendMultipleRequests(int id)
        {
//            Random random = new Random();

            int messageCount = 10;

            for (int i = 0; i < messageCount; i++)
            {
                int requestId = 1000 * id + i;
                SendRequest(requestId);
//                Thread.Sleep(TimeSpan.FromMilliseconds(random.Next(0, 300)));
            }
        }

        private static void SendRequest(int id)
        {
            Console.WriteLine(id + ": sending.");
            var client = new DatabaseClient();

            var juiceInfo = new JuiceInfo();
            juiceInfo.Name = "Amazing lemon";
            juiceInfo.Id = id;


            SoapLoggerForClient.SetRequestAndResponseCallbacks(RequestCallback, ResponseCallback);

            var similarArray = client.FindSimilar(juiceInfo);
            Console.WriteLine(id + ": finished. Count: " + similarArray.Length);
        }


        private const string LogDirectory = @"C:\SoapLogCustomClient";

        private static void RequestCallback(byte[] requestBody, SoapLoggerSettings settings) 
        {
//            throw new Exception("oops");
            SoapLoggerTools.WriteFileDefault(requestBody, true, LogDirectory);
        }

        private static void ResponseCallback(byte[] responseBody, SoapLoggerSettings settings) 
        {
//            throw new Exception("oops");
            SoapLoggerTools.WriteFileDefault(responseBody, false, LogDirectory);
        }


    }

    
}
