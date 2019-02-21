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
        // test functies
        Holder GetHolders();
        Holder AddHolder(string id, string name, string firstName, int phone, string email);
        Contract AddContract(string holderId, DateTime begin, DateTime einde, decimal tarif, decimal warranty, decimal warrantyBadge);

        //Holder AddHolder(string id, string name, string firstName, PhoneAttribute phone, MailAddress email);
    }
}
