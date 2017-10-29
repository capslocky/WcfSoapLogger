using System;

namespace WcfSoapLogger
{
    internal abstract class Handler
    {
        internal abstract byte[] HandleBody(SoapLoggerSettings settings, byte[] body);
    }
}
