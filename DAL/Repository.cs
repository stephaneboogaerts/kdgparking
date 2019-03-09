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

        public Contract CreateContract(Contract contract)
        {
            ctx.Contracts.Add(contract);
            ctx.SaveChanges();

            return contract;
        }

        //public Contract ReadContract(string contractId)
        //{
        //    Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.ContractId == contractId);
        //    return contract;
        //}

        public Contract ReadContract(int Id)
        {
            Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.Id == Id);
            return contract;
        }

        public Contract ReadHolderContract(int holderId)
        {
            //Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.HolderId == holderId);
            Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.Holder.Id == holderId);
            return contract;
        }

        public void DeleteContract(Contract contract)
        {
            ctx.Contracts.Remove(contract);
            ctx.SaveChanges();
        }

        public Contract UpdateContract(Contract contract)
        {
            ctx.Entry(contract).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();

            return contract;
        }

        public ContractHistory CreateContractHistory(ContractHistory contractHist)
        {
            ctx.ContractHistories.Add(contractHist);
            ctx.SaveChanges();

            return contractHist;
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
            IEnumerable<Vehicle> vehicles = ctx.Vehicles.Include("Contract.Holder").ToList<Vehicle>();
            return vehicles;
        }

        public IEnumerable<Vehicle> ReadVehicles(string numberplate)
        {
            IEnumerable<Vehicle> vehicles = ctx.Vehicles.Include("Contract.Holder").
                Where(v => (v.Numberplate.ToLower()).Contains(numberplate.ToLower())).ToList<Vehicle>();
            return vehicles;
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

        public List<Company> ReadCompanies(string searchString)
        {
            List<Company> companies = ctx.Companies.Where(c => (c.CompanyName.ToLower()).Contains(searchString.ToLower())).ToList<Company>();
            return companies;
        }
    }
}
