using System;
using WcfSoapLogger;
using WcfSoapLogger.FileWriting;
using WcfSoapLogger.HandlerCustom;

namespace CommonService.CustomHandling
{
    public class WeatherServiceEuropeCustomHandler : WeatherServiceEurope, ISoapLoggerHandlerService
    {
        public override long SendReport(WeatherReport report)
        {
            SoapLoggerService.CallCustomHandlers(this);
            return base.SendReport(report);
        }

        public override WeatherReport GetLastReportByLocation(string location)
        {
            SoapLoggerService.CallCustomHandlers(this);
            return base.GetLastReportByLocation(location);
        }

        public override WeatherReport[] GetForecastByLocation(string location, int days) 
        {
            SoapLoggerService.CallCustomHandlers(new CustomHandler_GetForecastByLocation());
            return base.GetForecastByLocation(location, days);
        }





        public void HandleRequestBody(byte[] requestBody, SoapLoggerSettings settings)
        {
            SoapLoggerTools.WriteFileDefault(requestBody, true, settings.LogPath);
        }

        public void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            SoapLoggerTools.WriteFileDefault(responseBody, false, settings.LogPath);
        }

        public void CustomHandlersDisabled(SoapLoggerSettings settings)
        {
            Console.WriteLine("CustomHandlersDisabled");
        }
    }
}
