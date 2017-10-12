using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    public static class SoapLoggerHandler
    {
        public static void ProcessBody(byte[] body, bool response, SoapLoggerSettings settings)
        {
            //TODO allow disabling default logging from config
            SoapLoggerTools.LogBytes(body, response, settings.LogPath);


            if (settings.IsService)
            {
                if (response)
                {
                    SoapLoggerForService.CallResponseCallback(body, settings);
                }
                else
                {
                    SoapLoggerForService.SetRequestBody(body, settings);
                }
            }

            if (settings.IsClient)
            {
                if (response)
                {
                    SoapLoggerForClient.CallResponseCallback(body, settings);
                }
                else
                {
                    SoapLoggerForClient.CallRequestCallback(body, settings);
                }
            }
        }
    }
}
