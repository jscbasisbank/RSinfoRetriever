
using Refit;
using RSinfoRetriever.Endpoints;
using RSinfoRetriever.Models.RS;

namespace RSinfoRetriever;
public static class RsOperations {

    public async static Task<long?> GetRsPayerInfo(IRsPayerClient client, PayerInfoRequest request) {

        long? resultStatus = null;
        Console.WriteLine($"Retrieving Rs info for: {request.personalId}");
        try {
            var result = await client
                  .GetPayerInfo(
                  1
                  , Guid.NewGuid().ToString()
                  , request
                  );

            Console.WriteLine($"Successfully retrieved");
        }
        catch (ApiException ex) {

            var apiEx = await ex.GetContentAsAsync<Dictionary<string, dynamic>>();
            dynamic statusCode = null; 
            var isSuccessful = apiEx?.TryGetValue("StatusCode", out statusCode);

            if (statusCode != null) {
                resultStatus = statusCode;
                if (statusCode != 200) {
                    Console.WriteLine($"{ex.Message} \n");
                }
            }
        }

        return resultStatus; 
    }
}
