using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfSoapLogger.FileWriting;

namespace WcfSoapLogger.CustomHandlers
{
    public class ServiceCustomHandler : ISoapLoggerHandlerService
    {
        public virtual void HandleRequestBody(byte[] requestBody, SoapLoggerSettings settings)
        {
            SoapLoggerTools.WriteFileDefault(requestBody, true, settings.LogPath);
        }

        public virtual void HandleResponseBodyCallback(byte[] responseBody, SoapLoggerSettings settings)
        {
            SoapLoggerTools.WriteFileDefault(responseBody, false, settings.LogPath);
        }

        public virtual void CustomHandlersDisabled(SoapLoggerSettings settings)
        {
        }

    }

}
