using System;
using System.Diagnostics;
using System.ServiceModel;
using CommonService;

namespace Service
{
    static class Program
    {
        private static void Main() {
            Console.Title = "ExampleSigma.Service";

            var serviceHost = new ServiceHost(typeof(WeatherServiceEurope));

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

            Console.WriteLine("Press Enter to stop.");
            Console.ReadLine();

            Console.WriteLine("Service is stopping...");
            serviceHost.Close();

            Console.WriteLine("Service stopped.");
            Console.WriteLine();
        }
    }
}
