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
            SoapLoggerTools.LogBytes(body, response, settings.LogPath);

            if (response)
            {
                SoapLoggerThreadStatic.CallResponseCallback(body, settings);
            }
            else
            {
                SoapLoggerThreadStatic.SetRequestBody(body, settings);
            }
        }
    }
}
