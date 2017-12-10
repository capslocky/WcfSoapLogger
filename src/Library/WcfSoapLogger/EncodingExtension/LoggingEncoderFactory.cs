// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.ServiceModel.Channels;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingEncoderFactory : MessageEncoderFactory
    {
        internal SoapLoggerSettings Settings { get; private set; }
        internal string MediaType { get; private set; }
        internal MessageEncoderFactory InnerMessageFactory { get; private set; }

        private readonly MessageVersion _messageVersion;
        private readonly MessageEncoder _encoder;

        public override MessageVersion MessageVersion { get { return _messageVersion; } }
        public override MessageEncoder Encoder { get { return _encoder; } }

        internal LoggingEncoderFactory(SoapLoggerSettings settings, string mediaType, MessageVersion version, MessageEncoderFactory messageFactory)
        {
            Settings = settings;
            MediaType = mediaType;
            InnerMessageFactory = messageFactory;

            _messageVersion = version;
            _encoder = new LoggingEncoder(this);
        }
    }
}
