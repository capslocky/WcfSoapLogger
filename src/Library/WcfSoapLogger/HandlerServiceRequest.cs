using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    internal class HandlerServiceRequest : Handler
    {
        internal override bool IsRequest {
            get{
                return true;
            }
        }

        protected override string CustomHandler(SoapLoggerSettings settings, byte[] body)
        {
            SoapLoggerForService.SetRequestBody(body, settings);
            return null;
        }
    }
}
