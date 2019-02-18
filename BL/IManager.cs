using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.BL.Domain;

namespace kdgparking.BL
{
    public interface IManager
    {
        // test fucnties
        Person GetPersonen();
        Person AddPersoon(Person person);
    }
}
