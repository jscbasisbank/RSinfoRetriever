using Refit;
using RSinfoRetriever.Models.RS;

namespace RSinfoRetriever.Endpoints;
internal interface IRsPayerClient {

    [Post("/api/v{version}/Rs/PayerInfo")]
    public Task<PayerInfoResponse> GetPayerInfo(
        int version
        ,[Header("BSB-DIAGNOSTICS-GROUPID")] string diagGrpId
        ,[Body] PayerInfoRequest request
        );
}
