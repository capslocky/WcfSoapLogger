// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfSoapLogger.CustomHandlers;

namespace WcfSoapLogger
{
    internal class HandlerClient : HandlerAbstract
    {
        public HandlerClient(SoapLoggerSettings settings) : base(settings) {
        }

        protected override void HandleRequest(byte[] body)
        {
            SoapLoggerClient.CallRequestCallback(body, settings);
        }

        protected override void HandleResponse(byte[] body)
        {
            SoapLoggerClient.CallResponseCallback(body, settings);
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
