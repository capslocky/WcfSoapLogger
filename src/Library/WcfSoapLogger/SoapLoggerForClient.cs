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
                try
                {
                    RequestBodyCallback(requestBody, settings);
                }
                catch (Exception ex)
                {
                    //TODO save exception info
                    HandleRequestError(requestBody, settings);
                }
                finally
                {
                    RequestBodyCallback = null;
                }
            }
            else
            {
                //method 'SetRequestAndResponseCallbacks' has not been called by client
                HandleRequestError(requestBody, settings);
            }
        }


        internal static void CallResponseCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            if (ResponseBodyCallback != null)
            {
                try
                {
                    ResponseBodyCallback(responseBody, settings);
                }
                catch (Exception ex)
                {
                    //TODO save exception info
                    HandleResponseError(responseBody, settings);
                }
                finally
                {
                    ResponseBodyCallback = null;
                }
            }
            else
            {
                //method 'SetRequestAndResponseCallbacks' has not been called by client
                HandleResponseError(responseBody, settings);
            }
        }


        private static void HandleRequestError(byte[] requestBody, SoapLoggerSettings settings)
        {
            SoapLoggerTools.WriteFileDefault(requestBody, true, settings.LogPath);
        }

        private static void HandleResponseError(byte[] responseBody, SoapLoggerSettings settings) 
        {
            SoapLoggerTools.WriteFileDefault(responseBody, false, settings.LogPath);
        }

    }
}
