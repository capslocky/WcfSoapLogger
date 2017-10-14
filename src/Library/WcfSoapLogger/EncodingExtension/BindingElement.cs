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

        public SoapLoggerSettings Settings { get; set; }

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


        public BindingElement(string logPath, string useCustomHandler) : this(new TextMessageEncodingBindingElement())
        {
            this.Settings = new SoapLoggerSettings();
            this.Settings.LogPath = logPath;
            this.Settings.UseCustomHandler = useCustomHandler.Equals(Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase);
        }

        public BindingElement(MessageEncodingBindingElement messageEncoderBindingElement) 
        {
            _innerBindingElement = messageEncoderBindingElement;
            _innerBindingElement.MessageVersion = _messageVersion;
        }

        public override MessageEncoderFactory CreateMessageEncoderFactory() {
            return new LoggingEncoderFactory(this.Settings, "text/xml", "utf-8", _messageVersion, _innerBindingElement.CreateMessageEncoderFactory());
        }

        public override System.ServiceModel.Channels.BindingElement Clone() {
            return new BindingElement(_innerBindingElement) {
                Settings = this.Settings
            };
        }

        public override T GetProperty<T>(BindingContext context) {
            return typeof(T) == typeof(XmlDictionaryReaderQuotas) ? _innerBindingElement.GetProperty<T>(context) : base.GetProperty<T>(context);
        }

        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);

            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context) {
            if (context == null)
                throw new ArgumentNullException("context");

            this.Settings.IsService = false;
            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }


        public override bool CanBuildChannelListener<TChannel>(BindingContext context) {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context) {
            if (context == null)
                throw new ArgumentNullException("context");

            this.Settings.IsService = true;
            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }


        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context) {
        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context) {
            ((IWsdlExportExtension)_innerBindingElement).ExportEndpoint(exporter, context);
        }
    }
}
