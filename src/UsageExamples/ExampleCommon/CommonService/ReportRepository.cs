using System;
using System.Collections.Generic;

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

                if (report.Id % 5 == 0)
                {
                    throw new InvalidOperationException($"Oops! Report with ID '{report.Id}' produced an error on server. Look for its Fault_Response.xml in log folder.");
                }

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
