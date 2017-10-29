using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    internal class HandlerServiceResponse : Handler
    {
        internal override byte[] HandleBody(SoapLoggerSettings settings, byte[] body)
        {
            throw new NotImplementedException();
        }
    }
}
