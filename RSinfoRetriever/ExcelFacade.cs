using IronXL;
using RSinfoRetriever.Models;

namespace RSinfoRetriever;
public class ExcelFacade {

    private readonly WorkBook workBook;
    private readonly WorkSheet workSheet;
    public ExcelFacade(string fileName) {
        workBook = new WorkBook(fileName);
        workSheet = workBook.WorkSheets.First();
    }

    public IEnumerable<ExcelClient> GetPayers() {
        List<ExcelClient> clients = new List<ExcelClient>();

        string personalIdCell = string.Empty;
        string mobileCell = string.Empty;
        string emailCell = string.Empty;

        foreach (var cell in workSheet["A2:A10328"]) {

            personalIdCell = $"B{cell.RowIndex + 1}";
            mobileCell = $"C{cell.RowIndex + 1}";
            emailCell = $"D{cell.RowIndex + 1}";


            Console.WriteLine($"{cell.Text}, ");
            Console.WriteLine(workSheet[personalIdCell].Value.ToString());

            clients.Add(new ExcelClient
            {
                CLIENT_ID = Int32.Parse(cell.Text)
                //bazidan aris wamosagebi
                , PERSONAL_ID = CorrectPersonalId(workSheet[personalIdCell].Value.ToString()!)
                , SMS_MOBILE_PHONE = workSheet[mobileCell].Value.ToString()
                , EMAIL = workSheet[emailCell].Value.ToString()
            }); ;

        }
        return clients;
    } 
}
