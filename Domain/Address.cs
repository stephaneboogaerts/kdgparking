using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL.Domain
{
    public class Address
    {
        // Veel op veel relatie met Holder
        //  note : Bij wijziging zou address voor meerdere personen wijzigen -> 1 op 1 relatie
        [Key]
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

    }
}
