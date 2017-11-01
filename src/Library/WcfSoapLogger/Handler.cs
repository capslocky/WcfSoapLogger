using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace WcfSoapLogger
{
    internal abstract class Handler
    {
        internal abstract bool IsRequest { get; }

        internal byte[] HandleBody(SoapLoggerSettings settings, byte[] body)
        {
            string errorMessage;

            try
            {
                if (settings.UseCustomHandler)
                {
                    errorMessage = CustomHandler(settings, body);
                }
                else
                {
                    errorMessage = DefaultHandler(settings, body);
                }
            }
            catch (LoggerException ex)
            {
                errorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }

            if (errorMessage == null)
            {
                return null;
            }

            string soapFault = ErrorBody.GetSoapFault(errorMessage);
            byte[] errorBody = Encoding.UTF8.GetBytes(soapFault);
            return errorBody;
        }


        private string DefaultHandler(SoapLoggerSettings settings, byte[] body)
        {
            SoapLoggerTools.WriteFileDefault(body, IsRequest, settings.LogPath);
            return null;
        }

        protected abstract string CustomHandler(SoapLoggerSettings settings, byte[] body);


    }
}
