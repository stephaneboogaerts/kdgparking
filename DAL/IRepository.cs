using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.BL.Domain;

namespace kdgparking.DAL
{
    public interface IRepository
    {
        Person CreatePersoon(Person persoon);
        Person ReadPersoon(int persoonNumber);
    }
}
