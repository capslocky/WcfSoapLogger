using System.ServiceModel;

namespace WcfSoapLogger.Tests.FullCycle.Service
{
    [ServiceContract(Namespace = XmlNamespaces.PriceService)]
    public interface IPriceService
    {
        [OperationContract]
        PriceInfo GetPriceInfo(int itemId);

        [OperationContract]
        void SendPriceInfo(PriceInfo priceInfo);
    }


    public static class XmlNamespaces
    {
        public const string PriceService = "http://wcf-soap-logger.org/price-service";
    }
}
