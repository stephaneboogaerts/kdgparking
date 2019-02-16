using System;
using System.Collections.Generic;
using System.Data.Entity;

using kdgparking.BL.Domain;

namespace kdgparking.DAL.EF
{
    internal class OurDbInitializer
        : DropCreateDatabaseAlways<OurDbContext>
    {
        protected override void Seed(OurDbContext context)
        {
            base.Seed(context);
            //Persoon p1 = new Persoon()
            //{
            //    PersoonNumber = 111,
            //    Name = "Jansen Jansen"
            //};
            //context.Personen.Add(p1);
            //// Save the changes in the context (all added entities) to the database
            //context.SaveChanges();
        }
    }
}
