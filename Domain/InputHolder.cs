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
        [DisplayName("Start Datum")]
        public DateTime startDate { get; set; }
        [DisplayName("Eind Datum")]
        public DateTime endDate { get; set; }
        [Required(ErrorMessage = "Company is vereist")]
        [DisplayName("Company")]
        public string company { get; set; }
        public string email { get; set; }
        public string nummerplaat { get; set; }
        public int PersoonNumber { get; set; }

        // Toegevoegd op 24 feb 2019
        public int Badge { get; set; }
        public string PNumber { get; set; } // <-- string voor excel
        public string ContractId { get; set; }
        public string VoertuigNaam { get; set; }
        public decimal Tarief { get; set; }
        public int BeginDatum { get; set; } // <-- excel geeft geen datetime mee 
        public int EindDatum { get; set; }
        public decimal Waarborg { get; set; }

        public InputHolder()
        {

        }
    }
}
