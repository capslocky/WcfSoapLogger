using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.DatabaseService;
using WcfSoapLogger;
using WcfSoapLogger.CustomHandlers;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
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

                try
                {
                    SendRequest(requestId);
                    //                Thread.Sleep(TimeSpan.FromMilliseconds(random.Next(0, 300)));    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
        }

        private static void SendRequest(int id)
        {
            Console.WriteLine(id + ": sending.");
            var client = new DatabaseClient();

            var juiceInfo = new JuiceInfo();
            juiceInfo.Name = "Amazing lemon";
            juiceInfo.Id = id;

            SoapLoggerClient.SetCustomHandlerCallbacks(new MyClientCustomHandler());

            var similarArray = client.FindSimilar(juiceInfo);
            Console.WriteLine(id + ": finished. Count: " + similarArray.Length);
        }

    }

    public class MyClientCustomHandler : ISoapLoggerHandlerClient
    {
        private const string LogDirectory = @"C:\SoapLogCustomClient";

        public void HandleRequestBodyCallback(byte[] requestBody, SoapLoggerSettings settings)
        {
            //            throw new Exception("Problem in Client - HandleRequestBodyCallback");
            SoapLoggerTools.WriteFileDefault(requestBody, true, LogDirectory);
        }

        public void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            //            throw new Exception("Problem in Client - HandleResponseBodyCallback");
            SoapLoggerTools.WriteFileDefault(responseBody, false, LogDirectory);
        }

        public void CustomHandlersDisabledCallback(SoapLoggerSettings settings)
        {
            Console.WriteLine("CustomHandlersDisabledCallback");
        }
    }



}
