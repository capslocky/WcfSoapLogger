// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using WcfSoapLogger.Exceptions;
using WcfSoapLogger.FileWriting;

namespace WcfSoapLogger.HandlerCustom
{
    public static class SoapLoggerService
    {
        // 'ThreadStaticAttribute' makes static field unique for every separate thread
        // That's how we can relate intercepted request/response bodies with appropriate execution of external method

        [ThreadStatic]
        private static SoapLoggerSettings _settings;

        [ThreadStatic]
        private static byte[] _requestBody;

        [ThreadStatic]
        private static ISoapLoggerHandlerService _service;

        [ThreadStatic]
        private static Exception _requestException;


        internal static void SetSettings(SoapLoggerSettings settings) 
        {
            _settings = settings;
        }

        internal static void SetRequestBody(byte[] requestBody) 
        {
            _requestBody = requestBody;
        }

        public static void CallCustomHandlers(ISoapLoggerHandlerService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            if (_settings == null)
            {
                throw new InvalidOperationException("_settings is null");
            }

            if (_requestBody == null)
            {
                service.CustomHandlersDisabled(_settings);
                return;
            }

            _service = service;

            try
            {
                _service.HandleRequestBody(_requestBody, _settings);
            }
            finally
            {
                _requestBody = null;
                _settings = null;
            }
        }

        internal static void CallResponseCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            if (_requestBody != null)
            {
                //something went wrong, either pipeline execution didn't reach web-service method 
                //or web-service method didn't call 'ReadRequestSetResponseCallback'

                //TODO determine case and log both files for first case

                var requestBody = _requestBody;
                _requestBody = null;

                SoapLoggerTools.WriteFileDefault(requestBody, true, settings.LogPath, false);
                SoapLoggerTools.WriteFileDefault(responseBody, false, settings.LogPath, false);

                //TODO add logging error message to file

                throw new LoggerException("something went wrong, either pipeline execution didn't reach web-service method or web-service method didn't execute 'SoapLoggerService.CallCustomHandlers'");
            }

            try
            {
                _service.HandleResponseBodyCallback(responseBody, settings);
            }
            finally
            {
                _service = null;
            }
        }

        internal static void SetRequestException(Exception ex)
        {
            _requestException = ex;
        }


        internal static Exception GetRequestException() 
        {
            var ex = _requestException;
            _requestException = null;
            return ex;
        }
    }
}
