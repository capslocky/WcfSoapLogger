using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    static class Program
    {
        private static void Main() 
        {
            Console.Title = "ExampleAlpha.Service";

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
//            Process.Start("http://localhost:5580/ExampleAlpha");

            Console.WriteLine("Press Enter to stop.");
            Console.ReadLine();

            Console.WriteLine("Service is stopping...");
            serviceHost.Close();

            Console.WriteLine("Service stopped.");
            Console.WriteLine();
        }
    }
}
