using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using CommonService;
using WcfSoapLogger.EncodingExtension;

namespace Service
{
    static class Program
    {
        private static void Main() 
        {
            Console.Title = "ExampleEpsilon.Service";
     
            const string baseAddress  = "http://localhost:5585/weatherServiceEpsilon";
            var serviceHost = new ServiceHost(typeof(WeatherServiceEurope), new Uri(baseAddress));

            serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;

            var metadataBehavior = new ServiceMetadataBehavior();
            metadataBehavior.HttpGetEnabled = true;
            serviceHost.Description.Behaviors.Add(metadataBehavior);

            serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            const string logPath = @"C:\SoapLog\Epsilon\Service";
            const bool saveOriginalBinaryBody = false;
            const bool useCustomHandler = false;
            MessageVersion messageVersion = MessageVersion.Soap11;

            var customBinding = new CustomBinding();
            customBinding.Elements.Add(new LoggingBindingElement(logPath, saveOriginalBinaryBody, useCustomHandler, messageVersion));
            customBinding.Elements.Add(new HttpTransportBindingElement());

            serviceHost.AddServiceEndpoint(typeof(IWeatherService), customBinding, "");


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

            Process.Start("http://localhost:5585/weatherServiceEpsilon");

            Console.WriteLine("Press Enter to stop.");
            Console.ReadLine();

            Console.WriteLine("Service is stopping...");
            serviceHost.Close();

            Console.WriteLine("Service stopped.");
            Console.WriteLine();
        }
    }
}
