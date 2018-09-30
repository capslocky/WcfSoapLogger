using WcfSoapLogger.EncodingExtension;

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
        
        // see stages
        // https://github.com/capslocky/WcfSoapLogger/blob/master/docs/GeneralFlow.md

        private void HandleBodyOnService(byte[] body, bool request)
        {
            if (request)
            {
                // service request [stage 3. logging]

                SoapLoggerService.SetSettings(_settings);
                SoapLoggerService.SetRequestBody(body);
            }
            else
            {
                // service response [stage 5. logging ]

                var requestException = SoapLoggerService.GetRequestException();

                if (requestException != null)
                {
                  throw requestException;
                }

                SoapLoggerService.CallResponseCallback(body, _settings);
            }
        }


        private void HandleBodyOnClient(byte[] body, bool request)
        {
            if (request)
            {
              // client request [stage 2. logging]
                SoapLoggerClient.CallRequestCallback(body, _settings);
            }
            else
            {
              // client response [stage 6. logging ]
                SoapLoggerClient.CallResponseCallback(body, _settings);
            }
        }
    }
}