using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Xml;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingEncoder : MessageEncoder
    {
        private readonly LoggingEncoderFactory _factory;
        private readonly string _contentType;
        private readonly MessageEncoder _innerEncoder;
        private readonly string _logPath;
        private readonly string _customCode;


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
            SoapLoggerThreadStatic.SetEncoder(this);

            _factory = factory;
            _innerEncoder = factory.InnerMessageFactory.Encoder;
            _contentType = _factory.MediaType;
            _logPath = _factory.LogPath;
            _customCode = _factory.CustomCode;

            Directory.CreateDirectory(_logPath);
        }

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType) {
            byte[] bytes = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, bytes, 0, bytes.Length);

            LogBytes(bytes, false);
            return _innerEncoder.ReadMessage(buffer, bufferManager, contentType);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType) {
            return _innerEncoder.ReadMessage(stream, maxSizeOfHeaders, contentType);
        }



        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset) {
            ArraySegment<byte> arraySegment = _innerEncoder.WriteMessage(message, maxMessageSize, bufferManager, messageOffset);

            var bytes = new byte[arraySegment.Count];
            Array.Copy(arraySegment.Array, arraySegment.Offset, bytes, 0, bytes.Length);

            LogBytes(bytes, true);
            return arraySegment;
        }

        public override void WriteMessage(Message message, Stream stream) {
            _innerEncoder.WriteMessage(message, stream);
        }


        private void LogBytes(byte[] bytes, bool response)
        {
            bool customCode = !string.IsNullOrEmpty(_customCode) && Boolean.Parse(_customCode);

            if (customCode)
            {
                LogBytesCustomCode(bytes, response);
                return;
            }

            SoapLoggerTools.LogBytes(bytes, response, _logPath);
        }

        private void LogBytesCustomCode(byte[] bytes, bool response)
        {
            if (response)
            {
                SoapLoggerThreadStatic.Service.LogResponseBody(bytes);
            }
            else
            {
                SoapLoggerThreadStatic.RequestBody = bytes;
            }
        }



    }
}
