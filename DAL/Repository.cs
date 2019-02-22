using System.Data.Entity;
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

        public Holder ReadHolder(string holderId)
        {
            Holder holder = ctx.Holders.Find(holderId);
            return holder;
        }

        public IEnumerable<Holder> ReadHolders()
        {
            // Eager-loading
            IEnumerable<Holder> holders = ctx.Holders.ToList<Holder>();
            return holders;
        }

        public IEnumerable<Holder> ReadHoldersWithContractsAndVehicles()
        {
            // Eager-loading
            IEnumerable<Holder> holders = ctx.Holders.Include(h => h.Contracts).Include("Contracts.Vehicle").ToList<Holder>();
            return holders;
        }

        public Contract CreateContract(Contract contract)
        {
            ctx.Contracts.Add(contract);
            ctx.SaveChanges();

            return contract;
        }

        public Contract ReadContract(int contractNr)
        {
            Contract contract = ctx.Contracts.Find(contractNr);
            return contract;
        }

        public Vehicle CreateVehicle(Vehicle vehicle)
        {
            ctx.Vehicles.Add(vehicle);
            ctx.SaveChanges();

            return vehicle;
        }

        public Vehicle ReadVehicle(string numberplate)
        {
            Vehicle vehicle = ctx.Vehicles.Find(numberplate);
            return vehicle;
        }
    }
}
