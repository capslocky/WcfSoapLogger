﻿using System.Runtime.Serialization;
using System.ServiceModel;

namespace Service
{
    [ServiceContract]
    public interface IDatabase
    {
        [OperationContract]
        JuiceInfo[] FindSimilar(JuiceInfo juice);
    }

    [DataContract]
    public class JuiceInfo
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string Location { get; set; }
    }
}
