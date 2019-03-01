﻿using System;
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
        public string Name { get; set; }
        [Required(ErrorMessage = "Voornaam is vereist")]
        [DisplayName("Voornaam")]
        public string FirstName { get; set; }
        [DisplayName("Start Datum")]
        public DateTime StartDate { get; set; }
        [DisplayName("Eind Datum")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Company is vereist")]
        [DisplayName("Company")]
        public string Company { get; set; }
        public string Email { get; set; }

        // Toegevoegd op 24 feb 2019
        public int Badge { get; set; }
        public string PNumber { get; set; } // <-- string voor excel
        public string ContractId { get; set; }
        public string VoertuigNaam { get; set; }
        public decimal Tarief { get; set; }
        public int BeginDatumSerial { get; set; } // <-- excel geeft geen datetime mee 
        public int EindDatumSerial { get; set; }
        public decimal Waarborg { get; set; }
        public decimal WaarborgBadge { get; set; }
        public string Straat { get; set; }
        public int Post { get; set; }
        public string Stad { get; set; }
        public string Tel { get; set; }
        public string GSM { get; set; }
        public string NumberPlate { get; set; }

        public InputHolder()
        {

        }
    }
}
