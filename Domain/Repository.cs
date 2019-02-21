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
        

    public Holder CreateHolder(Holder holder)
        {
            ctx.Holders.Add(holder);
            ctx.SaveChanges();

            return holder;
        }

        public Holder ReadHolder(int persoonNumber)
        {
            Holder holder = ctx.Holders.Find(persoonNumber);
            return holder;
        }
    }
}
