using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.ServiceAlpha;

namespace Client
{
    static class Program
    {
        private static readonly Random random = new Random();

        private static void Main() 
        {
            Console.Title = "ExampleAlpha.Client";
            Console.WriteLine("Press any key to start client.");
            Console.ReadKey();
            Console.WriteLine();

            string[] locations = {"Berlin", "Praha", "Brussels"};
            int count = 10;

            foreach (string location in locations)
            {
                Task.Factory.StartNew(() => SendReportSeries(location, count));
                Task.Factory.StartNew(() => WatchLastReport(location));
            }

            Console.ReadLine();
        }


        private static void SendReportSeries(string location, int count)
        {
            for (int i = 0; i < count; i++)
            {
                SendRandomReport(location);
                Thread.Sleep(TimeSpan.FromMilliseconds(random.Next(0, 3000)));
            }
        }

        private static void SendRandomReport(string location)
        {
            WeatherReport newReport = new WeatherReport();
            newReport.DateTime = DateTime.Now;
            newReport.Location = location;
            newReport.Temperature = GetRandomValue(10, 15);
            newReport.Humidity = GetRandomValue(90, 97);
            newReport.Pressure = GetRandomValue(990, 1020);
            newReport.WindSpeed = GetRandomValue(0.1, 8.0);
            newReport.WindDirection = GetRandomValue(0, 360);

            Console.WriteLine("Report for " + location + ": sending.");

            var client = new WeatherServiceClient();
            long id = client.SendReport(newReport);

            Console.WriteLine("Report for " + location + ": Report ID = " + id);
        }

        private static double GetRandomValue(double minimum, double maximum) 
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }


        private static void WatchLastReport(string location)
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(5000));
                GetLastReport(location);
            }
        }

        private static void GetLastReport(string location)
        {
            var client = new WeatherServiceClient();
            var report = client.GetLastReportByLocation(location);

            if (report == null)
            {
                Console.WriteLine("GetLastReport for " + location + ": Report ID = null");
            }
            else
            {
                Console.WriteLine("GetLastReport for " + location + ": Report ID = " + report.Id);
            }
        }

    }
}
