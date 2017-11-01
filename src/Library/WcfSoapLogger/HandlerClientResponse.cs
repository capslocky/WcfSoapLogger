using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    internal class HandlerClientResponse : Handler
    {
        internal override bool IsRequest {
            get{
                return false;
            }
        }

        protected override string CustomHandler(SoapLoggerSettings settings, byte[] body) 
        {
            SoapLoggerForClient.CallResponseCallback(body, settings);
            return null;
        }
    }
}
