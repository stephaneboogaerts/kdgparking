using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace kdgparking.BL.Domain
{
    public class Badge
    {
        [Key]
        public int Id { get; set; }
        public int BadgeId { get; set; }
        public BadgeStatus BadgeStatus { get; set; }
        
        List<Contract> Contracts { get; set; }
    }
}
