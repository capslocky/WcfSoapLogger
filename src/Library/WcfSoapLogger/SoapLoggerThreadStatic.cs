using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    public static class SoapLoggerThreadStatic
    {
//        [ThreadStatic]
//        internal static LoggingEncoder Encoder;

        [ThreadStatic]
        internal static ISoapLoggerService Service;

        [ThreadStatic]
        internal static string ContentRequest;

        internal static void SetEncoder(LoggingEncoder encoder) {
//            Encoder = encoder;
        }


        public static void SetService(ISoapLoggerService service, out string requestLog) {
            Service = service;
            requestLog = ContentRequest;
            ContentRequest = null;
        }


  
    }
}
