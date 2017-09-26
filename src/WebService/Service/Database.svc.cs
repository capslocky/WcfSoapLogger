using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfSoapLogger;

namespace Service
{
    public class Database : IDatabase, ISoapLoggerService
    {
        private const string LogDirectory = @"C:\SoapLogService";

        public JuiceInfo[] FindSimilar(JuiceInfo juice)
        {
            string request;
            SoapLoggerThreadStatic.SetService(this, out request);

            ProcessRequestLog(request);

            return new JuiceInfo[]
            {
                new JuiceInfo()
                {
                    Id = 5,
                    Name = "Happy apple",
                    Location = "Warehouse Zeta",
                    Price = 430
                },
                new JuiceInfo()
                {
                    Id = 8,
                    Name = "Wonderful carrot",
                    Location = "Warehouse Lambda",
                    Price = 575
                },
            };
            
        }

        private void ProcessRequestLog(string request)
        {
            Directory.CreateDirectory(LogDirectory);
            string filePath = Path.Combine(LogDirectory, "Request_" + Guid.NewGuid() + ".xml");
            File.WriteAllText(filePath, request);
        }

        public void ProcessResponseLog(string response)
        {
            Directory.CreateDirectory(LogDirectory);
            string filePath = Path.Combine(LogDirectory, "Response_" + Guid.NewGuid() + ".xml");
            File.WriteAllText(filePath, response);
        }


    }
}
