using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure.Annotations;

using kdgparking.BL.Domain;

namespace kdgparking.DAL.EF
{
    [DbConfigurationType(typeof(OurDbConfiguration))]
    internal class OurDbContext : DbContext /* 'public' for testing with project 'DAL-Testing'! */
    {
        public OurDbContext() : base("kdgparking")
        {
            Database.SetInitializer<OurDbContext>(new DropCreateDatabaseAlways<OurDbContext>());
        }

        public DbSet<Holder> Holders { get; set; }
        public DbSet<Vehicle> Voertuigen { get; set; }
    }
}
