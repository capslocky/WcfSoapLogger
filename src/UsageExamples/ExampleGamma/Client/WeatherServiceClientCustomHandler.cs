using System;
using System.IO;
using CommonClient;
using Service;
using WcfSoapLogger;
using WcfSoapLogger.Exceptions;
using WcfSoapLogger.FileWriting;
using WcfSoapLogger.HandlerCustom;

namespace Client
{
    // this class just inherits standard auto-generated client class (from solution ExampleCommon) and adds own custom handler
    public class WeatherServiceClientCustomHandler : WeatherServiceClient, ISoapLoggerHandlerClient
    {
        public WeatherServiceClientCustomHandler()
        {
            // you need just this line in constructor of inherited client class to apply your custom handler to all requests
            // NOTE: don't reuse this custom handling client class object twice, it should be instantiated for every new request
            SoapLoggerClient.SetCustomHandlerCallbacks(this);
        }



        public void HandleRequestBodyCallback(byte[] requestBody, SoapLoggerSettings settings)
        {
            WriteFileCustom(requestBody, true, settings.LogPath);
        }

        public void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            WriteFileCustom(responseBody, false, settings.LogPath);
        }

        public void CustomHandlersDisabledCallback(SoapLoggerSettings settings)
        {
            Console.WriteLine("CustomHandlersDisabled");
        }
        


        // this custom handling method looks for 'GetLastReportByLocation' only
        private void WriteFileCustom(byte[] body, bool request, string logPath)
        {
            const string operationNameToLog = "GetLastReportByLocation";
            logPath = Path.Combine(logPath, operationNameToLog);

            var fileNameFactory = new FileNameFactory();

            try
            {
                var message = SoapMessage.Parse(body, request);
                string operationName = message.GetOperationName();

                if (operationName != operationNameToLog)
                {
                    return;
                }

//                _dbLoggingTest.LogToDatabase(message, request); // uncomment to see simple DB logging

                fileNameFactory.AddSegment(operationName);
                fileNameFactory.AddSegment(message.GetNodeValue($"Body / {operationNameToLog} / Location"));

                fileNameFactory.AddDirection(request);
                string indentedXml = message.GetIndentedXml();

                SoapLoggerTools.WriteFile(fileNameFactory.GetFileName(), indentedXml, null, logPath);
            }
            catch (FileSystemAcccesDeniedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                fileNameFactory.AddSegment("ERROR");
                SoapLoggerTools.WriteFile(fileNameFactory.GetFileName(), null, body, logPath);

                fileNameFactory.AddSegment("exception");
                SoapLoggerTools.WriteFile(fileNameFactory.GetFileName(), ex.ToString(), null, logPath);
            }
        }

//        readonly DbLoggingTest _dbLoggingTest = new DbLoggingTest("Client" , "Body / GetLastReportByLocation / Location");
    }
}
