using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class WeatherServiceEurope : IWeatherService
    {
        private static readonly ReportRepository _reportRepository  = new ReportRepository();
        private static readonly Random random = new Random();

        public long SendReport(WeatherReport report)
        {
            long id = _reportRepository.Add(report);
            Thread.Sleep(TimeSpan.FromMilliseconds(random.Next(0, 2000)));
            return id;
        }

        public WeatherReport GetLastReportByLocation(string location)
        {
            return _reportRepository.GetLastByLocation(location);
        }
    }


    public class ReportRepository
    {
        private readonly object _lock  = new object();
        private readonly List<WeatherReport> reports = new List<WeatherReport>();

        public long Add(WeatherReport report)
        {
            lock (_lock)
            {
                report.Id = reports.Count + 1;
                reports.Add(report);
                return report.Id;
            }
        }

        public WeatherReport GetLastByLocation(string location)
        {
            var lastReport = reports.FindLast(report => report.Location == location);
            return lastReport;
        }
    }
}
