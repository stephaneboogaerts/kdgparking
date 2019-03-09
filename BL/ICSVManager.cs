using kdgparking.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
