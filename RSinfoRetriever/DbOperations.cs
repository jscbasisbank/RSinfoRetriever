namespace RSinfoRetriever;

using Dapper;
using Microsoft.Data.SqlClient;
using RSinfoRetriever.Models;

public class DbOperations {

    private string _b2000ConnectionString;
    private string _basisCRMConnectionString;
    public DbOperations(string b2000ConnectionString, string basisCRMConnectionString) {

        _b2000ConnectionString = b2000ConnectionString;
        _basisCRMConnectionString = basisCRMConnectionString;

    }

    public Client GetClientFromBank2000(int CLIENT_NO) {

        using (SqlConnection con = new SqlConnection(_b2000ConnectionString)) {

            string query = "SELECT CLIENT_NO, PERSONAL_ID, SMS_MOBILE_PHONE, E_MAIL " +
                " FROM dbo.CLIENTS" +
                " WHERE CLIENT_NO = @CLIENT_NO"; 

            Client client = con.Query<Client>(query, new { CLIENT_NO = CLIENT_NO }).FirstOrDefault()!;

            if(client == null) {
                return new Client { CLIENT_NO = CLIENT_NO };
            }

            return client;
        }
    }

    public IEnumerable<BulkClient> GetBulkClinetsForRS(int bulkId) {

        using (SqlConnection con = new SqlConnection(_basisCRMConnectionString)) {

            string query = "SELECT *" +
                " FROM rs.BulkClients" +
                " WHERE BULK_ID = @BULK_ID" +
                " AND (STATUS = 0 OR STATUS = 2)";

            IEnumerable<BulkClient> clients = con.Query<BulkClient>(query, new { BULK_ID = bulkId }).ToList();

            return clients;
        }

    }

    public void UpdateClientStatus(int clientNo, int status, string description) {

        using (SqlConnection con = new SqlConnection(_basisCRMConnectionString)) {

            string query = "UPDATE rs.BulkClients " +
                "SET [STATUS] = @STATUS, [DESCRIPTION] = @DESCRIPTION, UPDATE_DT = @UPDATE_DT " +
                "WHERE CLIENT_NO = @CLIENT_NO";

            var result = con.Execute(query, new { CLIENT_NO = clientNo, STATUS = status, DESCRIPTION = description, UPDATE_DT = DateTime.Now });
        }
    }
}