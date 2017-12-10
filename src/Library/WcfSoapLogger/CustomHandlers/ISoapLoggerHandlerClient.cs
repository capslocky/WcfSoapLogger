// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace WcfSoapLogger.CustomHandlers
{
    public interface ISoapLoggerHandlerClient
    {
        void HandleRequestBodyCallback(byte[] requestBody, SoapLoggerSettings settings);
        void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings);
        void CustomHandlersDisabledCallback(SoapLoggerSettings settings);
    }
}
