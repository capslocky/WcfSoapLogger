using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        protected override void SetCustomHandler()
        {
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
