
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
                        channel = "bulk"
                        , applicationName = "rs"
                        , serviceName = "console-app"
                        , lang = "GEO"
                        , personalId = "piradi nomeri"
                        , action = 2
                        , ipAddress = "127.0.0.1"
                        , date = "2023-10-19"
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

        Console.WriteLine("Approving consent: ");
        try {
            var response = client.Approve(
                consentId
                , guid 
                , new ApproveRequest
                {
                    channel = "bulk"
                    ,
                    ipAddress = "127.0.0.1"
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
