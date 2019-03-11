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
        public int Id { get; set; }
        public string ContractId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Wanneer een termijn afloopt of wordt opgezegd
        public bool Archived { get; set; }
        
        public Holder Holder { get; set; }
        public Badge Badge { get; set; }
    }
}
