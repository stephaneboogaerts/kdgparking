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
        //Validatie verplaatst naar InputHolder
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        // Badge geschiedenis, of is badge uniek per holder? -> Hoogstwaarschijnlijk irrelevant
        public Badge Badge { get; set; }
        public Address Address { get; set; }
        public List<Contract> Contracts { get; set; }
    }
}
