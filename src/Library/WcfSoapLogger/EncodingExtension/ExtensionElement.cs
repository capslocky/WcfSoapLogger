using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace WcfSoapLogger.EncodingExtension
{
    public class ExtensionElement : BindingElementExtensionElement
    {
        private const string LogPathName = "logPath";
        private const string CustomCodeName = "customCode";

        [ConfigurationProperty(LogPathName, IsRequired = true)]
        public string LogPath {
            get {
                return (string)base[LogPathName];
            }

            set {
                base[LogPathName] = value;
            }
        }


        [ConfigurationProperty(CustomCodeName, IsRequired = false)]
        public string CustomCode {
            get {
                return (string)base[CustomCodeName];
            }

            set {
                base[CustomCodeName] = value;
            }
        }

        protected override System.ServiceModel.Channels.BindingElement CreateBindingElement() {
            var bindingElement = new BindingElement(LogPath, CustomCode);
            ApplyConfiguration(bindingElement);
            return bindingElement;
        }

        public override Type BindingElementType {
            get { return typeof(BindingElement); }
        }
    }
}
