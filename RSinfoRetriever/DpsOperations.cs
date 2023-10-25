
using RSinfoRetriever.Clients;
using RSinfoRetriever.Models.DPS;

namespace RSinfoRetriever;
public static class DpsOperations {
    public static string GetDpsConsentId(
        IDpsConsentClient client
        , StartProcessRequest request
        , string guid) {

            Console.WriteLine($"Obtaining consent for: {request?.personalId}");
            try {
                var response = client.StartProcess(
                    guid
                    , new StartProcessRequest {
                        channel = request.channel
                        , applicationName = request.applicationName
                        , serviceName = request.serviceName
                        , lang = request.lang
                        , personalId = request.personalId 
                        , action =request.action 
                        , ipAddress = request.ipAddress 
                        , date = request.date 
                        , extraData = new ExtraData[] {
                            new ExtraData {
                                lang = "GEO"
                                , data = new Data[] {
                                }
                            }
                        },
                    }
                ).GetAwaiter().GetResult();

                 return response.consentId;
            }
            catch (Exception ex) {
                Console.WriteLine($"Could not obtaing consentId; Reason: {ex?.Message}");          
                throw;
            }
        }
    public static string ApproveConsent( IDpsConsentClient client
        , ApproveRequest request
        , string consentId
        , string guid
        ) {

        Console.WriteLine($"Approving consent: {consentId}");
        try {
            var response = client.Approve(
                consentId
                , guid 
                , new ApproveRequest
                {
                    channel = request.channel
                    ,
                    ipAddress = request.ipAddress 
                }).GetAwaiter().GetResult();
            Console.WriteLine($"Consent approved: {response.consentId}");
            return response.consentId;
        }
        catch (Exception ex) {
            Console.WriteLine($"Could not approve: {ex.Message}");
            throw;
        }
    }    
}
