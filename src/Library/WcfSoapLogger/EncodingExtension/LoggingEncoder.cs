// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Text;
using WcfSoapLogger.Exceptions;
using WcfSoapLogger.HandlerCustom;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingEncoder : MessageEncoder
    {
        private readonly LoggingEncoderFactory _factory;
        private readonly string _contentType;
        private readonly MessageEncoder _innerEncoder;
        private readonly SoapLoggerSettings _settings;
        private readonly HandlerDefault _handler;


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

            if (factory.Settings.UseCustomHandler)
            {
                _handler = new HandlerCustom.HandlerCustom(_settings);
            }
            else
            {
                _handler = new HandlerDefault(_settings);
            }
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
            //XOR, because request is writeMessage on client side and readMessage on service side
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
                    // on client side exception would naturally emerge for both request and response
                    throw;
                }

                byte[] errorBody = GetServiceSideErrorBody(ex, request);

                var errorBuffer = new ArraySegment<byte>(errorBody);
                return errorBuffer;
            }
        }


        private byte[] GetServiceSideErrorBody(Exception ex, bool request) {
            if (request)
            {
                //this can happen only with default handler on service side
                //typically when lacking access to file system 
                SoapLoggerService.SetRequestException(ex);

                //in order to force HTTP 500 Internal Server Error and avoid processing
                //we overwrite request with deliberately invalid body
                return ErrorBody.InvalidRequest();
            }

            // response
            string message = null;

            if (ex is LoggerException)
            {
                message = ex.Message;
            }
            else
            {
                message = ex.ToString();
            }

            string soapFault = ErrorBody.CreateSoapFaultResponse(message);
            byte[] errorBody = Encoding.UTF8.GetBytes(soapFault);
            return errorBody;
        }


  }
}
