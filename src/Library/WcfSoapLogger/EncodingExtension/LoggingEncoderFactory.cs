using System.ServiceModel.Channels;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingEncoderFactory : MessageEncoderFactory
    {
        internal SoapLoggerSettings Settings { get; }
        internal string MediaType { get; }
//        internal string CharSet { get; }
        internal MessageEncoderFactory InnerMessageFactory { get; }

        public override MessageVersion MessageVersion { get; }
        public override MessageEncoder Encoder { get; }

        internal LoggingEncoderFactory(SoapLoggerSettings settings, string mediaType, string charSet, MessageVersion version, MessageEncoderFactory messageFactory)
        {
            Settings = settings;
            MediaType = mediaType;
//            CharSet = charSet;
            MessageVersion = version;
            InnerMessageFactory = messageFactory;
            Encoder = new LoggingEncoder(this);
        }
    }
}
