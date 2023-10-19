
using RSinfoRetriever.Models;
using RSinfoRetriever.Models.DPS;
using RSinfoRetriever.Models.RS;

namespace RSinfoRetriever;
public class Mappers {
    public static StartProcessRequest ExcelToDpsSp(ExcelClient client) {
        return new StartProcessRequest {
            channel = "bulk"
            , applicationName = "rs"
            , serviceName = "console-app"
            , lang = "GEO"
            , personalId = client.PERSONAL_ID
            , action = 2
            , ipAddress = "127.0.0.1"
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
    
    public static PayerInfoRequest ExcelToRs(ExcelClient client, string consentId, string batchId) {
        return new PayerInfoRequest {
            userId = client.CLIENT_ID
            , personalId = client.PERSONAL_ID
            , mobile = client.SMS_MOBILE_PHONE
            , email = client.EMAIL 
            , channel = "bulk" 
            , consentId = consentId 
            , batchId = batchId
            , msgId = Guid.NewGuid().ToString()
        };
    }
}
