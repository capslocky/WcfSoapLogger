using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace WcfSoapLogger.EncodingExtension
{
    public class BindingElement : MessageEncodingBindingElement, IWsdlExportExtension
    {
        private readonly MessageVersion _messageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap11, AddressingVersion.None);
        private MessageEncodingBindingElement _innerBindingElement;

        public string LogPath { get; set; }
        public string CustomCode { get; set; }

        public MessageEncodingBindingElement InnerMessageEncodingBindingElement {
            get {
                return _innerBindingElement;
            }
            set {
                _innerBindingElement = value;
            }
        }


        public override MessageVersion MessageVersion {
            get {
                return _innerBindingElement.MessageVersion;
            }
            set {
                _innerBindingElement.MessageVersion = value;
            }
        }


        public BindingElement(string logPath, string customCode) : this(new TextMessageEncodingBindingElement())
        {
            LogPath = logPath;
            CustomCode = customCode;
        }

        public BindingElement(MessageEncodingBindingElement messageEncoderBindingElement) 
        {
            _innerBindingElement = messageEncoderBindingElement;
            _innerBindingElement.MessageVersion = _messageVersion;
        }

        public override MessageEncoderFactory CreateMessageEncoderFactory() {
            return new LoggingEncoderFactory("text/xml", "utf-8", _messageVersion, _innerBindingElement.CreateMessageEncoderFactory(), LogPath, CustomCode);
        }

        public override System.ServiceModel.Channels.BindingElement Clone() {
            return new BindingElement(_innerBindingElement) {
                LogPath = LogPath,
                CustomCode = CustomCode
            };
        }

        public override T GetProperty<T>(BindingContext context) {
            return typeof(T) == typeof(XmlDictionaryReaderQuotas) ? _innerBindingElement.GetProperty<T>(context) : base.GetProperty<T>(context);
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context) {
            if (context == null)
                throw new ArgumentNullException("context");
            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context) {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context) {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context) {

        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context) {
            ((IWsdlExportExtension)_innerBindingElement).ExportEndpoint(exporter, context);
        }
    }
}
