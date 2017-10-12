using System;

namespace WcfSoapLogger
{
    public static class SoapLoggerForClient
    {
        [ThreadStatic]
        private static Action<byte[], SoapLoggerSettings> RequestBodyCallback;

        [ThreadStatic]
        private static Action<byte[], SoapLoggerSettings> ResponseBodyCallback;


        public static void SetRequestAndResponseCallbacks(Action<byte[], SoapLoggerSettings> requestBodyCallback, Action<byte[], SoapLoggerSettings> responseBodyCallback)
        {
            if (requestBodyCallback == null)
            {
                throw new ArgumentNullException("requestBodyCallback");
            }

            if (responseBodyCallback == null)
            {
                throw new ArgumentNullException("responseBodyCallback");
            }

            RequestBodyCallback = requestBodyCallback;
            ResponseBodyCallback = responseBodyCallback;
        }

        internal static void CallRequestCallback(byte[] requestBody, SoapLoggerSettings settings) 
        {
            if (RequestBodyCallback != null)
            {
                RequestBodyCallback(requestBody, settings);
                RequestBodyCallback = null;
            }
        }


        internal static void CallResponseCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            if (ResponseBodyCallback != null)
            {
                ResponseBodyCallback(responseBody, settings);
                ResponseBodyCallback = null;
            }
        }


    }
}
