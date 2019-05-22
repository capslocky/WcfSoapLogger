// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using WcfSoapLogger.Exceptions;

namespace WcfSoapLogger.HandlerCustom
{
    public static class SoapLoggerClient
    {
        [ThreadStatic]
        private static ISoapLoggerHandlerClient _client;

        public static void SetCustomHandlerCallbacks(ISoapLoggerHandlerClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            _client = client;
        }

        internal static void CallRequestCallback(byte[] requestBody, SoapLoggerSettings settings) 
        {
            if (_client == null)
            {
                const string methodName = "SoapLoggerClient.SetCustomHandlerCallbacks";
                throw new LoggerException("You have enabled 'useCustomHandler' for client class of given service. So method '" + methodName+ "' should be called before each request (typically in constructor of inherited client class).");
            }

            _client.HandleRequestBodyCallback(requestBody, settings);
        }

        internal static void CallResponseCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            try
            {
                _client.HandleResponseBodyCallback(responseBody, settings);
            }
            finally
            {
                _client = null;
            }
        }

        internal static void CallCustomHandlersDisabledCallback(SoapLoggerSettings settings)
        {
            if (_client != null)
            {
                _client.CustomHandlersDisabledCallback(settings);
            }
        }


    }
}
