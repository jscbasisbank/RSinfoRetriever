
using RSinfoRetriever.Models;
using RSinfoRetriever.Models.DPS;
using RSinfoRetriever.Models.RS;

namespace RSinfoRetriever;
public class Mappers {
    public static StartProcessRequest ClientToDpsSp(Client client, Config config) {
        return new StartProcessRequest {
            channel = config.channel 
            , applicationName = config.applicationName 
            , serviceName = config.serviceName
            , lang = config.lang 
            , personalId = client.PERSONAL_ID
            , action = config.action 
            , ipAddress = config.ipAddress 
            , date = DateTime.Now.ToString("yyyy-MM-d") 
            , extraData = new ExtraData[] {
                new ExtraData {
                    lang = "GEO"
                    , data = new Data[] {
                    }
                }
            },
        };
    }
    
    public static PayerInfoRequest ClientToRs(Client client, string consentId, string batchId) {
        return new PayerInfoRequest {
            userId = 1192
            , personalId = client.PERSONAL_ID
            , mobile = client.SMS_MOBILE_PHONE!
            , email = client.E_MAIL 
            , channel = "bulk" 
            , consentId = consentId 
            , batchId = batchId
            , msgId = Guid.NewGuid().ToString()
        };
    }
}
