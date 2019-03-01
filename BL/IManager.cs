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
        Holder UpdateHolder(int id, InputHolder updatedHolder);
        IEnumerable<Holder> GetHolders();
        Holder GetHolder(int id);
        Holder AddHolder(string name);
        Holder AddHolder(string name, string firstName, string phone, string email);
        Holder AddNewHolder(InputHolder holder);
        //Contract
        Contract AddContract(int holderId, string numberplate, DateTime begin, DateTime end, decimal tarif, decimal warranty, decimal warrantyBadge);
        //Vehicle
        Vehicle GetVehicle(string numberplate);

        //File
        List<InputHolder> ProcessFile(HttpPostedFileBase file);        
        //Holder AddHolder(string id, string name, string firstName, PhoneAttribute phone, MailAddress email);
    }
}
