using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace kdgparking.BL.Domain
{
    public class Holder
    {
        [Key]
        public string HolderNumber { get; set; }
        [RegularExpression(@"^\w+$")] // <-- nog te testen
        [StringLength(40, ErrorMessage = "Name must be less than 40 characters")]
        public string Name { get; set; }
        [RegularExpression(@"^\w+$")] // <-- nog te testen
        [StringLength(40, ErrorMessage = "Name must be less than 40 characters")]
        public string FirstName { get; set; }
        public PhoneAttribute Phone { get; set; }
        public EmailAddressAttribute Email { get; set; }

        // Badge geschiedenis, of is badge uniek per holder?
        public Badge Badge { get; set; }
        public Vehicle Vehicle { get; set; }
        public Address Address { get; set; }
    }
}
