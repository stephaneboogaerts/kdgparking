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
        [Required]
        public string ContractId { get; set; }
        public decimal Tarif { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Warranty { get; set; }
        public decimal WarrantyBadge { get; set; }
        //public bool Actief { get; set; } // Deze wordt in de DB view realtime weergegeven

        public Holder Holder { get; set; }
        //1 voertuig per contract? -> Onwaarschijnlijk, badge kan doorgegeven worden. LPR herkent enkel de nummerplaat, geen invloed op de toegang
        // update: gewijzigd naar List
        public List<Vehicle> Vehicles { get; set; }
    }
}
