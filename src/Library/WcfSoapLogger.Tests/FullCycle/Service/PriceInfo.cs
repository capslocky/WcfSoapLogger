using System;
using System.Runtime.Serialization;

namespace WcfSoapLogger.Tests.FullCycle.Service
{
    [DataContract(Namespace = XmlNamespaces.PriceService)]
    public class PriceInfo
    {
        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public DateTime LastUpdated { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string Notes { get; set; }
    }
}
