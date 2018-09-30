using System;
using CommonClient;

namespace Client
{
    static class Program
    {
        private static void Main() 
        {
            Console.Title = "ExampleAlpha.Client";
            Console.WriteLine("Press any key to start client.");
            Console.ReadKey();
            Console.WriteLine();

            var serviceClient = new WeatherServiceClient();
            var randomDataClient = new RandomDataClient(() => serviceClient);
            randomDataClient.StartThreads();

            Console.ReadLine();
        }
    }
}
