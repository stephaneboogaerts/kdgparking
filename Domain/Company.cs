using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL.Domain
{
    public class Company
    {
        //Company?
        [Key]
        public int CompanynId { get; set; }
        public string CompanyName { get; set; }
    }
}
