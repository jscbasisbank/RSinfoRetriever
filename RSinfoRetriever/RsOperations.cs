
using RSinfoRetriever.Endpoints;
using RSinfoRetriever.Models.RS;

namespace RSinfoRetriever;
public static class RsOperations {

    public static void GetRsPayerInfo(IRsPayerClient client, PayerInfoRequest request) {
        Console.WriteLine($"Retrieving Rs info for: {client}");
        try {
            var result = client
                  .GetPayerInfo(
                  1
                  , Guid.NewGuid().ToString()
                  , request 
                  )
                  .GetAwaiter()
                  .GetResult();


            Console.WriteLine($"This must be a value: {result}");
        }
        catch (Exception ex) {
            Console.WriteLine($"Could not retrieve info from RS; Reason: {ex.Message}");
            throw;
        }
    }
}
