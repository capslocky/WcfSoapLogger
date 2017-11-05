using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    internal class HandlerClient : HandlerAbstract
    {
        public HandlerClient(SoapLoggerSettings settings) : base(settings) {
        }

        protected override void HandleRequest(byte[] body)
        {
            SoapLoggerForClient.CallRequestCallback(body, settings);
        }

        protected override void HandleResponse(byte[] body)
        {
            SoapLoggerForClient.CallResponseCallback(body, settings);
        }

        protected override byte[] GetRequestErrorBody(Exception ex)
        {
            throw new InvalidOperationException("Client logging should throw an exception. No body substitution.");
        }

        protected override byte[] GetResponseErrorBody(Exception ex)
        {
            throw new InvalidOperationException("Client logging should throw an exception. No body substitution.");
        }
    }
}
