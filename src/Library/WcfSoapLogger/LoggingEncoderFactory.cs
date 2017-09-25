using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    public class LoggingEncoderFactory : MessageEncoderFactory
    {
        private readonly MessageEncoder _encoder;
        private readonly MessageVersion _version;
        private readonly string _mediaType;
        private readonly string _charSet;
        private readonly MessageEncoderFactory _innerMessageFactory;

        public string LogPath { get; set; }

        public override MessageEncoder Encoder {
            get {
                return _encoder;
            }
        }

        public override MessageVersion MessageVersion {
            get {
                return _version;
            }
        }

        internal MessageEncoderFactory InnerMessageFactory {
            get {
                return _innerMessageFactory;
            }
        }

        internal string MediaType {
            get {
                return _mediaType;
            }
        }

        internal string CharSet {
            get {
                return _charSet;
            }
        }

        internal LoggingEncoderFactory(string mediaType, string charSet, MessageVersion version, MessageEncoderFactory messageFactory, string logPath) {
            _version = version;
            _mediaType = mediaType;
            _charSet = charSet;
            _innerMessageFactory = messageFactory;
            LogPath = logPath;
            _encoder = new LoggingEncoder(this);
        }
    }
}
