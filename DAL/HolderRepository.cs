using kdgparking.BL.Domain;
using kdgparking.DAL.EF;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            holder.SamAccountName = "_PW_" + holder.Company.CompanyName + "_" + holder.Id;
            holder.MifareSerial = holder.Id.ToString();
            ctx.SaveChanges();            

            return holder;
        }

        public Holder ReadHolder(int holderId)
        {
            Holder holder = ctx.Holders.Include("Company").Include("Contracts").FirstOrDefault(x => x.Id == holderId);
            return holder;
        }

        // Zoekt in db op PNumber (enkel Holders toegevoegd adhv excel hebben deze value)
        public Holder ReadHolder(string pNumber)
        {
            Holder holder = ctx.Holders.Include("Company").Include("Contracts").Include("Contracts.Badge").Include("Vehicles").FirstOrDefault(x => x.HolderNumber == pNumber);
            return holder;
        }

        public void UpdateHolder(Holder holder)
        {
            ctx.Entry(holder).State = EntityState.Modified;
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
            IEnumerable<Holder> holders = ctx.Holders.Include("Contracts").Include("Vehicles").Include("Company").ToList<Holder>();
            return holders;
        }

        public IEnumerable<Holder> ReadHoldersWithContractsAndVehicles(string company)
        {
            IEnumerable<Holder> holders = ctx.Holders.Where(h => h.Company.CompanyName.ToLower() == company.ToLower()).Include("Contracts").Include("Vehicles").Include("Company").ToList<Holder>();
            return holders;
        }
        
        public Holder ReadHolderWithBadges(int holderId)
        {
            return ctx.Holders.Include("Contracts").Include("Contracts.Badge").FirstOrDefault(h => h.Id == holderId);
        }

        public Vehicle CreateVehicle(Vehicle vehicle)
        {
            ctx.Vehicles.Add(vehicle);
            ctx.SaveChanges();

            return vehicle;
        }

        public Vehicle ReadVehicle(string numberplate)
        {
            Vehicle vehicle = ctx.Vehicles.FirstOrDefault(v => v.Numberplate == numberplate);
            return vehicle;
        }

        public IEnumerable<Vehicle> ReadVehicles()
        {
            IEnumerable<Vehicle> vehicles = ctx.Vehicles.Include("Holder").ToList<Vehicle>();
            return vehicles;
        }

        public IEnumerable<Vehicle> ReadVehicles(string numberplate)
        {
            IEnumerable<Vehicle> vehicles = ctx.Vehicles.Include("Holder").
                Where(v => (v.Numberplate.ToLower()).Contains(numberplate.ToLower())).ToList<Vehicle>();
            return vehicles;
        }

        //SQL View moet via een repo opgehaald worden
        public void ExecuteViewQuery()
        {
            var whatIsThis = ctx.Database.ExecuteSqlCommand(@"select h.Name Naam, h.FirstName Voornaam, h.SamAccountName,(select cast(case when (
                                           CAST(GETDATE() AS DATE) between CAST(StartDate AS DATE)
                                           and CAST(EndDate AS DATE))
                                           THEN CAST(1 AS BIT)
                                           ELSE CAST(0 AS BIT) END AS BIT)) Actief, h.Email Mail, h.MifareSerial, cmp.CompanyName
                                           from dbo.Holders h
                                           join dbo.Contracts c on h.Id = c.Holder_Id
                                           join dbo.Companies cmp on cmp.CompanyId = h.Company_CompanyId; ");
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
