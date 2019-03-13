using kdgparking.BL.Domain;
using System;
using System.ComponentModel;

namespace kdgparking.Models
{
    public class ContractModel
    {
        public int HolderId { get; set; }
        public string BadgeId { get; set; }
        [DisplayName("Naam")]
        public string Name { get; set; }
        [DisplayName("Voornaam")]
        public string FirstName { get; set; }
        public string Email { get; set; }
        [DisplayName("Actief")]
        public int Active { get; set; }
        [DisplayName("Start Datum")]
        public DateTime StartDate { get; set; }
        [DisplayName("Eind Datum")]
        public DateTime EndDate { get; set; }
        public string Company { get; set; }
        public BadgeStatus BadgeStatus { get; set; }
    }
}