using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace WcfSoapLogger
{
    internal abstract class HandlerAbstract
    {
        protected SoapLoggerSettings settings;

        internal HandlerAbstract(SoapLoggerSettings settings)
        {
            this.settings = settings;
        }

        //Debug.QuickWatch: Encoding.UTF8.GetString(body)

        internal void HandleBody(byte[] body, bool request)
        {
            if (settings.IsService && !request)
            {
                var requestException = SoapLoggerForService.GetRequestException();

                if (requestException != null)
                {
                    throw requestException;
                }
            }

            if (settings.UseCustomHandler)
            {
                if (request)
                {
                    HandleRequest(body);
                }
                else
                {
                    HandleResponse(body);
                }

                return;
            }

            //default handler
            SoapLoggerTools.WriteFileDefault(body, request, settings.LogPath);
        }


        internal byte[] GetErrorBody(Exception ex, bool request)
        {
            if (request)
            {
                return GetRequestErrorBody(ex);
            }

            return GetResponseErrorBody(ex);
        }

        protected abstract void HandleRequest(byte[] body);
        protected abstract void HandleResponse(byte[] body);

        protected abstract byte[] GetRequestErrorBody(Exception ex);
        protected abstract byte[] GetResponseErrorBody(Exception ex);
    }
}
