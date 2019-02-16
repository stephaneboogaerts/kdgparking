using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace kdgparking.BL.Domain
{
    public class Person
    {
        [Key]
        public int PId { get; set; }
        public string Name { get; set; }
        public int PersoonNumber { get; set; }
    }
}
