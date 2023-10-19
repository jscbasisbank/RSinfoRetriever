namespace RSinfoRetriever.Models.DPS;
public class StartProcessResponse {
    public string consentId { get; set; } = null!;
    public string consentText { get; set; } = null!;
    public string personalId { get; set; } = null!;
    public int status { get; set; }
    public string expireDate { get; set; } = null!;
    public string insertDate { get; set; } = null!;
    public bool isValid { get; set; }
    public bool createNewConsent { get; set; }
}
