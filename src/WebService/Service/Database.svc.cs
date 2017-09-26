using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using WcfSoapLogger;

namespace Service
{
    public class Database : IDatabase, ISoapLoggerService
    {
        private const string LogDirectory = @"C:\SoapLogService";

        public JuiceInfo[] FindSimilar(JuiceInfo juice)
        {
            byte[] requestBody;
            SoapLoggerThreadStatic.SetService(this, out requestBody);
            LogRequestBody(requestBody);

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

        private void LogRequestBody(byte[] requestBody)
        {
            Directory.CreateDirectory(LogDirectory);
//            string filePath = Path.Combine(LogDirectory, "Request_" + Guid.NewGuid() + ".xml");
//            File.WriteAllText(filePath, requestBody);

            SoapLoggerTools.LogBytes(requestBody, false, LogDirectory);
        }

        public void LogResponseBody(byte[] responseBody)
        {
            Directory.CreateDirectory(LogDirectory);
            //            string filePath = Path.Combine(LogDirectory, "Response_" + Guid.NewGuid() + ".xml");
            //            File.WriteAllText(filePath, response);

            SoapLoggerTools.LogBytes(responseBody, true, LogDirectory);
        }


    }
}
