using System;

namespace WcfSoapLogger
{
    public static class SoapLoggerForService
    {
        // 'ThreadStaticAttribute' makes static field unique for every separate thread
        // That's how we can relate intercepted request/response bodies with appropriate execution of external method

        [ThreadStatic]
        private static byte[] RequestBody;

        [ThreadStatic]
        private static SoapLoggerSettings Settings;

        [ThreadStatic]
        private static Action<byte[], SoapLoggerSettings> ResponseBodyCallback;

        [ThreadStatic]
        private static Exception RequestException;


        internal static void SetRequestBody(byte[] requestBody, SoapLoggerSettings settings) 
        {
            RequestBody = requestBody;
            Settings = settings;
        }

        public static void ReadRequestSetResponseCallback(out byte[] outRequestBody, out SoapLoggerSettings outSettings, Action<byte[], SoapLoggerSettings> responseBodyCallback)
        {
            if (Settings == null)
            {
                outRequestBody = null;
                outSettings = null;
                return;
            }

            if (RequestBody == null)
            {
                throw new InvalidOperationException("RequestBody is null");
            }

            if (responseBodyCallback == null)
            {
                throw new ArgumentNullException(nameof(responseBodyCallback));
            }

            outRequestBody = RequestBody;
            outSettings = Settings;

            RequestBody = null;
            Settings = null;

            ResponseBodyCallback = responseBodyCallback;
        }


        internal static void CallResponseCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            if (RequestBody != null)
            {
                //something went wrong, either pipeline execution didn't reach web-service method 
                //or web-service method didn't call 'ReadRequestSetResponseCallback'

                //TODO determine case and log both files for first case

                throw new LoggerException("something went wrong, either pipeline execution didn't reach web-service method or web-service method didn't call 'ReadRequestSetResponseCallback'");
            }

            if (ResponseBodyCallback != null)
            {
                try
                {
                    ResponseBodyCallback(responseBody, settings);
                }
                finally
                {
                    ResponseBodyCallback = null;
                }
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
