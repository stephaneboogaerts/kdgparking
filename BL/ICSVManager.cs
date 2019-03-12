using kdgparking.BL.Domain;
using System.Collections.Generic;

namespace kdgparking.BL
{
    public interface ICSVManager
    {
        //File Upload
        List<InputHolder> ProcessInputholderList(List<InputHolder> inputHolderList);
        List<InputHolder> ProcessFile(System.Web.HttpPostedFileBase file);
        string CsvExport(IEnumerable<Vehicle> vehicles);
    }
}
