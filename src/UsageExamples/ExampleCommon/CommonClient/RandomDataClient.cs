using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommonClient
{
    public class RandomDataClient
    {
        private readonly Random _random;
        private readonly Func<IWeatherService> _getClient;

        public RandomDataClient(Func<IWeatherService> getClient)
        {
            _random = new Random();
            _getClient = getClient;
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

        protected void SendRandomReport(string location) {
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
                long id = _getClient().SendReport(newReport);
                Console.WriteLine("Report for " + location + ": Report ID = " + id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

      private float GetRandomValue(float minimum, float maximum) 
      {
        double value = minimum + _random.NextDouble() * (maximum - minimum);
        value = value * 100;
        value = Math.Round(value, MidpointRounding.AwayFromZero);
        value = value / 100;
        return (float) value;
      }


        private void WatchLastReport(string location) 
        {
            while (true)
            {
                ThreadSleep(2000, 5000);
                GetLastReport(location);
            }
        }

        protected void GetLastReport(string location)
        {
            try
            {
                var report = _getClient().GetLastReportByLocation(location);

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
                ThreadSleep(3000, 6000);
                GetForecast(location, days);
            }
        }

        protected void GetForecast(string location, int days) 
        {
            try
            {
                var forecastArray = _getClient().GetForecastByLocation(location, days);
                Console.WriteLine("Forecast for " + location + " for " + forecastArray.Length + " days received.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }
}
