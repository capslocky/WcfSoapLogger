using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonClient;

namespace Client
{
    static class Program
    {
        private static void Main() 
        {
            Console.Title = "ExampleDelta.Client";
            Console.WriteLine("Press any key to start client.");
            Console.ReadKey();
            Console.WriteLine();

            var serviceClient = new WeatherServiceClient();

            serviceClient.ClientCredentials.UserName.UserName = "neo";
            serviceClient.ClientCredentials.UserName.Password = "matrix";

            var randomDataClient = new RandomDataClient(serviceClient);
            randomDataClient.StartThreads();

            Task.Delay(500).ContinueWith(_ => Process.Start("explorer.exe", @"C:\SoapLog\Delta"));

            Console.ReadLine();
        }
    }
}
