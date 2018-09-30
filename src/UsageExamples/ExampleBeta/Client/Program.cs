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
            Console.Title = "ExampleBeta.Client";
            Console.WriteLine("Press any key to start client.");
            Console.ReadKey();
            Console.WriteLine();

            // if no custom handling - it's ok to reuse client class object

            var serviceClient = new WeatherServiceClient();
            var randomDataClient = new RandomDataClient(() => serviceClient);
            randomDataClient.StartThreads();

            Task.Delay(500).ContinueWith(_ => Process.Start("explorer.exe", @"C:\SoapLog\Beta"));

            Console.ReadLine();
        }
    }
}
