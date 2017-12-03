using System;
using System.ServiceModel.Configuration;

namespace Client
{
    public class SigmaLoggerBehaviorExtension : BehaviorExtensionElement
    {
        protected override object CreateBehavior() {
            return new SigmaLoggerMessageInspector();
        }

        public override Type BehaviorType {
            get
            {
                return typeof(SigmaLoggerMessageInspector);
            }
        }
    }
}
