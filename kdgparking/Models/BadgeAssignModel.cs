using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kdgparking.Models
{
    public class BadgeAssignModel
    {
        public int HolderId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string MifareSerial { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string NewMifareSerial { get; set; }
    }
}