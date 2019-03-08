using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kdgparking.Models
{
    public class ContractModel
    {
        public string BadgeId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int Active { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Company { get; set; }
    }
}