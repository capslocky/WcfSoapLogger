using System;
using CommonClient;
using CommonClient.CustomHandling;

namespace Client
{
    static class Program
    {
        private static void Main() {
            Console.Title = "ExampleGamma.Client";
            Console.WriteLine("Press any key to start client.");
            Console.ReadKey();
            Console.WriteLine();

            var serviceClient = new WeatherServiceClient();
            var randomDataClient = new RandomDataClientCustomHandler(serviceClient);
            randomDataClient.StartThreads();

            Console.ReadLine();
        }
    }
}
