using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonService
{
    public class ReportRepository
    {
        private readonly object _lock = new object();
        private readonly List<WeatherReport> _reports = new List<WeatherReport>();

        public long Add(WeatherReport report)
        {
            lock (_lock)
            {
                report.Id = _reports.Count + 1;
                _reports.Add(report);
                return report.Id;
            }
        }

        public WeatherReport GetLastByLocation(string location)
        {
            lock (_lock)
            {
                var lastReport = _reports.FindLast(report => report.Location == location);
                return lastReport;
            }
        }
    }
}
