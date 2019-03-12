using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace kdgparking.BL.Domain
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }
        public string ContractId { get; set; }
        [DisplayName("Start Datum")]
        public DateTime StartDate { get; set; }
        [DisplayName("Eind Datum")]
        public DateTime EndDate { get; set; }

        // Wanneer een termijn afloopt of wordt opgezegd
        public bool Archived { get; set; }
        
        public Holder Holder { get; set; }
        public Badge Badge { get; set; }
    }
}
