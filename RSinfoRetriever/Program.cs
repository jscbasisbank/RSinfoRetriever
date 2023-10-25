#define DEBUG

using Refit;
using Microsoft.Extensions.Configuration;
using RSinfoRetriever.Endpoints;
using RSinfoRetriever.Clients;
using System.Text.RegularExpressions;
using RSinfoRetriever.Models;
using RSinfoRetriever.Models.DPS;
using Newtonsoft.Json;

namespace RSinfoRetriever;
public class EntryPoint {
    public static void Main(string[] args) {

        var config = SetupConfig();

        DbOperations dbclient = new(
            config.GetConnectionString("B2000") 
             ??  throw new Exception("should not be null")
            , config.GetConnectionString("BasisCRM") 
             ?? throw new Exception("should not be null") 
            );

        while (true) {
            try {
                ProgramLoop(config, dbclient);

                Console.Write($"\nType 'r' to restart the process; enter to exit: ");
                string? continueProcess = Console.ReadLine();

                if (continueProcess == null) {
                    break;
                }
                if(continueProcess != "r") {
                    break;
                }
            }
            catch (BulkIdException bidex) {
                Console.WriteLine(bidex.Message);
            }
            catch (Exception ex) {
                throw;
            }
        }
    }

    private static void ProgramLoop(IConfiguration config, DbOperations dbclient) {

        int bulkId = GetBulkId();
        var clients = BuildClients(dbclient, bulkId);

        ApiCallsEntry(clients, config, dbclient).GetAwaiter().GetResult();
    }

    private static int TryConvertBulkId(string? bulkId) {
        Regex bulkRex = new Regex("^[0-9]+$");

        if(bulkId is null) {
           throw new BulkIdException("BULK_ID can not be empty");
        }

        if (!bulkRex.IsMatch(bulkId)) {
            throw new BulkIdException("Invalid format for bulkId");
        }

        return Int32.Parse(bulkId);
        
    }

    private static int GetBulkId() {
        Console.Write("Enter BULK_ID for data retrieval: ");
        string? bulkId = Console.ReadLine();

        int bulkIdInt = TryConvertBulkId(bulkId);
        Console.WriteLine($"Retrieving CLIENTS with bulkId: {bulkId}\n");

        return bulkIdInt;
    }

    private static IConfiguration SetupConfig() {
         return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    //.AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
                    //.AddEnvironmentVaraiable()
                    .Build();
    }

    private static List<Client> BuildClients(
        DbOperations dbclient
        , int bulkId) {

        var bulkClients = dbclient.GetBulkClinetsForRS(bulkId);
        List<Client> clients = new();

        Console.WriteLine("Retrieving data for clients from database, please wait...\n");
        foreach (var bulkClient in bulkClients) {

            Client? client = dbclient.GetClientFromBank2000(bulkClient.CLIENT_NO);
            clients.Add(client);
        }
        Console.WriteLine($"Finished retrieving client data from DB; Retrieved: {clients.Count} clients; \n");

        return clients;
    }

    private async static Task ApiCallsEntry(
        IEnumerable<Client> clients
        , IConfiguration config
        , DbOperations dbclient 
        ) {

        int totalClientsCount = clients.Count();
        int successfulCount = 0;
        int unsuccessfulCount = 0;

        var dpsServicePath = config["externalServices:dps_consent"];
        var dpsConsentClient = RestService.For<IDpsConsentClient>(dpsServicePath ??
            throw new ArgumentException("Path to dps host should not be null"));

        Config dpsConfig = new Config() {
            action = Int32.Parse(config["appSpecific:action"] ?? throw new ArgumentException())
            , applicationName = config["appSpecific:applicationName"] ?? throw new ArgumentException()
            , channel = config["appSpecific:channel"] ?? throw new ArgumentException()
            , ipAddress = config["appSpecific:ipAddress"] ?? throw new ArgumentException()
            , lang = config["appSpecific:lang"] ?? throw new ArgumentException()
            , serviceName = config["appSpecific:serviceName"] ?? throw new ArgumentException()
        }; 
        var dpsGuid = Guid.NewGuid().ToString();
        // es rato ar mushaobs???
        /*JsonConvert.DeserializeObject<Config>(config["appSpecific"] 
            ?? throw new ArgumentException("app specific config should not be null"))
        ?? throw new ArgumentException("could not deserialize appSpecifig config into DPSConfig"); 
        */

        var rsServicePath = config["externalServices:rs"];
        var rsPayerClient = RestService.For<IRsPayerClient>(rsServicePath ?? 
            throw new ArgumentException("Path to rs host should not be null"));
        var rsGuid = Guid.NewGuid().ToString();

        string consentId = String.Empty;
        string batchId = String.Empty;
        foreach (var client in clients) {
            try {

#if DEBUG
                client.SMS_MOBILE_PHONE = "999999999";
#endif

                Regex rexPhone = new Regex(@"^\d{9}$");
                Regex rexPersonalId = new Regex(@"^\d{11}$");

                if(client is null) {
                    unsuccessfulCount++;   
                    continue;
                }

                if(client.PERSONAL_ID is null) {
                    unsuccessfulCount++;   
                    dbclient.UpdateClientStatus(client.CLIENT_NO, 3, "კლიენტი ვერ მოეძებნა");
                    continue;
                }


                if(client.SMS_MOBILE_PHONE is null) {
                    unsuccessfulCount++;   
                    dbclient.UpdateClientStatus(client.CLIENT_NO, 3, "კლიენტს ვერ მოეძებნა მობილურის ნომერი");
                    continue;
                }

                if(!rexPhone.IsMatch(client.SMS_MOBILE_PHONE!)) {
                    unsuccessfulCount++;   
                    dbclient.UpdateClientStatus(client.CLIENT_NO, 3, "ცუდი ფორმატის მობილურის ნომერი");
                    continue;
                } else if(!rexPersonalId.IsMatch(client.PERSONAL_ID)) {
                    unsuccessfulCount++;   
                    dbclient.UpdateClientStatus(client.CLIENT_NO, 3, "ცუდი ფორმატის პირადობის ნომერი");
                    continue;
                }

                var dpsSpResponse = DpsOperations.GetDpsConsentId(
                    dpsConsentClient
                    , Mappers.ClientToDpsSp(client, dpsConfig)
                    , dpsGuid
                    );

                var consent = DpsOperations.ApproveConsent(
                    dpsConsentClient
                    , new Models.DPS.ApproveRequest { channel = dpsConfig.channel, ipAddress = dpsConfig.ipAddress }
                    , dpsSpResponse
                    , dpsGuid
                    );

                consentId = consent;
                batchId = rsGuid;
                var rsStatus = await RsOperations.GetRsPayerInfo(
                   rsPayerClient
                   , Mappers.ClientToRs(client, consent, rsGuid)
                   );

                if(rsStatus is not null ) {
                    unsuccessfulCount++;
                    dbclient.UpdateClientStatus(client.CLIENT_NO, 2, $"rs შეცდომა: {rsStatus}");
                    continue;
                }
                
                successfulCount++;
                Console.WriteLine(
                    $"Total: {totalClientsCount}; \t" +
                    $" Successful: {successfulCount}; \t" +
                    $" Unsuccessful: {unsuccessfulCount}; \t" +
                    $" Remaining: {totalClientsCount - (successfulCount + unsuccessfulCount)} \n");
                Console.WriteLine("---------------------------------------");

                dbclient.UpdateClientStatus(client.CLIENT_NO, 1, "წარმატებით დასრულდა");

            } catch(Exception ex) {
                unsuccessfulCount++;
                dbclient.UpdateClientStatus(client.CLIENT_NO, 2, $"{ex.Message}");
                Console.WriteLine("---------------------------------------");
            }
        }

        Console.WriteLine($"Proccess finished; Successful RS calls: {successfulCount}/{totalClientsCount}");
    }

}