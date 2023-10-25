namespace RSinfoRetriever.Models.DPS;
public class Config {
    public string channel { get; set; } = null!;
    public string applicationName { get; set; } = null!;
    public string serviceName { get; set; } = null!;
    public string lang { get; set; } = null!;
    public int action { get; set; }
    public string ipAddress { get; set; } = null!;
}