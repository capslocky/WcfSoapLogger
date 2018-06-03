using System;
using System.Text;
using WcfSoapLogger.EncodingExtension;
using WcfSoapLogger.Exceptions;

namespace WcfSoapLogger.HandlerCustom
{
    internal class HandlerCustom : HandlerDefault
    {
        public HandlerCustom(SoapLoggerSettings settings) : base(settings)
        {
        }

        //Debug.QuickWatch: Encoding.UTF8.GetString(body)


        public override void HandleBody(byte[] body, bool request)
        {
            if (_settings.IsService)
            {
                this.HandleBodyOnService(body, request);
            }
            else
            {
                this.HandleBodyOnClient(body, request);
            }
        }


        private void HandleBodyOnService(byte[] body, bool request)
        {
            if (request)
            {
                SoapLoggerService.SetSettings(_settings);
                SoapLoggerService.SetRequestBody(body);
                return;
            }

            // service response [5. logging ]

            var requestException = SoapLoggerService.GetRequestException();

            if (requestException != null)
            {
                throw requestException;
            }

            SoapLoggerService.CallResponseCallback(body, _settings);
        }


        private void HandleBodyOnClient(byte[] body, bool request)
        {
            if (request)
            {
                SoapLoggerClient.CallRequestCallback(body, _settings);
            }
            else
            {
                SoapLoggerClient.CallResponseCallback(body, _settings);
            }
        }
    }
}