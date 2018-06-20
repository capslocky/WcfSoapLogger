using System;
using System.Diagnostics;
using System.ServiceModel;
using CommonService.CustomHandling;

namespace Service
{
    static class Program
    {
        private static void Main() 
        {
            Console.Title = "ExampleGamma.Service";

            var serviceHost = new ServiceHost(typeof(WeatherServiceEuropeCustomHandler));

            try
            {
                serviceHost.Open();
            }
            catch (AddressAccessDeniedException)
            {
                Console.WriteLine("ERROR. Please run this application with needed rights.");
                throw;
            }

            Console.WriteLine("Service started.");

            Process.Start("http://localhost:5580/weatherService");

            Console.WriteLine("Press Enter to stop.");
            Console.ReadLine();

            Console.WriteLine("Service is stopping...");
            serviceHost.Close();

            Console.WriteLine("Service stopped.");
            Console.WriteLine();
        }
    }
}
