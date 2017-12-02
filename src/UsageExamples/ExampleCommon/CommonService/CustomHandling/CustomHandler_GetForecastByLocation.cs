using System;
using System.IO;
using WcfSoapLogger;
using WcfSoapLogger.CustomHandlers;
using WcfSoapLogger.Exceptions;
using WcfSoapLogger.FileWriting;

namespace CommonService.CustomHandling
{
    public class CustomHandler_GetForecastByLocation : ServiceCustomHandler
    {
        public override void HandleRequestBody(byte[] requestBody, SoapLoggerSettings settings)
        {
            WriteFileCustom(requestBody, true, settings.LogPath);
        }

        public override void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            WriteFileCustom(responseBody, false, settings.LogPath);
        }

        public override void CustomHandlersDisabled(SoapLoggerSettings settings)
        {
        }


        private void WriteFileCustom(byte[] body, bool request, string logPath)
        {
            logPath = Path.Combine(logPath, "GetForecastByLocation");

            var fileNameFactory = new FileNameFactory();

            try
            {
                var message = SoapMessage.Parse(body, request);
                fileNameFactory.AddSegment(message.GetOperationName());

                fileNameFactory.AddSegment(message.GetNodeValue("body", "GetForecastByLocation", "location"));

                fileNameFactory.AddSegment(message.GetNodeValue("body", "GetForecastByLocationResponse", "GetForecastByLocationResult", "WeatherReport", "Location"));
                fileNameFactory.AddSegment(message.GetNodeValue("body", "GetForecastByLocationResponse", "GetForecastByLocationResult", "WeatherReport", "Temperature"));

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
