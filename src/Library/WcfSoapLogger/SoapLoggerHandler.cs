using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    internal static class SoapLoggerHandler
    {
        internal static void HandleBody(byte[] body, bool request, SoapLoggerSettings settings)
        {
            try
            {

            }
            catch (Exception ex)
            {
                
            }
            
            if (!settings.UseCustomHandler)
            {
                SoapLoggerTools.WriteFileDefault(body, request, settings.LogPath);
                return;
            }

            if (settings.IsService)
            {
                if (request)
                {
                    SoapLoggerForService.SetRequestBody(body, settings);
                }
                else
                {
                    SoapLoggerForService.CallResponseCallback(body, settings);
                }
            }

            if (settings.IsClient)
            {
                if (request)
                {
                    SoapLoggerForClient.CallRequestCallback(body, settings);
                }
                else
                {
                    SoapLoggerForClient.CallResponseCallback(body, settings);
                }
            }
        }
    }
}
