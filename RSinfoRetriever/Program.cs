using Refit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using RSinfoRetriever.Endpoints;
using RSinfoRetriever.Models.RS;

namespace rsir;
public class EntryPoint {
    public static void Main(string[] args) {

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
            //.AddEnvironmentVaraiable()
            .Build();

        var rsServicePath = config["externalServices:rs_dev"];
        var rsPayerClient = RestService.For<IRsPayerClient>(rsServicePath);

        var result = rsPayerClient
            .GetPayerInfo(
            1
            ,Guid.NewGuid().ToString()
            ,new PayerInfoRequest {
            }
            )
            .GetAwaiter()
            .GetResult();


        Console.WriteLine($"This must be a value: {result}");
    }

}