using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WcfSoapLogger;
using WcfSoapLogger.CustomHandlers;

namespace Service
{
    public class Database : IDatabase, ISoapLoggerHandlerService
    {
        private const string LogDirectory = @"C:\SoapLogCustomService";

        public JuiceInfo[] FindSimilar(JuiceInfo juice)
        {
            Console.WriteLine(juice.Id + ": received.");
            
            //should be on the very top of method
            SoapLoggerService.CallCustomHandlers(this);

//            throw new InvalidOperationException("Problem in Service - FindSimilar");

            var context = OperationContext.Current;

            var result  = new JuiceInfo[]
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

            Console.WriteLine(juice.Id + ": returning. Count: " + result.Length);
            return result;
        }



        public void HandleRequestBody(byte[] requestBody, SoapLoggerSettings settings) 
        {
            //            throw new InvalidOperationException("Problem in Service - HandleRequestBody.");
            SoapLoggerTools.WriteFileDefault(requestBody, true, LogDirectory);
        }

        public void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings) 
        {
            //            throw new InvalidOperationException("Problem in Service - HandleResponseBodyCallback.");
            SoapLoggerTools.WriteFileDefault(responseBody, false, LogDirectory);
        }

        public void CustomHandlersDisabled(SoapLoggerSettings settings) 
        {
            Console.WriteLine("CustomHandlersDisabled");
        }
        

    }
}
