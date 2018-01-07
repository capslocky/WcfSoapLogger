using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonClient
{
    public class RandomDataClient
    {
        private readonly Random _random;
        private readonly IWeatherService _client;

        public RandomDataClient(IWeatherService client)
        {
            _random = new Random();
            _client = client;
        }

        public void StartThreads()
        {
            string[] locations = { "Berlin", "Praha", "Brussels" };
            int reportCount = 10;

            foreach (string location in locations)
            {
                Task.Factory.StartNew(() => SendReportSeries(location, reportCount));
                Task.Factory.StartNew(() => WatchLastReport(location));
                Task.Factory.StartNew(() => WatchForecast(location, 4));
            }
        }


        private void SendReportSeries(string location, int reportCount) 
        {
            for (int i = 0; i < reportCount; i++)
            {
                SendRandomReport(location);
                ThreadSleep(0, 3000);
            }
        }

        private void ThreadSleep(int minValue, int maxValue)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(_random.Next(minValue, maxValue)));
        }

        protected virtual void SendRandomReport(string location) {
            WeatherReport newReport = new WeatherReport();

            newReport.DateTime = DateTime.Now;
            newReport.Location = location;
            newReport.Temperature = GetRandomValue(10, 15);
            newReport.Humidity = GetRandomValue(90, 97);
            newReport.Pressure = GetRandomValue(990, 1020);
            newReport.WindSpeed = GetRandomValue(0.1f, 8.0f);
            newReport.WindDirection = GetRandomValue(0, 360);

            Console.WriteLine("Report for " + location + ": sending.");

            try
            {
                long id = _client.SendReport(newReport);
                Console.WriteLine("Report for " + location + ": Report ID = " + id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private float GetRandomValue(float minimum, float maximum) 
        {
            return (float)_random.NextDouble() * (maximum - minimum) + minimum;
        }


        private void WatchLastReport(string location) 
        {
            while (true)
            {
                ThreadSleep(2000, 5000);
                GetLastReport(location);
            }
        }

        protected virtual void GetLastReport(string location)
        {
            try
            {
                var report = _client.GetLastReportByLocation(location);

                if (report == null)
                {
                    Console.WriteLine("GetLastReport for " + location + ": Report ID = null");
                }
                else
                {
                    Console.WriteLine("GetLastReport for " + location + ": Report ID = " + report.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void WatchForecast(string location, int days) 
        {
            while (true)
            {
                ThreadSleep(4000, 7000);
                GetForecast(location, days);
            }
        }

        protected virtual void GetForecast(string location, int days) 
        {
            try
            {
                var forecastArray = _client.GetForecastByLocation(location, days);
                Console.WriteLine("Forecast for " + location + " for " + forecastArray.Length + " days received.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }
}
