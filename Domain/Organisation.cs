using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL.Domain
{
    public class Organisation
    {
        [Key]
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
    }
}
