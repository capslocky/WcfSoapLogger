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
                throw new ArgumentNullException(nameof(requestBodyCallback));
            }

            if (responseBodyCallback == null)
            {
                throw new ArgumentNullException(nameof(responseBodyCallback));
            }

            RequestBodyCallback = requestBodyCallback;
            ResponseBodyCallback = responseBodyCallback;
        }

        internal static void CallRequestCallback(byte[] requestBody, SoapLoggerSettings settings) 
        {
            if (RequestBodyCallback == null)
            {
                string methodName = typeof(SoapLoggerForClient).Name + "." + nameof(SetRequestAndResponseCallbacks);
                throw new LoggerException("You have enabled 'useCustomHandler' for client class of given service. So method '" + methodName+ "' should be called before making any request. Make sure to call it everywhere.");
            }

            try
            {
                RequestBodyCallback(requestBody, settings);
            }
            finally
            {
                RequestBodyCallback = null;
            }
        }


        internal static void CallResponseCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            if (ResponseBodyCallback == null)
            {
                throw new LoggerException("method 'SetRequestAndResponseCallbacks' has not been called by client");
            }

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
