using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfSoapLogger.EncodingExtension;

namespace WcfSoapLogger
{
    public interface ISoapLoggerService
    {
        void LogResponseBody(byte[] responseBody);
    }

    public static class SoapLoggerThreadStatic
    {
//        [ThreadStatic]
//        internal static LoggingEncoder Encoder;

        [ThreadStatic]
        internal static ISoapLoggerService Service;

        [ThreadStatic]
        internal static byte[] RequestBody;

        internal static void SetEncoder(LoggingEncoder encoder) {
//            Encoder = encoder;
        }


        public static void SetService(ISoapLoggerService service, out byte[] requestBody) {
            Service = service;
            requestBody = RequestBody;
            RequestBody = null;
        }


  
    }
}
