using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

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

            if (lastReport == null)
            {
                return new WeatherReport[0];
            }

            var list = new List<WeatherReport>();

            var memoryStream = new MemoryStream();
            var serializer = new DataContractSerializer(typeof(WeatherReport));
            serializer.WriteObject(memoryStream, lastReport);

            for (int i = 0; i < days; i++)
            {
                memoryStream.Position = 0;
                var clone = (WeatherReport)serializer.ReadObject(memoryStream);

                clone.Id = -1;
                clone.DateTime = clone.DateTime.AddDays(i + 1).Date;
                clone.Temperature += GetRandomValue(0.1f, 3);
                clone.Pressure += GetRandomValue(-10, +10);

                list.Add(clone);
            }

            return list.ToArray();
        }

        private float GetRandomValue(float minimum, float maximum) 
        {
            return (float)_random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
