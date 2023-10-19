namespace RSinfoRetriever.Models.RS;
public class PayerInfoResponse {
    public string period { get; set; } = null!;
    public string employer_Name { get; set; } = null!;
    public string employer_Tin { get; set; } = null!;
    public string income_Type { get; set; } = null!;
    public string taxable_Total_Income { get; set; } = null!;
    public string income { get; set; } = null!;
    public string first_Decl_Date { get; set; } = null!;
    public string dec_Type { get; set; } = null!;
    public string last_Decl_Date { get; set; } = null!;
    public string vat_Payed { get; set; } = null!;
    public string income_Tax_Payed { get; set; } = null!;
    public string small_Status { get; set; } = null!;
    public string small_Status_Sdate { get; set; } = null!;
    public string small_Status_Edate { get; set; } = null!;
    public string activity_Type { get; set; } = null!;
    public string small_Business_Revenue { get; set; } = null!;
    public string small_Business_Taxable_Revenue { get; set; } = null!;
    public string micro_Business_Revenue { get; set; } = null!;
    public string spec_Regime_Tax { get; set; } = null!;
}
