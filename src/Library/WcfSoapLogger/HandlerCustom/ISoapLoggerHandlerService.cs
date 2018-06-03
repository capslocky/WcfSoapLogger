// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace WcfSoapLogger.HandlerCustom
{
    public interface ISoapLoggerHandlerService
    {
        void HandleRequestBody(byte[] requestBody, SoapLoggerSettings settings);
        void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings);
        void CustomHandlersDisabled(SoapLoggerSettings settings);
    }
}
