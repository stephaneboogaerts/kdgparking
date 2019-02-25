using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL.Domain
{
    public class InputHolder
    {
        [Required(ErrorMessage = "Naam is vereist")]
        [DisplayName("Naam")]
        public string naam { get; set; }
        [Required(ErrorMessage = "Voornaam is vereist")]
        [DisplayName("Voornaam")]
        public string voornaam { get; set; }
        [Required(ErrorMessage = "Startdatum is vereist")]
        [DisplayName("Start Datum")]
        public DateTime startDate { get; set; }
        [Required(ErrorMessage = "Einddatum is vereist")]
        [DisplayName("Eind Datum")]
        public DateTime endDate { get; set; }
        [Required(ErrorMessage = "Company is vereist")]
        [DisplayName("Company")]
        public string company { get; set; }
        [DisplayName("Telefoon")]
        public string phone { get; set; }
        [DisplayName("GSM")]
        public string GSM { get; set; }
        [DisplayName("E-Mail")]
        public string email { get; set; }
        [DisplayName("Nummerplaat")]
        public string nummerplaat { get; set; }

        public InputHolder()
        {

        }
    }
}
