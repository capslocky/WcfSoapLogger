namespace WcfSoapLogger.CustomHandlers
{
    public interface ISoapLoggerHandlerClient
    {
        void HandleRequestBodyCallback(byte[] requestBody, SoapLoggerSettings settings);
        void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings);
        void CustomHandlersDisabledCallback(SoapLoggerSettings settings);
    }
}
