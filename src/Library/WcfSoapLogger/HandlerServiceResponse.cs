using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    internal class HandlerServiceResponse : Handler
    {
        internal override bool IsRequest {
            get{
                return false;
            }
        }

        protected override string CustomHandler(SoapLoggerSettings settings, byte[] body) 
        {
            SoapLoggerForService.CallResponseCallback(body, settings);
            return null;
        }
    }
}
