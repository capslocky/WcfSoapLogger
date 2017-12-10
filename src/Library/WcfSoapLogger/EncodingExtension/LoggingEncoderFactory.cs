// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.ServiceModel.Channels;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingEncoderFactory : MessageEncoderFactory
    {
        internal SoapLoggerSettings Settings { get; }
        internal string MediaType { get; }
        internal MessageEncoderFactory InnerMessageFactory { get; }

        public override MessageVersion MessageVersion { get; }
        public override MessageEncoder Encoder { get; }

        internal LoggingEncoderFactory(SoapLoggerSettings settings, string mediaType, MessageVersion version, MessageEncoderFactory messageFactory)
        {
            Settings = settings;
            MediaType = mediaType;
            MessageVersion = version;
            InnerMessageFactory = messageFactory;
            Encoder = new LoggingEncoder(this);
        }
    }
}
