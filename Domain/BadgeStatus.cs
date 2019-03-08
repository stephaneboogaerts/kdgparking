using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL.Domain
{
    public enum BadgeStatus : byte
    {
        Active = 1,
        Lost,
        Disabled
    }
}
