using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using kdgparking.BL.Domain;

namespace kdgparking.BL
{
    public interface IManager
    {
        // Holder
        IEnumerable<Holder> GetHolders();
        Holder GetHolder(string id);
        Holder AddHolder(string id, string name, string firstName, int phone, string email);
        //Contract
        Contract AddContract(string holderId, string numberplate, DateTime begin, DateTime end, decimal tarif, decimal warranty, decimal warrantyBadge);
        //Vehicle
        Vehicle GetVehicle(string numberplate);

        //File
        void ProcessFile(HttpPostedFileBase file); // <-- zal later nog iets teruggeven aan controller
        //void ProcessFileData(string fileData);
        
        //Holder AddHolder(string id, string name, string firstName, PhoneAttribute phone, MailAddress email);
    }
}
