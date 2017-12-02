using System;
using CommonClient;

namespace Client
{
    static class Program
    {
        private static void Main() {
            Console.Title = "ExampleDelta.Client";
            Console.WriteLine("Press any key to start client.");
            Console.ReadKey();
            Console.WriteLine();

            var serviceClient = new WeatherServiceClient();

            serviceClient.ClientCredentials.UserName.UserName = "admin";
            serviceClient.ClientCredentials.UserName.Password = "password";

            var randomDataClient = new RandomDataClient(serviceClient);
            randomDataClient.StartThreads();

            Console.ReadLine();
        }
    }
}
