using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace kdgparking.Models
{
    public class HolderVehicle
    {
        [DisplayName("Naam")]
        public string Name { get; set; }
        [DisplayName("Voornaam")]
        public string FirstName { get; set; }
        [DisplayName("Telefoon")]
        public string Phone { get; set; }
        public string GSM { get; set; }
        public string Email { get; set; }

        [DisplayName("Model")]
        public string VehicleName { get; set; }
        [DisplayName("Nummerplaat")]
        public string Numberplate { get; set; }
    }
}