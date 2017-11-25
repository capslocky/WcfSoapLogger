using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Service
{
    [ServiceContract]
    public interface IWeatherService
    {
        [OperationContract]
        long SendReport(WeatherReport report);

        [OperationContract]
        WeatherReport GetLastReportByLocation(string location);
    }


    [DataContract]
    public class WeatherReport
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public double Temperature { get; set; }

        [DataMember]
        public double Humidity { get; set; }

        [DataMember]
        public double Pressure { get; set; }

        [DataMember]
        public double WindSpeed { get; set; }

        [DataMember]
        public double WindDirection { get; set; }
    }

}
