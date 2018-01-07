using System;
using System.ServiceModel;

namespace WcfSoapLogger.Tests.FullCycle.Service
{
    [ServiceBehavior(Namespace = XmlNamespaces.PriceService)]
    public class PriceServiceKiaMotors : IPriceService
    {
        public PriceInfo GetPriceInfo(int itemId)
        {
            var priceInfo = new PriceInfo();

            priceInfo.ItemId = itemId;
            priceInfo.LastUpdated  = new DateTime(2018, 01, 07, 16, 54, 12, DateTimeKind.Utc);
            priceInfo.Price = 233.33m;
            priceInfo.Notes = "Part Number: 66311B2000";

            return priceInfo;
        }

        public void SendPriceInfo(PriceInfo priceInfo)
        {
            //empty method
        }
    }
}
