using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.BL.Domain;

namespace kdgparking.BL
{
    public interface IManager
    {
        // test fucnties
        Holder GetPersonen();
        Holder AddHolder(string id, string name, string firstName, PhoneAttribute phone, EmailAddressAttribute email);
    }
}
