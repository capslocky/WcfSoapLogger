using System;
using System.ServiceModel;
using System.Threading;
using WcfSoapLogger;
using WcfSoapLogger.CustomHandlers;
using WcfSoapLogger.FileWriting;

namespace CommonService.CustomHandling
{
    [ServiceBehavior(Namespace = XmlNamespaces.WeatherService)]
    public class WeatherServiceEuropeCustomHandler : IWeatherService, ISoapLoggerHandlerService
    {
        private static readonly ReportRepository _reportRepository = new ReportRepository();
        private static readonly ForecastCalculator _forecastCalculator = new ForecastCalculator(_reportRepository);
        private static readonly Random _random = new Random();

        public long SendReport(WeatherReport report)
        {
            SoapLoggerService.CallCustomHandlers(this);

            long id = _reportRepository.Add(report);
            Thread.Sleep(TimeSpan.FromMilliseconds(_random.Next(100, 1000)));
            return id;
        }

        public WeatherReport GetLastReportByLocation(string location)
        {
            SoapLoggerService.CallCustomHandlers(this);

            return _reportRepository.GetLastByLocation(location);
        }

        public WeatherReport[] GetForecastByLocation(string location, int days) 
        {
            SoapLoggerService.CallCustomHandlers(new CustomHandler_GetForecastByLocation());

            Thread.Sleep(TimeSpan.FromMilliseconds(_random.Next(100, 2000)));
            return _forecastCalculator.GetForecast(location, days);
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
