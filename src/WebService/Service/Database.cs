using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Database : IDatabase//, ISoapLoggerService
    {
        private const string LogDirectory = @"C:\SoapLogCustom";

        public JuiceInfo[] FindSimilar(JuiceInfo juice) {
            //            byte[] requestBody;
            //            SoapLoggerThreadStatic.SetService(this, out requestBody);
            //
            //            if (requestBody != null)
            //            {
            //                LogRequestBody(requestBody);
            //            }

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


    }
}
