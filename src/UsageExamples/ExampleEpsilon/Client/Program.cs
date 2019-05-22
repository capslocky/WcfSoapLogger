﻿using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
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

            const string serviceUrl = "http://localhost:5585/weatherServiceEpsilon";
            var address = new EndpointAddress(serviceUrl);

            const string logPath = @"C:\SoapLog\Epsilon\Client";
            const bool saveOriginalBinaryBody = false;
            const bool useCustomHandler = false;
            MessageVersion messageVersion = MessageVersion.Soap11;

            var customBinding = new CustomBinding();

            customBinding.Elements.Add(new LoggingBindingElement(logPath, saveOriginalBinaryBody, useCustomHandler, messageVersion));
            customBinding.Elements.Add(new HttpTransportBindingElement());

            var serviceClient = new WeatherServiceClient(customBinding, address);

            var randomDataClient = new RandomDataClient(() => serviceClient);
            randomDataClient.StartThreads();

            Task.Delay(500).ContinueWith(_ => Process.Start("explorer.exe", logPath));

            Console.ReadLine();
        }
    }
}
