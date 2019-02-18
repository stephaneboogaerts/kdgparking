using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net.Mail;

namespace kdgparking.BL.Domain
{
    public class Badge
    {
        [Key]
        public int BadgeId { get; set; }
    }
}
