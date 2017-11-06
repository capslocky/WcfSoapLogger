using System;
using WcfSoapLogger.Exceptions;

namespace WcfSoapLogger.CustomHandlers
{
    public static class SoapLoggerClient
    {
        [ThreadStatic]
        private static ISoapLoggerHandlerClient Client;

        public static void SetCustomHandlerCallbacks(ISoapLoggerHandlerClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            Client = client;
        }

        internal static void CallRequestCallback(byte[] requestBody, SoapLoggerSettings settings) 
        {
            if (Client == null)
            {
                string methodName = typeof(SoapLoggerClient).Name + "." + nameof(SetCustomHandlerCallbacks);
                throw new LoggerException("You have enabled 'useCustomHandler' for client class of given service. So method '" + methodName+ "' should be called before making any request. Make sure to call it everywhere.");
            }

            Client.HandleRequestBodyCallback(requestBody, settings);
        }

        internal static void CallResponseCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            try
            {
                Client.HandleResponseBodyCallback(responseBody, settings);
            }
            finally
            {
                Client = null;
            }
        }

        internal static void CallCustomHandlersDisabledCallback(SoapLoggerSettings settings)
        {
            if (Client != null)
            {
                Client.CustomHandlersDisabledCallback(settings);
            }
        }


    }
}
