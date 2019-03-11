using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kdgparking.BL.Domain;
using kdgparking.DAL.EF;

namespace kdgparking.DAL
{
    public class HolderRepository : IHolderRepository
    {
        public OurDbContext ctx { get; }

        public HolderRepository()
        {
            ctx = new OurDbContext();
            ctx.Database.Initialize(false);
        }

        public HolderRepository(OurDbContext context)
        {
            ctx = context;
        }
        
        public Holder CreateHolder(Holder holder)
        {
            ctx.Holders.Add(holder);
            ctx.SaveChanges();

            // SamAccountName en MifareSerial unieke id toewijzen adhv Id, toegewezen door DB, en prefix.
            holder.SamAccountName = "SAM" + holder.Id;
            holder.MifareSerial = "MiS" + holder.Id;
            ctx.SaveChanges();            

            return holder;
        }

        public Holder ReadHolder(int holderId)
        {
            Holder holder = ctx.Holders.Include("Company").Include("Contract").FirstOrDefault(x => x.Id == holderId);
            return holder;
        }

        // Zoekt in db op PNumber (enkel Holders toegevoegd adhv excel hebben deze value)
        public Holder ReadHolder(string pNumber)
        {
            Holder holder = ctx.Holders.Include("Company").Include("Contract").FirstOrDefault(x => x.HolderNumber == pNumber);
            return holder;
        }

        public void UpdateHolder(Holder holder)
        {
            ctx.Entry(holder).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteHolder(Holder holder)
        {
            ctx.Holders.Remove(holder);
            ctx.SaveChanges();
        }

        public IEnumerable<Holder> ReadHolders()
        {
            // Eager-loading
            IEnumerable<Holder> holders = ctx.Holders.Include("Company").ToList<Holder>();
            return holders;
        }

        public IEnumerable<Holder> ReadHolders(string searchString)
        {
            // Zoekt op volledige naam
            IEnumerable<Holder> holders = ctx.Holders.Include("Company").Where(h => ((h.FirstName + " " + h.Name).ToLower()).Contains(searchString.ToLower())).ToList<Holder>();
            return holders;
        }

        public IEnumerable<Holder> ReadHoldersWithContractsAndVehicles()
        {
            IEnumerable<Holder> holders = ctx.Holders.Include("Contract").Include("Contract.Vehicles").Include("Company").ToList<Holder>();
            return holders;
        }

        public IEnumerable<Holder> ReadHoldersWithContractsAndVehicles(string company)
        {
            IEnumerable<Holder> holders = ctx.Holders.Where(h => h.Company.CompanyName.ToLower() == company.ToLower()).Include("Contract").Include("Contract.Vehicles").Include("Company").ToList<Holder>();
            return holders;
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
