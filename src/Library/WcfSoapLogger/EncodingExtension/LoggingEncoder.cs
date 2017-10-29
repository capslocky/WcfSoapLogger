using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Text;
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

        public LoggingEncoder(LoggingEncoderFactory factory) 
        {
            _factory = factory;
            _innerEncoder = factory.InnerMessageFactory.Encoder;
            _contentType = factory.MediaType;
            _settings = factory.Settings;
        }


        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType) 
        {
            ArraySegment<byte> bufferError = HandleMessage(buffer, false);

            if (bufferError.Count > 0)
            {
                buffer = bufferError;
            }

            var message = _innerEncoder.ReadMessage(buffer, bufferManager, contentType);
            return message;
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType) 
        {
            return _innerEncoder.ReadMessage(stream, maxSizeOfHeaders, contentType);
        }


        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset) 
        {
            ArraySegment<byte> buffer = _innerEncoder.WriteMessage(message, maxMessageSize, bufferManager, messageOffset);
            ArraySegment<byte> bufferError = HandleMessage(buffer, true);

            if (bufferError.Count > 0)
            {
                buffer = bufferError;
            }

            return buffer;
        }

        public override void WriteMessage(Message message, Stream stream) 
        {
            _innerEncoder.WriteMessage(message, stream);
        }


        private ArraySegment<byte> HandleMessage(ArraySegment<byte> buffer, bool writeMessage) 
        {
            //XOR, because request is writeMessage for client and readMessage for web-service
            bool request = writeMessage ^ _settings.IsService;

            byte[] body = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, body, 0, body.Length);

            Handler handler;

            if (_settings.IsService)
            {
                handler = request ? (Handler) new HandlerServiceRequest() : new HandlerServiceResponse();
            }
            else
            {
                handler = request ? (Handler) new HandlerClientRequest() : new HandlerClientResponse();
            }

            byte[] errorBody =  handler.HandleBody(_settings, body);

            if (errorBody != null)
            {
                return new ArraySegment<byte>(errorBody);
            }

            return new ArraySegment<byte>();
        }

    }
}
