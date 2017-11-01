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


        internal static void SetRequestBody(byte[] requestBody, SoapLoggerSettings settings) 
        {
            RequestBody = requestBody;
            Settings = settings;
        }


        public static void ReadRequestSetResponseCallback(out byte[] requestBody, out SoapLoggerSettings settings, Action<byte[], SoapLoggerSettings> responseBodyCallback)
        {
            if (Settings == null)
            {
                requestBody = null;
                settings = null;
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

            requestBody = RequestBody;
            settings = Settings;

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

                throw new LoggerException("something went wrong, either pipeline execution didn't reach web-service method ");
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
    }
}
