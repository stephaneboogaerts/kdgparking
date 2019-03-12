using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure.Annotations;

using kdgparking.BL.Domain;

namespace kdgparking.DAL.EF
{
    [DbConfigurationType(typeof(OurDbConfiguration))]
    public class OurDbContext : DbContext
    {
        public OurDbContext() : base("kdgparking")
        {
            Database.SetInitializer<OurDbContext>(new CreateDatabaseIfNotExists/*DropCreateDatabaseAlways*/<OurDbContext>());
        }

        public DbSet<Holder> Holders { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Company> Companies { get; set; }

        //public DbSet<Address> Addresses { get; set; }
        //public DbSet<BadgeHistory> BadgeHistories { get; set; }
        //public DbSet<ContractHistory> ContractHistories { get; set; }
    }
}
