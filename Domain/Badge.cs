using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace kdgparking.BL.Domain
{
    public class Badge
    {
        [Key]
        [StringLength(20, ErrorMessage = "The MifareSerial value cannot exceed 20 characters. ")]
        public string MifareSerial { get; set; }
        public BadgeStatus BadgeStatus { get; set; }
        
        List<Contract> Contracts { get; set; }
    }
}
