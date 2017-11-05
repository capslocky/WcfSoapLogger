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
        private readonly HandlerAbstract _handler;

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
            _handler = factory.Settings.IsService ? (HandlerAbstract) new HandlerService(factory.Settings) : new HandlerClient(factory.Settings);
        }


        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            ArraySegment<byte> errorBuffer = HandleMessage(buffer, false);

            if (errorBuffer.Array != null)
            {
                buffer = errorBuffer;
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

            ArraySegment<byte> errorBuffer = HandleMessage(buffer, true);

            if (errorBuffer.Array != null)
            {
                buffer = errorBuffer;
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

            try
            {
                byte[] body = new byte[buffer.Count];
                Array.Copy(buffer.Array, buffer.Offset, body, 0, body.Length);

                _handler.HandleBody(body, request);
                return new ArraySegment<byte>();
            }
            catch (Exception ex)
            {
                if (_settings.IsClient)
                {
                    throw;
                }

                byte[] errorBody = _handler.GetErrorBody(ex, request);
                var errorBuffer = new ArraySegment<byte>(errorBody);
                return errorBuffer;
            }
        }


    

    }
}
