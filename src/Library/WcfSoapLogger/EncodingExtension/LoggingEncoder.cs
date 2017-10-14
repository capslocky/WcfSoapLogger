using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Threading;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingEncoder : MessageEncoder
    {
        private readonly LoggingEncoderFactory _factory;
        private readonly string _contentType;
        private readonly MessageEncoder _innerEncoder;
        private readonly SoapLoggerSettings _settings;

        public override string ContentType {
            get {
                return _contentType;
            }
        }

        public override string MediaType {
            get {
                return _factory.MediaType;
            }
        }

        public override MessageVersion MessageVersion {
            get {
                return _factory.MessageVersion;
            }
        }

        public LoggingEncoder(LoggingEncoderFactory factory) {
            _factory = factory;
            _innerEncoder = factory.InnerMessageFactory.Encoder;
            _contentType = factory.MediaType;
            _settings = factory.Settings;
        }

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType) {
            byte[] body = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, body, 0, body.Length);
            HandleMessage(body, false);

            return _innerEncoder.ReadMessage(buffer, bufferManager, contentType);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType) {
            return _innerEncoder.ReadMessage(stream, maxSizeOfHeaders, contentType);
        }



        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset) {
            ArraySegment<byte> arraySegment = _innerEncoder.WriteMessage(message, maxMessageSize, bufferManager, messageOffset);

            var body = new byte[arraySegment.Count];
            Array.Copy(arraySegment.Array, arraySegment.Offset, body, 0, body.Length);
            HandleMessage(body, true);

            return arraySegment;
        }

        public override void WriteMessage(Message message, Stream stream) {
            _innerEncoder.WriteMessage(message, stream);
        }


        private void HandleMessage(byte[] body, bool writeMessage)
        {
            //XOR, because request is writeMessage for client and readMessage for web-service
            bool request = writeMessage ^ _settings.IsService;
            SoapLoggerHandler.HandleBody(body, request, _settings);
        }


    }
}
