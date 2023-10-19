using Refit;
using RSinfoRetriever.Models.DPS;

namespace RSinfoRetriever.Clients {
    public interface IDpsConsentClient {

        [Post("/api/Consent/StartProcess")]
        public Task<StartProcessResponse> StartProcess(
            [Header("BSB-DIAGNOSTICS-GROUPID")] string guid
            , [Body] StartProcessRequest request
            );

        [Post("/api/Consent/{consentId}/approve")]
        public Task<ApproveResponse> Approve(
            string consentId
            , [Header("BSB-DIAGNOSTICS-GROUPID")] string guid
            , [Body] ApproveRequest request 
            );
    }
}
