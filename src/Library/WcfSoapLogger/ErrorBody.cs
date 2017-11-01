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
            <faultcode>s:{0}</faultcode>
            <faultstring>{1}</faultstring>
        </s:Fault>
    </s:Body>
</s:Envelope>";

        public static string GetSoapFault(string message, bool server = true)
        {
            string code = server ? "Server" : "Client";
            string escapedMessage = SecurityElement.Escape(message);

            string faultBody = string.Format(SoapFaultTemplate, code, escapedMessage);
            return faultBody;
        }


    }
}
