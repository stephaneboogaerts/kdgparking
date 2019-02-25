using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using kdgparking.BL.Domain;

namespace kdgparking.BL
{
    public interface IManager
    {
        // Holder
        IEnumerable<Holder> GetHolders();
        Holder GetHolder(int id);
        Holder AddHolder(string name, string firstName, string phone, string email);
        void AddNewHolder(InputHolder inputHolder); //Omzetting Input naar Domein objecten
        //Contract
        Contract AddContract(int holderId, string numberplate, DateTime begin, DateTime end, decimal tarif, decimal warranty, decimal warrantyBadge);
        //Vehicle
        Vehicle GetVehicle(string numberplate);

        //Holder AddHolder(string id, string name, string firstName, PhoneAttribute phone, MailAddress email);
    }
}
