using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfSoapLogger.Tests.FullCycle.Client;

namespace WcfSoapLogger.Tests.FullCycle
{
    [TestClass]
    public class FullCycleDefaultService : FullCycleBase
    {
        protected override ServiceHost GetServiceHost(){
            return GetServiceWithDefaultLogging();
        }

        protected override PriceServiceClient GetClient() {
            return GetClientStandard();
        }

        [TestMethod]
        public void Check() {
            RunFullCycle();
        }
    }
}
