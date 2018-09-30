using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfSoapLogger.EncodingExtension;
using WcfSoapLogger.FileWriting;
using WcfSoapLogger.Tests.FullCycle.Client;
using WcfSoapLogger.Tests.FullCycle.Service;
using IPriceService = WcfSoapLogger.Tests.FullCycle.Service.IPriceService;

namespace WcfSoapLogger.Tests.FullCycle
{
    public abstract class FullCycleBase
    {
        private const string serviceUrl = @"http://localhost:5580/weatherService";
        private const string BaseLogFolder = @"C:\SoapLog";

        protected abstract ServiceHost GetServiceHost();
        protected abstract PriceServiceClient GetClient();

        protected readonly string logPathClient;
        protected readonly string logPathService;

        protected FullCycleBase()
        {
            string className = GetType().Name;

            logPathClient = Path.Combine(BaseLogFolder, className + "_client");
            logPathService = Path.Combine(BaseLogFolder, className + "_service");
        }


        protected void RunFullCycle() {
            if (Directory.Exists(logPathClient))
            {
                DeleteDirectory(logPathClient);
            }

            if (Directory.Exists(logPathService))
            {
                DeleteDirectory(logPathService);
            }

            using (var serviceHost = GetServiceHost())
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

                var client = GetClient();
                var priceInfo = client.GetPriceInfo(45);

                serviceHost.Close();
            }

//            if (!Directory.Exists(logPathService))
//            {
//                Assert.Fail("No directory created: " + logPathService);
//            }
//
//            if (!Directory.Exists(logPathClient))
//            {
//                Assert.Fail("No directory created: " + logPathClient);
//            }

            const int fileCount = 2;


            if (Directory.Exists(logPathService))
            {
                string[] files = Directory.GetFiles(logPathService, "*", SearchOption.AllDirectories);

                Assert.AreEqual(fileCount, files.Length);

                CheckDefaultFileName(files[0], "GetPriceInfo", true);
                CheckDefaultFileName(files[1], "GetPriceInfo", false);
            }

            if (Directory.Exists(logPathClient))
            {
                string[] files = Directory.GetFiles(logPathClient, "*", SearchOption.AllDirectories);

                Assert.AreEqual(fileCount, files.Length);

                CheckDefaultFileName(files[0], "GetPriceInfo", true);
                CheckDefaultFileName(files[1], "GetPriceInfo", false);
            }
        }




        protected ServiceHost GetServiceStandard() {
            var serviceHost = new ServiceHost(typeof(PriceServiceKiaMotors), new Uri(serviceUrl));

            var metadataBehavior = new ServiceMetadataBehavior();
            metadataBehavior.HttpGetEnabled = true;
            serviceHost.Description.Behaviors.Add(metadataBehavior);

            serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
            BasicHttpBinding basicHttpBinding = new BasicHttpBinding();

            serviceHost.AddServiceEndpoint(typeof(IPriceService), basicHttpBinding, "");
            return serviceHost;
        }


        protected ServiceHost GetServiceWithDefaultLogging() {
            var serviceHost = new ServiceHost(typeof(PriceServiceKiaMotors), new Uri(serviceUrl));

            var metadataBehavior = new ServiceMetadataBehavior();
            metadataBehavior.HttpGetEnabled = true;
            serviceHost.Description.Behaviors.Add(metadataBehavior);

            serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            string saveOriginalBinaryBody = Boolean.FalseString;
            string useCustomHandler = Boolean.FalseString;

            CustomBinding customBinding = new CustomBinding();
            customBinding.Elements.Add(new LoggingBindingElement(logPathService, saveOriginalBinaryBody, useCustomHandler));
            customBinding.Elements.Add(new HttpTransportBindingElement());

            serviceHost.AddServiceEndpoint(typeof(IPriceService), customBinding, "");
            return serviceHost;
        }




        protected PriceServiceClient GetClientStandard() {
            var address = new EndpointAddress(serviceUrl);
            var binding = new BasicHttpBinding();
            var client = new PriceServiceClient(binding, address);
            return client;
        }


        protected PriceServiceClient GetClientWithDefaultLogging() {
            var address = new EndpointAddress(serviceUrl);
            string SaveOriginalBinaryBody = Boolean.FalseString;
            string useCustomHandler = Boolean.FalseString;

            CustomBinding customBinding = new CustomBinding();
            customBinding.Elements.Add(new LoggingBindingElement(logPathClient, SaveOriginalBinaryBody, useCustomHandler));
            customBinding.Elements.Add(new HttpTransportBindingElement());

            var client = new PriceServiceClient(customBinding, address);
            return client;
        }



        //https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true
        protected void DeleteDirectory(string path) {
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


        //2018-01-07_at_19-21-55-245__GetPriceInfo_Request.xml
        protected void CheckDefaultFileName(string filepath, string operationName, bool request) {
            string fileName = Path.GetFileName(filepath);

            const string dateFormat = FileNameFactory.DateTimeFileNameFormat;
            string partDateTime = fileName.Substring(0, dateFormat.Replace("'", null).Length);
            DateTime fileDateTime = DateTime.ParseExact(partDateTime, dateFormat, CultureInfo.InvariantCulture);

            int passedSeconds = (int)DateTime.Now.Subtract(fileDateTime).TotalSeconds;

            Assert.IsTrue(passedSeconds >= 0);
            Assert.IsTrue(passedSeconds < 20);

            Assert.AreEqual(".xml", Path.GetExtension(fileName));

            string partMainActual = Path.GetFileNameWithoutExtension(fileName).Substring(dateFormat.Length);
            string partMainExpected = operationName + "_" + (request ? "Request" : "Response");

            Assert.AreEqual(partMainExpected, partMainActual);
        }





    }
}
