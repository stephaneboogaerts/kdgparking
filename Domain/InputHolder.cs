using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace kdgparking.BL.Domain
{
    public class InputHolder
    {
        public int HolderId { get; set; }
        public enum EmployeeType {
            Student,
            Personeel,
            Andere }

        [Required(ErrorMessage = "Naam is vereist"), DisplayName("Naam"), StringLength(40, ErrorMessage = "Naam mag niet langer dan 40 karakters zijn")]
        [RegularExpression(@"^\w+$", ErrorMessage ="Naam bevat incorrecte tekens")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Voornaam is vereist"), DisplayName("Voornaam"), StringLength(40, ErrorMessage = "Voornaam mag niet langer dan 40 karakters zijn")]
        [RegularExpression(@"^\w+$", ErrorMessage ="Voornaam bevat incorrecte tekens")]
        public string FirstName { get; set; }
        [DisplayName("Start Datum")]
        public DateTime StartDate { get; set; }
        [DisplayName("Eind Datum")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Company is vereist")]
        public string Company { get; set; }

        [DisplayName("Nummerplaat")]
        public string NumberPlate { get; set; }
        public string VoertuigNaam { get; set; }

        public int MiFareSerial { get; set; }

        public string Email { get; set; }
        public string Telefoon { get; set; }
        public string GSM { get; set; }
        
        public string Badge { get; set; } // <-- MifareSerial id
        public string PNumber { get; set; } // <-- string voor excel
        public string ContractId { get; set; }
        public decimal Tarief { get; set; }
        public decimal Waarborg { get; set; }
        public decimal WaarborgBadge { get; set; }
        public string Straat { get; set; }
        public string Post { get; set; }
        public string Stad { get; set; }
    }
}
