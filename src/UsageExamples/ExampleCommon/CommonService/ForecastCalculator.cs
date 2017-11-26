using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonService
{
    public class ForecastCalculator
    {
        private readonly ReportRepository _reportRepository;
        private readonly Random _random;

        public ForecastCalculator(ReportRepository reportRepository) 
        {
            _reportRepository = reportRepository;
            _random = new Random();
        }

        public WeatherReport[] GetForecast(string location, int days) 
        {
            var lastReport = _reportRepository.GetLastByLocation(location);
            var list = new List<WeatherReport>();

            var memoryStream = new MemoryStream();
            var serializer = new DataContractSerializer(typeof(WeatherReport));
            serializer.WriteObject(memoryStream, lastReport);

            for (int i = 0; i < days; i++)
            {
                memoryStream.Position = 0;
                var clone = (WeatherReport)serializer.ReadObject(memoryStream);

                clone.DateTime = clone.DateTime.AddDays(i).Date;
                clone.Temperature += GetRandomValue(0.1, 3);
                clone.Pressure += GetRandomValue(-10, +10);

                list.Add(clone);
            }

            return list.ToArray();
        }

        private double GetRandomValue(double minimum, double maximum) 
        {
            return _random.NextDouble() * (maximum - minimum) + minimum;
        }

    }
}
