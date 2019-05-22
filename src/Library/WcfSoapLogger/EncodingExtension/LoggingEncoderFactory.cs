// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.ServiceModel.Channels;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingEncoderFactory : MessageEncoderFactory
    {
        private readonly MessageVersion _messageVersion;
        private readonly MessageEncoder _encoder;

        public override MessageVersion MessageVersion { get { return _messageVersion; } }
        public override MessageEncoder Encoder { get { return _encoder; } }

        internal LoggingEncoderFactory(SoapLoggerSettings settings, MessageEncoderFactory messageFactory)
        {
            // messageFactory is an instance of internal class 'TextMessageEncoderFactory'
            // encoder is an instance of internal class 'TextMessageEncoder'

            var encoder = messageFactory.Encoder;

            _messageVersion = messageFactory.MessageVersion;
            _encoder = new LoggingEncoder(encoder, settings);
        }
    }
}
