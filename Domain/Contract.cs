using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL.Domain
{
    public class Contract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ContractId { get; set; }
        public decimal Tarif { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Warranty { get; set; }
        public decimal WarrantyBadge { get; set; }
        
        [ForeignKey("Holder")]
        public int HolderId { get; set; }
        public Holder Holder { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
}
