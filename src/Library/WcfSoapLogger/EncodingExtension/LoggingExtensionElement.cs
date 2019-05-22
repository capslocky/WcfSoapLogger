// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingExtensionElement : BindingElementExtensionElement
    {
        private const string LogPathName = "logPath";
        private const string SaveOriginalBinaryBodyName = "saveOriginalBinaryBody";
        private const string UseCustomHandlerName = "useCustomHandler";
        private const string MessageVersionName = "messageVersion";


        [ConfigurationProperty(LogPathName, IsRequired = true)]
        public string LogPath {
            get {
                return (string)base[LogPathName];
            }
        }

        [ConfigurationProperty(SaveOriginalBinaryBodyName, IsRequired = false, DefaultValue = "false")]
        public string SaveOriginalBinaryBody {
          get {
            return (string)base[SaveOriginalBinaryBodyName];
          }
        }

        [ConfigurationProperty(UseCustomHandlerName, IsRequired = false, DefaultValue = "false")]
        public string UseCustomHandler {
            get {
                return (string)base[UseCustomHandlerName];
            }
        }

        // possible values:
        // "Soap11"
        // "Soap11WSAddressing10"
        // "Soap11WSAddressingAugust2004"
        // "Soap12"
        // "Soap12WSAddressing10"
        // "Soap12WSAddressingAugust2004"
        // http://www.codemeit.com/wcf/soap11-vs-soap12-wsaddressing.html

        [ConfigurationProperty(MessageVersionName, IsRequired = false, DefaultValue = "Soap11")]
        public string MessageVersion
        {
            get
            {
                return (string)base[MessageVersionName];
            }
        }

        /*
            SOAP 1.1
            mediaType = "text/xml";
            contentType = "text/xml; charset=utf-8";
            <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">

            SOAP 1.2
            mediaType = "application/soap+xml"
            contentType = "application/soap+xml; charset=utf-8"
            <s:Envelope xmlns:s="http://www.w3.org/2003/05/soap-envelope">


            Addressing10
            xmlns:a="http://www.w3.org/2005/08/addressing"

            AddressingAugust2004
            xmlns:a="http://schemas.xmlsoap.org/ws/2004/08/addressing"
        */


        /*
        [ConfigurationProperty("writeEncoding", DefaultValue = "utf-8")]
        [TypeConverter(typeof(EncodingConverter))]
        public Encoding WriteEncoding
        {
            get
            {
                return (Encoding)this["writeEncoding"];
            }
            set
            {
                this["writeEncoding"] = (object)value;
            }
        }
        */

        protected override BindingElement CreateBindingElement() 
        {
            string logPath = LogPath;
            bool saveOriginalBinaryBody = ParseBoolean(SaveOriginalBinaryBody);
            bool useCustomHandler = ParseBoolean(UseCustomHandler);
            EnvelopeVersion envelopeVersion = ParseEnvelopeVersion(MessageVersion);
            AddressingVersion addressingVersion = ParseAddressingVersion(MessageVersion);
            var messageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(envelopeVersion, addressingVersion);

            var bindingElement = new LoggingBindingElement(
                    logPath,
                    saveOriginalBinaryBody,
                    useCustomHandler,
                    messageVersion
            );

            ApplyConfiguration(bindingElement);
            return bindingElement;
        }


        private EnvelopeVersion ParseEnvelopeVersion(string messageVersion)
        {
            try
            {
                messageVersion = messageVersion.ToLowerInvariant();

                if (messageVersion.StartsWith("soap11"))
                {
                    return EnvelopeVersion.Soap11;
                }

                if (messageVersion.StartsWith("soap12"))
                {
                    return EnvelopeVersion.Soap12;
                }

                if (messageVersion.StartsWith("none"))
                {
                    return EnvelopeVersion.None;
                }

                return EnvelopeVersion.Soap11;
            }
            catch
            {
                return EnvelopeVersion.Soap11;
            }
        }

        private AddressingVersion ParseAddressingVersion(string messageVersion)
        {
            try
            {
                messageVersion = messageVersion.ToLowerInvariant();

                if (messageVersion.EndsWith("addressing10"))
                {
                    return AddressingVersion.WSAddressing10;
                }

                if (messageVersion.EndsWith("addressing2004") || messageVersion.EndsWith("august2004"))
                {
                    return AddressingVersion.WSAddressingAugust2004;
                }

                return AddressingVersion.None;
            }
            catch
            {
                return AddressingVersion.None;
            }
        }

        private bool ParseBoolean(string value)
        {
            try
            {
                return bool.Parse(value);
            }
            catch
            {
                return false;
            }
        }

        public override Type BindingElementType {
            get { return typeof(LoggingBindingElement); }
        }
    }
}

// sources can be seen here
// https://referencesource.microsoft.com/#System.ServiceModel/System/ServiceModel/Channels/TextMessageEncoder.cs
// https://github.com/Microsoft/referencesource/blob/master/System.ServiceModel/System/ServiceModel/Configuration/TextMessageEncodingElement.cs
