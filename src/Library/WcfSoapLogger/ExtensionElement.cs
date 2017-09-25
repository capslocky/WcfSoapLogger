using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    public class ExtensionElement : BindingElementExtensionElement
    {
        private const string LogPathName = "logPath";

        [ConfigurationProperty(LogPathName, IsRequired = true)]
        public string LogPath {
            get {
                return (string)base[LogPathName];
            }

            set {
                base[LogPathName] = value;
            }
        }

        protected override System.ServiceModel.Channels.BindingElement CreateBindingElement() {
            var bindingElement = new BindingElement(LogPath);
            ApplyConfiguration(bindingElement);
            return bindingElement;
        }

        public override Type BindingElementType {
            get { return typeof(BindingElement); }
        }
    }
}
