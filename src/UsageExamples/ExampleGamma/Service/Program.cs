﻿using System;
using System.Diagnostics;
using System.ServiceModel;

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

            Process.Start("http://localhost:5583/weatherServiceGamma");

            Console.WriteLine("Press Enter to stop.");
            Console.ReadLine();

            Console.WriteLine("Service is stopping...");
            serviceHost.Close();

            Console.WriteLine("Service stopped.");
            Console.WriteLine();
        }
    }
}
