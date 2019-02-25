using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net.Mail;

namespace kdgparking.BL.Domain
{
    public class Holder
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^\w+$")] // <-- nog te testen
        [StringLength(40, ErrorMessage = "Name must be less than 40 characters")]
        public string Name { get; set; }
        [RegularExpression(@"^\w+$")] // <-- nog te testen
        [StringLength(40, ErrorMessage = "Name must be less than 40 characters")]
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string GSM { get; set; }
        public string Email { get; set; }

        // Badge geschiedenis, of is badge uniek per holder?
        public Badge Badge { get; set; }
        public Address Address { get; set; }
        public List<Contract> Contracts { get; set; }
        //public Vehicle Vehicle { get; set; } <-- verhuist naar Contract

    }
}
