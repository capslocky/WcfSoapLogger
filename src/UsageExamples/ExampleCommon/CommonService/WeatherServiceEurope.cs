using System;
using System.ServiceModel;
using System.Threading;

namespace CommonService
{
    [ServiceBehavior(Namespace = XmlNamespaces.WeatherService)]
    public class WeatherServiceEurope : IWeatherService
    {
        private static readonly ReportRepository _reportRepository = new ReportRepository();
        private static readonly ForecastCalculator _forecastCalculator  = new ForecastCalculator(_reportRepository);
        private static readonly Random _random = new Random();

        public virtual long SendReport(WeatherReport report)
        {
            Console.WriteLine("SendReport for " + report.Location + ": request");

            long id = _reportRepository.Add(report);
            Thread.Sleep(TimeSpan.FromMilliseconds(_random.Next(100, 1000)));

            Console.WriteLine("SendReport for " + report.Location + ": returning Report ID = " + id);
            return id;
        }

        public virtual WeatherReport GetLastReportByLocation(string location)
        {
            Console.WriteLine("GetLastReport for " + location + ": request");

            var lastReport = _reportRepository.GetLastByLocation(location);

            Console.WriteLine("GetLastReport for " + location + ": returning Report ID = " + lastReport.Id);
            return lastReport;
        }

        public virtual WeatherReport[] GetForecastByLocation(string location, int days)
        {
            Console.WriteLine("Forecast for " + location + " for " + days + " days: request");

            Thread.Sleep(TimeSpan.FromMilliseconds(_random.Next(100, 2000)));
            var forecast =  _forecastCalculator.GetForecast(location, days);

            Console.WriteLine("Forecast for " + location + " for " + days + " days: returning");
            return forecast;
        }
    }
}
