using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kdgparking.BL.Domain;
using kdgparking.DAL.EF;

namespace kdgparking.DAL
{
    public class Repository : IRepository
    {
        private OurDbContext ctx;

        public Repository()
        {
            ctx = new OurDbContext();
            ctx.Database.Initialize(true);
        }
        

    public Person CreatePersoon(Person persoon)
        {
            throw new NotImplementedException();
        }

        public Person ReadPersoon(int persoonNumber)
        {
            throw new NotImplementedException();
        }
    }
}
