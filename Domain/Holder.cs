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
        public string HolderNumber { get; set; }
        [Required]
        [RegularExpression(@"^\w+$")] // <-- nog te testen
        [StringLength(40, ErrorMessage = "Name must be less than 40 characters")]
        public string Name { get; set; }
        [RegularExpression(@"^\w+$")] // <-- nog te testen
        [StringLength(40, ErrorMessage = "Name must be less than 40 characters")]
        public string FirstName { get; set; }
        public int Phone { get; set; }
        public int GSM { get; set; }
        public string Email { get; set; }

        // Badge geschiedenis, of is badge uniek per holder?
        public Badge Badge { get; set; }
        public Address Address { get; set; }
        public List<Contract> Contracts { get; set; }
        //public Vehicle Vehicle { get; set; } <-- verhuist naar Contract

        /**
        [Key]
        public int PId { get; set; }
        [Required]
        [DisplayName("Naam")]
        public string naam { get; set; }
        [Required]
        [DisplayName("Voornaam")]
        public string voornaam { get; set; }
        [Required]
        [DisplayName("Start Datum")]
        public DateTime startDate { get; set; }
        [Required]
        [DisplayName("Eind Datum")]
        public DateTime endDate { get; set; }
        [Required]
        [DisplayName("Company")]
        public string company { get; set; }
        public MailAddress email { get; set; }
        public string nummerplaat { get; set; }
        public int PersoonNumber { get; set; }

        public Holder(string naam, string voornaam, DateTime startDate, DateTime endDate, string company, MailAddress email, string nummerplaat)
        {
            this.naam = naam ?? throw new ArgumentNullException(nameof(naam));
            this.voornaam = voornaam ?? throw new ArgumentNullException(nameof(voornaam));
            this.startDate = startDate;
            this.endDate = endDate;
            this.company = company ?? throw new ArgumentNullException(nameof(company));
            this.email = email;
            this.nummerplaat = nummerplaat;
        }
        **/
    }
}
