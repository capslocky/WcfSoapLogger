using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    internal class HandlerClientRequest : Handler
    {
        internal override bool IsRequest{
            get{
                return true;
            }
        }

        protected override string CustomHandler(SoapLoggerSettings settings, byte[] body) 
        {
            SoapLoggerForClient.CallRequestCallback(body, settings);
            return null;
        }
    }
}
