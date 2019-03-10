﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations.Schema;

namespace kdgparking.BL.Domain
{
    public class Holder
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Persoon Nummer")]
        public string HolderNumber { get; set; }
        [Required]
        [DisplayName("Achternaam")]
        public string Name { get; set; }
        [DisplayName("Voornaam")]
        public string FirstName { get; set; }
        [DisplayName("Telefoon")]
        public string Phone { get; set; }
        public string GSM { get; set; }
        public string Email { get; set; }

        // Address naar hier verhuist : 1 op 1 relatie
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        // Unieke identifiers
        [StringLength(50, ErrorMessage = "The SamAccountName value cannot exceed 50 characters. ")]
        public string SamAccountName { get; set; } // <-- wordt toegewezen in repo bij het aanmaken
        [StringLength(20, ErrorMessage = "The MifareSerial value cannot exceed 20 characters. ")]
        public string MifareSerial { get; set; } // <-- wordt toegewezen in repo bij het aanmaken
        
        public Company Company { get; set; }
        public List<Contract> Contracts { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
}
