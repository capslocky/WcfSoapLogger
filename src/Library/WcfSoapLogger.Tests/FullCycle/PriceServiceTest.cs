using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfSoapLogger.EncodingExtension;
using WcfSoapLogger.Tests.FullCycle.Client;
using WcfSoapLogger.Tests.FullCycle.Service;
using IPriceService = WcfSoapLogger.Tests.FullCycle.Service.IPriceService;

namespace WcfSoapLogger.Tests.FullCycle
{
    [TestClass]
    public class PriceServiceTest
    {
        const string serviceUrl = @"http://localhost:5580/weatherService";
        const string logPath = @"C:\SoapLog\Tests\Service";

        [TestMethod]
        public void ServiceDefaultLoggingOneCall()
        {
            if (Directory.Exists(logPath))
            {
                DeleteDirectory(logPath);
            }

            using (var serviceHost = GetServiceWithDefaultLogging())
            {
                try
                {
                    serviceHost.Open();
                }
                catch (AddressAccessDeniedException)
                {
                    Trace.WriteLine("ERROR. Please run this application with needed rights.");
                    throw;
                }

                var client = GetClientStandard();
                var priceInfo = client.GetPriceInfo(45);

                serviceHost.Close();
            }

            const int fileCount = 2;
            string[] files = Directory.GetFiles(logPath, "*", SearchOption.AllDirectories);

            Assert.AreEqual(fileCount, files.Length);
        }


        //https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true
        public static void DeleteDirectory(string path)
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
        }

        private PriceServiceClient GetClientStandard()
        {
            var address = new EndpointAddress(serviceUrl);
            var binding = new BasicHttpBinding();
            var client = new PriceServiceClient(binding, address);
            return client;
        }


        private ServiceHost GetServiceWithDefaultLogging()
        {
            var serviceHost = new ServiceHost(typeof(PriceServiceKiaMotors), new Uri(serviceUrl));

            var metadataBehavior = new ServiceMetadataBehavior();
            metadataBehavior.HttpGetEnabled = true;
            serviceHost.Description.Behaviors.Add(metadataBehavior);

            serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
        
            string useCustomHandler = Boolean.FalseString;

            CustomBinding customBinding = new CustomBinding();
            customBinding.Elements.Add(new LoggingBindingElement(logPath, useCustomHandler));
            customBinding.Elements.Add(new HttpTransportBindingElement());

            serviceHost.AddServiceEndpoint(typeof(IPriceService), customBinding, "");
            return serviceHost;
        }

    }
}
