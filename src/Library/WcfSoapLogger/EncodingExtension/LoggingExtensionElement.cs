// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace WcfSoapLogger.EncodingExtension
{
    public class LoggingExtensionElement : BindingElementExtensionElement
    {
        private const string LogPathName = "logPath";
        private const string SaveOriginalBinaryBodyName = "saveOriginalBinaryBody";
        private const string UseCustomHandlerName = "useCustomHandler";


        [ConfigurationProperty(LogPathName, IsRequired = true)]
        public string LogPath {
            get {
                return (string)base[LogPathName];
            }
        }

        [ConfigurationProperty(SaveOriginalBinaryBodyName, IsRequired = false)]
        public string SaveOriginalBinaryBody {
          get {
            return (string)base[SaveOriginalBinaryBodyName];
          }
        }

        [ConfigurationProperty(UseCustomHandlerName, IsRequired = false)]
        public string UseCustomHandler {
            get {
                return (string)base[UseCustomHandlerName];
            }
        }

        protected override BindingElement CreateBindingElement() 
        {
            bool saveOriginalBinaryBody = ParseBoolean(SaveOriginalBinaryBody);
            bool useCustomHandler = ParseBoolean(UseCustomHandler);

            var bindingElement = new LoggingBindingElement(LogPath, saveOriginalBinaryBody, useCustomHandler);
            ApplyConfiguration(bindingElement);
            return bindingElement;
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
