using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfSoapLogger;

namespace Service
{
    public class Database : IDatabase
    {
        private const string LogDirectory = @"C:\SoapLogCustomService";

        public JuiceInfo[] FindSimilar(JuiceInfo juice)
        {

            byte[] requestBody;
            SoapLoggerSettings settings;

            SoapLoggerForService.ReadRequestSetResponseCallback(out requestBody, out settings, ResponseCallback);
            SoapLoggerTools.LogBytes(requestBody, false, LogDirectory);


            return new JuiceInfo[]
            {
                new JuiceInfo()
                {
                    Id = juice.Id,
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

        private void ResponseCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            SoapLoggerTools.LogBytes(responseBody, true, LogDirectory);
        }


    }
}
