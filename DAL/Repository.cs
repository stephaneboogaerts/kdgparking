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
            ctx.Database.Initialize(false);
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
            Holder holder = ctx.Holders.Include("Company").FirstOrDefault(x => x.Id == holderId);
            return holder;
        }

        // Zoekt in db op PNumber (enkel Holders toegevoegd adhv excel hebben deze value)
        public Holder ReadHolder(string pNumber)
        {
            Holder holder = ctx.Holders.Include("Company").FirstOrDefault(x => x.HolderNumber == pNumber);
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

        public Contract ReadContract(string contractId)
        {
            Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.ContractId == contractId);
            return contract;
        }

        public Contract ReadContract(int Id)
        {
            Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.Id == Id);
            return contract;
        }

        public Contract ReadHolderContract(int holderId)
        {
            Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.HolderId == holderId);
            return contract;
        }

        public void DeleteContract(Contract contract)
        {
            ctx.Contracts.Remove(contract);
            ctx.SaveChanges();
        }

        public void UpdateContract(Contract contract)
        {
            ctx.Entry(contract).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
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

        public Company CreateCompany(Company company)
        {
            ctx.Companies.Add(company);
            ctx.SaveChanges();

            return company;
        }

        public Company ReadCompany(string companyName)
        {
            Company company = ctx.Companies.FirstOrDefault(c => c.CompanyName == companyName);
            return company;
        }

        public List<Company> ReadCompanies()
        {
            List<Company> companies = ctx.Companies.ToList<Company>();
            return companies;
        }
    }
}
