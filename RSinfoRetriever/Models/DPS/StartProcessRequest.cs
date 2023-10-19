
namespace RSinfoRetriever.Models.DPS {
    public class StartProcessRequest {
        public string channel { get; set; } = null!;
        public string applicationName { get; set; } = null!;
        public string serviceName { get; set; } = null!;
        public string lang { get; set; } = null!;
        public string personalId { get; set; } = null!;
        public int action { get; set; } 
        public string ipAddress { get; set; } = null!;
        public string date { get; set; } = null!;
        public ExtraData[] extraData { get; set; } = null!;
    }

    public class ExtraData {
        public string lang { get; set; } = null!;
        public Data[] data { get; set; } = null!;

    }  

    public class Data {
        public string key { get; set; }
        public string value { get; set; }
    }
}
