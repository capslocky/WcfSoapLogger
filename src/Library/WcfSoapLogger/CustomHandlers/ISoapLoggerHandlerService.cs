namespace WcfSoapLogger.CustomHandlers
{
    public interface ISoapLoggerHandlerService
    {
        void HandleRequestBody(byte[] requestBody, SoapLoggerSettings settings);
        void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings);
        void CustomHandlersDisabled(SoapLoggerSettings settings);
    }
}
