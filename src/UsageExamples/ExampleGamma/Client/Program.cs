using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonClient;
using CommonClient.CustomHandling;

namespace Client
{
    static class Program
    {
        private static void Main() 
        {
            Console.Title = "ExampleGamma.Client";
            Console.WriteLine("Press any key to start client.");
            Console.ReadKey();
            Console.WriteLine();

            // if we use custom handling - each new request should use new client class object

            var randomDataClient = new RandomDataClient(() => new WeatherServiceClientCustomHandler());
            randomDataClient.StartThreads();

            Task.Delay(3000).ContinueWith( _ => Process.Start("explorer.exe", @"C:\SoapLog\Gamma"));

            Console.ReadLine();
        }
    }
}
