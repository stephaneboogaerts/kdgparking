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
        }
    }
}
