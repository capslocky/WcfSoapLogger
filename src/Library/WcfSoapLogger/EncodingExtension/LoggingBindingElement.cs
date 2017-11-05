using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingBindingElement : MessageEncodingBindingElement, IWsdlExportExtension
    {
        public MessageEncodingBindingElement InnerMessageEncodingBindingElement { get; private set; }
        public SoapLoggerSettings Settings { get; private set; }

        private readonly MessageVersion _messageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap11, AddressingVersion.None);

        private LoggingBindingElement(){
        }

        public LoggingBindingElement(string logPath, string useCustomHandler)
        {
            this.InnerMessageEncodingBindingElement = new TextMessageEncodingBindingElement();
            this.InnerMessageEncodingBindingElement.MessageVersion = _messageVersion;

            this.Settings = new SoapLoggerSettings();
            this.Settings.LogPath = logPath;
            this.Settings.UseCustomHandler = useCustomHandler.Equals(Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase);
        }

        public override MessageVersion MessageVersion {
            get {
                return InnerMessageEncodingBindingElement.MessageVersion;
            }
            set {
                InnerMessageEncodingBindingElement.MessageVersion = value;
            }
        }

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new LoggingEncoderFactory(this.Settings, "text/xml", _messageVersion, InnerMessageEncodingBindingElement.CreateMessageEncoderFactory());
        }

        public override BindingElement Clone() 
        {
            return new LoggingBindingElement() 
            {
                InnerMessageEncodingBindingElement = this.InnerMessageEncodingBindingElement,
                Settings = this.Settings,
            };
        }

        public override T GetProperty<T>(BindingContext context) 
        {
            return typeof(T) == typeof(XmlDictionaryReaderQuotas) ? InnerMessageEncodingBindingElement.GetProperty<T>(context) : base.GetProperty<T>(context);
        }

        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context) 
        {
            if (context == null)
                throw new ArgumentNullException("context");

            Settings.IsService = false;
            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }


        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context) 
        {
            if (context == null)
                throw new ArgumentNullException("context");

            Settings.IsService = true;
            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }


        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context) 
        {
            ((IWsdlExportExtension)InnerMessageEncodingBindingElement).ExportContract(exporter, context);
        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context) 
        {
            ((IWsdlExportExtension)InnerMessageEncodingBindingElement).ExportEndpoint(exporter, context);
        }
    }
}
