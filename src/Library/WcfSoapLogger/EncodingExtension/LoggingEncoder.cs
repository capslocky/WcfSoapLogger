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
            byte[] body = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, body, 0, body.Length);

            try
            {
                HandleMessage(body, false);
            }
            catch (FileSystemAcccesDeniedException ex)
            {
                string xmlError = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body></s:Body></s:Envelope>";
                buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(xmlError));
            }

            return _innerEncoder.ReadMessage(buffer, bufferManager, contentType);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType) 
        {
            return _innerEncoder.ReadMessage(stream, maxSizeOfHeaders, contentType);
        }



        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset) 
        {
            ArraySegment<byte> arraySegment = _innerEncoder.WriteMessage(message, maxMessageSize, bufferManager, messageOffset);

            byte[] body = new byte[arraySegment.Count];
            Array.Copy(arraySegment.Array, arraySegment.Offset, body, 0, body.Length);

            try
            {
                HandleMessage(body, true);
            }
            catch (FileSystemAcccesDeniedException ex)
            {
                arraySegment = new ArraySegment<byte>(ErrorText(ex.Message));
            }

            return arraySegment;
        }

        private byte[] ErrorText(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("Error. " + text);
            return bytes;
        }

        public override void WriteMessage(Message message, Stream stream) 
        {
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
