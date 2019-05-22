// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingBindingElement : MessageEncodingBindingElement, IWsdlExportExtension
    {
        public MessageEncodingBindingElement InnerMessageEncodingBindingElement { get; private set; }
        public SoapLoggerSettings Settings { get; private set; }


        private LoggingBindingElement(MessageEncodingBindingElement messageEncodingBinding, SoapLoggerSettings settings)
        {
            this.InnerMessageEncodingBindingElement = messageEncodingBinding;
            this.Settings = settings;
        }

        public LoggingBindingElement(string logPath, bool saveOriginalBinaryBody, bool useCustomHandler, System.ServiceModel.Channels.MessageVersion messageVersion)
        {
            var encoding = Encoding.UTF8; // we can set it via config if needed
            this.InnerMessageEncodingBindingElement = new TextMessageEncodingBindingElement(messageVersion, encoding);

            this.Settings = new SoapLoggerSettings();
            this.Settings.LogPath = logPath;
            this.Settings.SaveOriginalBinaryBody = saveOriginalBinaryBody;
            this.Settings.UseCustomHandler = useCustomHandler;
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
            return new LoggingEncoderFactory(
                this.Settings,
                InnerMessageEncodingBindingElement.CreateMessageEncoderFactory()
            );
        }

        public override BindingElement Clone()
        {
            return new LoggingBindingElement(
                this.InnerMessageEncodingBindingElement, 
                this.Settings
            );
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
