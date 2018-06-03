using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using CommonClient;
using WcfSoapLogger.EncodingExtension;

namespace Client
{
    static class Program
    {
        private static void Main() 
        {
            Console.Title = "ExampleEpsilon.Client";
            Console.WriteLine("Press any key to start client.");
            Console.ReadKey();
            Console.WriteLine();

            const string serviceUrl = "http://localhost:5580/weatherService";
            var address = new EndpointAddress(serviceUrl);

            const string logPath = @"C:\SoapLog\Epsilon\Client";
            const string useCustomHandler = "False";

            var customBinding = new CustomBinding();

            customBinding.Elements.Add(new LoggingBindingElement(logPath, useCustomHandler));
            customBinding.Elements.Add(new HttpTransportBindingElement());

            var serviceClient = new WeatherServiceClient(customBinding, address);

            var randomDataClient = new RandomDataClient(serviceClient);
            randomDataClient.StartThreads();

            Console.ReadLine();
        }
    }
}
