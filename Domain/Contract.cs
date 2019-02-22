using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL.Domain
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }
        public decimal Tarif { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Warranty { get; set; }
        public decimal WarrantyBadge { get; set; }

        public Holder Holder { get; set; }
        //1 voertuig per contract?
        public Vehicle Vehicle { get; set; }
    }
}
