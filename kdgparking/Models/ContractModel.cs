using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace kdgparking.Models
{
    public class ContractModel
    {
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
    }
}