using System;
using System.Collections.Generic;
using System.Data.Entity;

using kdgparking.BL.Domain;

namespace kdgparking.DAL.EF
{
    internal class OurDbInitializer
        //: DropCreateDatabaseAlways<OurDbContext>
    : CreateDatabaseIfNotExists<OurDbContext>  // <-- Uit commentaar bij deployment
    {
        protected override void Seed(OurDbContext context)
        {
            base.Seed(context);
        }
    }
}
