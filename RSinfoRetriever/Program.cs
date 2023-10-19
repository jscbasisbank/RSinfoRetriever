#define DEBUG
using Refit;
using Microsoft.Extensions.Configuration;
using RSinfoRetriever.Endpoints;
using RSinfoRetriever.Models.RS;
using RSinfoRetriever.Clients;
using RSinfoRetriever.Models.DPS;

namespace RSinfoRetriever;
public class EntryPoint {
    public static void Main(string[] args) {

        var config = SetupConfig();
        var rsServicePath = config["externalServices:rs_dev"];
        var rsPayerClient = RestService.For<IRsPayerClient>(rsServicePath ?? 
            throw new ArgumentException("Path to rs host should not be null"));

        var dpsServicePath = config["externalServices:dps_consent_dev"];
        var dpsConsentClient = RestService.For<IDpsConsentClient>(dpsServicePath ?? 
            throw new ArgumentException("Path to dps host should not be null"));

        RsOperations.GetRsPayerInfo(rsPayerClient);

    }

    private static IConfiguration SetupConfig() {
         return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    //.AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
                    //.AddEnvironmentVaraiable()
                    .Build();
    }

}