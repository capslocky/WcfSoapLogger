using System;
using WcfSoapLogger.Exceptions;

namespace WcfSoapLogger.CustomHandlers
{
    public static class SoapLoggerService
    {
        // 'ThreadStaticAttribute' makes static field unique for every separate thread
        // That's how we can relate intercepted request/response bodies with appropriate execution of external method

        [ThreadStatic]
        private static SoapLoggerSettings Settings;

        [ThreadStatic]
        private static byte[] RequestBody;

        [ThreadStatic]
        private static ISoapLoggerHandlerService Service;

        [ThreadStatic]
        private static Exception RequestException;


        internal static void SetSettings(SoapLoggerSettings settings) 
        {
            Settings = settings;
        }

        internal static void SetRequestBody(byte[] requestBody) 
        {
            RequestBody = requestBody;
        }

        public static void CallCustomHandlers(ISoapLoggerHandlerService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (Settings == null)
            {
                throw new InvalidOperationException("Settings is null");
            }

            if (RequestBody == null)
            {
                service.CustomHandlersDisabled(Settings);
                return;
            }

            Service = service;

            try
            {
                Service.HandleRequestBody(RequestBody, Settings);
            }
            finally
            {
                RequestBody = null;
                Settings = null;
            }
        }

        internal static void CallResponseCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            if (RequestBody != null)
            {
                //something went wrong, either pipeline execution didn't reach web-service method 
                //or web-service method didn't call 'ReadRequestSetResponseCallback'

                //TODO determine case and log both files for first case

                var requestBody = RequestBody;
                RequestBody = null;

                SoapLoggerTools.WriteFileDefault(requestBody, true, settings.LogPath);
                SoapLoggerTools.WriteFileDefault(responseBody, false, settings.LogPath);

                //TODO add logging error message to file

                throw new LoggerException("something went wrong, either pipeline execution didn't reach web-service method or web-service method didn't call 'ReadRequestSetResponseCallback'");
            }

            try
            {
                Service.HandleResponseBodyCallback(responseBody, settings);
            }
            finally
            {
                Service = null;
            }
        }

        internal static void SetRequestException(Exception ex)
        {
            RequestException = ex;
        }


        internal static Exception GetRequestException() 
        {
            var ex = RequestException;
            RequestException = null;
            return ex;
        }
    }
}
