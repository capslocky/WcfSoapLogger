using System.Security;
using System.Text;

namespace WcfSoapLogger.Exceptions
{
    internal static class ErrorBody
    {
        public static string GetSoapFault(string message)
        {
            string escapedMessage = SecurityElement.Escape(message);
            string faultBody = string.Format(SoapFaultResponseTemplate, escapedMessage);
            return faultBody;
        }

        private const string SoapFaultResponseTemplate =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
    <s:Body>
        <s:Fault>
            <faultcode>s:Server</faultcode>
            <faultstring>{0}</faultstring>
        </s:Fault>
    </s:Body>
</s:Envelope>";


        static ErrorBody()
        {
            SoapInvalidRequestBytes = Encoding.UTF8.GetBytes(SoapInvalidRequest);
        }

        public static byte[] GetSoapInvalidRequest() 
        {
            return SoapInvalidRequestBytes;
        }


        private static readonly byte[] SoapInvalidRequestBytes;

        private const string SoapInvalidRequest =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
    <s:Body>
        <WcfSoapLoggerRequestError xmlns=""http://wcf-soap-logger.org/"">Exception</WcfSoapLoggerRequestError>
    </s:Body>
</s:Envelope>";


 

    }
}
