using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL.Domain
{
    public class ContractHistory
    {
        [Key]
        public string ContractId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Warranty { get; set; }
        public decimal WarrantyBadge { get; set; }
        //public decimal Tarif { get; set; } // <--  wordt niet meer naar db geschreven

        public Holder Holder { get; set; }
    }
}
