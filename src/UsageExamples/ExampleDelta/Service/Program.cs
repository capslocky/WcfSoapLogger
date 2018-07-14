using System;
using System.Diagnostics;
using System.ServiceModel;
using CommonService;

namespace Service
{
    static class Program
    {
        private static void Main() 
        {
            Console.Title = "ExampleDelta.Service";

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

            Process.Start("https://wcf.mozilla.org:5584/weatherServiceDelta");

            Console.WriteLine("Press Enter to stop.");
            Console.ReadLine();

            Console.WriteLine("Service is stopping...");
            serviceHost.Close();

            Console.WriteLine("Service stopped.");
            Console.WriteLine();
        }
    }
}
