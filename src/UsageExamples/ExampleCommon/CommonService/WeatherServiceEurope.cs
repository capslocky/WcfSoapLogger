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
            long id = _reportRepository.Add(report);
            Thread.Sleep(TimeSpan.FromMilliseconds(_random.Next(100, 1000)));
            return id;
        }

        public virtual WeatherReport GetLastReportByLocation(string location)
        {
            return _reportRepository.GetLastByLocation(location);
        }

        public virtual WeatherReport[] GetForecastByLocation(string location, int days)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(_random.Next(100, 2000)));
            return _forecastCalculator.GetForecast(location, days);
        }
    }
}
