using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace kdgparking.DAL.EF
{
    internal class OurDbConfiguration : DbConfiguration
    {
        public OurDbConfiguration()
        {
            this.SetDefaultConnectionFactory(new SqlConnectionFactory()); // SQL Server instantie op machine
            this.SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
            this.SetDatabaseInitializer(new OurDbInitializer());
        }
    }
}
