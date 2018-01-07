using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfSoapLogger.Tests.FullCycle.Client;

namespace WcfSoapLogger.Tests.FullCycle
{
    [TestClass]
    public class FullCycleDefaultClient : FullCycleBase
    {
        protected override ServiceHost GetServiceHost(){
            return GetServiceStandard();
        }

        protected override PriceServiceClient GetClient() {
            return GetClientWithDefaultLogging();
        }

        [TestMethod]
        public void Check() {
            RunFullCycle();
        }
    }
}
