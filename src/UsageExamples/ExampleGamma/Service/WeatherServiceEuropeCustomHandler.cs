using System;
using System.IO;
using CommonService;
using WcfSoapLogger;
using WcfSoapLogger.Exceptions;
using WcfSoapLogger.FileWriting;
using WcfSoapLogger.HandlerCustom;

namespace Service
{
    // this class just inherits standard web-service (from solution ExampleCommon) and adds own custom handler
    public class WeatherServiceEuropeCustomHandler : WeatherServiceEurope, ISoapLoggerHandlerService
    {
        public WeatherServiceEuropeCustomHandler()
        {
            // you need just this line in web-service constructor to apply your custom handlers to all operation methods
            // default WCF behavior is assumed here - creating new class instance 'per call'
            SoapLoggerService.CallCustomHandlers(this);
        }



        public void HandleRequestBody(byte[] requestBody, SoapLoggerSettings settings)
        {
            WriteFileCustom(requestBody, true, settings.LogPath);
        }

        public void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            WriteFileCustom(responseBody, false, settings.LogPath);
        }

        public void CustomHandlersDisabled(SoapLoggerSettings settings)
        {
            Console.WriteLine("CustomHandlersDisabled");
        }


      

        // this custom handling method looks for 'GetForecastByLocation' only
        private void WriteFileCustom(byte[] body, bool request, string logPath)
        {
            const string operationNameToLog = "GetForecastByLocation";
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

                fileNameFactory.AddSegment(message.GetNodeValue("Body / GetForecastByLocation / Location"));

                fileNameFactory.AddSegment(message.GetNodeValue("Body / GetForecastByLocationResponse / GetForecastByLocationResult / WeatherReport / Location"));
                fileNameFactory.AddSegment(message.GetNodeValue("Body / GetForecastByLocationResponse / GetForecastByLocationResult / WeatherReport / Temperature"));

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

//        readonly DbLoggingTest _dbLoggingTest = new DbLoggingTest("Service", "Body / GetForecastByLocation / Location");
    }
}
