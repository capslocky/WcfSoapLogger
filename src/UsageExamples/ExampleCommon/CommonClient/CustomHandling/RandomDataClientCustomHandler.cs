using System;
using System.IO;
using WcfSoapLogger;
using WcfSoapLogger.CustomHandlers;
using WcfSoapLogger.Exceptions;
using WcfSoapLogger.FileWriting;

namespace CommonClient.CustomHandling
{
    public class RandomDataClientCustomHandler : RandomDataClient, ISoapLoggerHandlerClient
    {
        public RandomDataClientCustomHandler(WeatherServiceClient client) : base(client)
        {
        }

        protected override void SendRandomReport(string location)
        {
            SoapLoggerClient.SetCustomHandlerCallbacks(this);
            base.SendRandomReport(location);
        }

        protected override void GetLastReport(string location) {
            SoapLoggerClient.SetCustomHandlerCallbacks(this);
            base.GetLastReport(location);
        }

        protected override void GetForecast(string location, int days) {
            SoapLoggerClient.SetCustomHandlerCallbacks(this);
            base.GetForecast(location, days);
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

                fileNameFactory.AddSegment(operationName);

                fileNameFactory.AddSegment(message.GetNodeValue("Body", "GetLastReportByLocation", "Location"));

//                fileNameFactory.AddSegment(message.GetNodeValue("Body", "SendReport", "report", "location"));
//                fileNameFactory.AddSegment(message.GetNodeValue("Body", "GetForecastByLocation", "location"));

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

    }
}
