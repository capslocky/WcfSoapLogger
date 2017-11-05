using System.Security;

namespace WcfSoapLogger
{
    internal static class ErrorBody
    {
        private const string SoapFaultTemplate = 
@"<?xml version=""1.0"" encoding=""utf-8""?>
<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
    <s:Body>
        <s:Fault>
            <faultcode>s:Server</faultcode>
            <faultstring>{0}</faultstring>
        </s:Fault>
    </s:Body>
</s:Envelope>";

        public static string GetSoapFault(string message)
        {
            string escapedMessage = SecurityElement.Escape(message);
            string faultBody = string.Format(SoapFaultTemplate, escapedMessage);
            return faultBody;
        }


    }
}
