﻿namespace RSinfoRetriever.Models.RS;
public class PayerInfoRequest {
    public string? consentId { get; set; }
    public string personalId { get; set; } = null!;
    public string mobile { get; set; } = null!;
    public string channel { get; set; } = null!;
    public int userId { get; set; }
    public string? email { get; set; }
    public string msgId { get; set; } = null!;
    public string batchId { get; set; } = null!;
}

