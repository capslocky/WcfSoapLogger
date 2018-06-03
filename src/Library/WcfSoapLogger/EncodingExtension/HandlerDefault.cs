using WcfSoapLogger.FileWriting;
using WcfSoapLogger.HandlerCustom;

namespace WcfSoapLogger.EncodingExtension
{
    internal class HandlerDefault
    {
        protected readonly SoapLoggerSettings _settings;

        public HandlerDefault(SoapLoggerSettings settings)
        {
            _settings = settings;
        }

        public virtual void HandleBody(byte[] body, bool request)
        {
            SoapLoggerTools.WriteFileDefault(body, request, _settings.LogPath);

            if (!_settings.UseCustomHandler && _settings.IsClient && request)
            {
                SoapLoggerClient.CallCustomHandlersDisabledCallback(_settings);
            }
        }

    }
}