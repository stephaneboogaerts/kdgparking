using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.DAL;
using kdgparking.BL.Domain;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Web;
using OfficeOpenXml;
using System.IO;
using Syroot.Windows.IO;
using kdgparking.DAL.EF;

namespace kdgparking.BL
{
    public class HolderManager : IHolderManager
    {
        private readonly IHolderRepository repo;

        public HolderManager()
        {
            repo = new HolderRepository();
        }

        public HolderManager(OurDbContext context)
        {
            repo = new HolderRepository(context);
        }

        private void SeedTestData()
        {
            Holder testDummy = new Holder()
            {
                Name = "McTesterson",
                FirstName = "Test",
                Phone = "0123456789",
                GSM = "9874563210",
                Email = "mctesterson@testers.tst",
                HolderNumber = "P0110"
            };
            this.AddHolder(testDummy);
        }

        //REFACTORING & DOCUMENTATIE NODIG
        public Holder ChangeHolder(int id, InputHolder updatedHolder)
        {
            CompanyManager compMng = new CompanyManager(repo.ctx);
            ContractManager contMng = new ContractManager(repo.ctx);

            /////////////////////////////////////////////////////////
            //Holder changedHolder = this.GetHolder(id);
            Holder changedHolder = contMng.HandleBadgeAssignment(id, updatedHolder.Badge, updatedHolder.StartDate, updatedHolder.EndDate);

            changedHolder.Name = updatedHolder.Name;
            changedHolder.FirstName = updatedHolder.FirstName;
            changedHolder.Email = updatedHolder.Email;

            //ChangedHolder.Contract.StartDate = updatedHolder.StartDate;
            //ChangedHolder.Contract.EndDate = updatedHolder.EndDate;
            /////////////////////////////////////////////////////////

            //Add new Company indien nodig
            if (!updatedHolder.Company.Equals(changedHolder.Company.CompanyName))
            {
                Company HolderCompany = compMng.GetCompany(updatedHolder.Company);
                if (HolderCompany == null)
                {
                    HolderCompany = compMng.AddCompany(updatedHolder.Company);
                }
                changedHolder.Company = HolderCompany;
            }
            repo.UpdateHolder(changedHolder);
            //contMng.ChangeContract(changedHolder.Contract);
            return changedHolder;
        }

        public void ChangeHolder(Holder holder)
        {
            repo.UpdateHolder(holder);
        }

        public Holder AddHolder(string name)
        {
            Holder h = new Holder()
            {
                Name = name
            };
            return this.AddHolder(h);
        }

        public Holder AddHolder(string name, string firstName, string phone, string email)
        {
            Holder h = new Holder()
            {
                Name = name,
                FirstName = firstName, // <-- te verplaatsen naar overload functie (als organisatie geen aparte klasse wordt)
                Phone = phone,
                Email = email
            };
            return this.AddHolder(h);
        }

        public Holder AddNewHolder(InputHolder inputHolder)
        {
            // Bij het toevoegen van een nieuwe holder heb je een badge nodig om een termijn toe te voegen
            CompanyManager CompMng = new CompanyManager(repo.ctx);
            ContractManager ContMng = new ContractManager(repo.ctx);
            Company HolderCompany = CompMng.GetCompany(inputHolder.Company);
            if (HolderCompany == null)
            {
                HolderCompany = CompMng.AddCompany(inputHolder.Company);
            }
            //Contract NewContract = new Contract()
            //{
            //    StartDate = inputHolder.StartDate,
            //    EndDate = inputHolder.EndDate
            //};
            Holder CreatedHolder = new Holder()
            {
                Name = inputHolder.Name,
                FirstName = inputHolder.FirstName,
                Email = inputHolder.Email,
                Company = HolderCompany,
                Contracts = new List<Contract>()
            };

            CreatedHolder = this.AddHolder(CreatedHolder);
            return CreatedHolder;
        }

        public Holder GetHolder(int id)
        {
            return repo.ReadHolder(id);
        }

        public Holder GetHolder(string pNumber)
        {
            return repo.ReadHolder(pNumber);
        }

        public IEnumerable<Holder> GetHolders()
        {
            return repo.ReadHolders();
        }

        public IEnumerable<Holder> GetHolders(string searchString)
        {
            return repo.ReadHolders(searchString);
        }

        public IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles()
        {
            return repo.ReadHoldersWithContractsAndVehicles();
        }

        public IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles(string company)
        {
            return repo.ReadHoldersWithContractsAndVehicles(company);
        }

        private Holder AddHolder(Holder holder)
        {
            // Validatie gebeurt in InputHolder
            return repo.CreateHolder(holder);
        }

        public Holder AddHolder(string name, string firstName, string holderNr, string email,
            string phone, string gsm, string city, string street, string post, string companyName)
        {
            CompanyManager CompMng = new CompanyManager(repo.ctx);
            // Kijken of een Company alreeds bestaat in de DB
            // Wanneer de Company niet gevonden word, wordt er een nieuwe Company aangemaakt
            Company company = CompMng.GetCompany(companyName);
            if (company == null)
            {
                company = CompMng.AddCompany(companyName);
            }

            Holder holder = new Holder()
            {
                Name = name,
                FirstName = firstName,
                HolderNumber = holderNr,
                Email = email,
                Phone = phone,
                GSM = gsm,
                City = city,
                Street = street,
                PostalCode = post,
                Company = company,
                Vehicles = new List<Vehicle>()
            };
            return this.AddHolder(holder);
        }

        public void RemoveHolder(int id)
        {
            ContractManager ContMng = new ContractManager(repo.ctx);
            int ContractId = ContMng.GetHolderContract(id).Id;
            repo.DeleteHolder(this.GetHolder(id));
            ContMng.DeleteContract(ContMng.GetContract(ContractId));
            return;
        }

        public Holder GetHolderWithBadges(int holderId)
        {
            return repo.ReadHolderWithBadges(holderId);
        }

        public Vehicle AddVehicle(string vehicleName, string numberPlate)
        {
            Vehicle vehicle = new Vehicle()
            {
                VehicleName = vehicleName,
                Numberplate = numberPlate
            };

            return repo.CreateVehicle(vehicle);
        }

        public Vehicle GetVehicle(string numberplate)
        {
            return repo.ReadVehicle(numberplate);
        }

        public IEnumerable<Vehicle> GetVehicles()
        {
            return repo.ReadVehicles();
        }

        public IEnumerable<Vehicle> GetVehicles(string numberplate)
        {
            return repo.ReadVehicles(numberplate);
        }

        public void Validate(InputHolder inputHolder)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(inputHolder, new ValidationContext(inputHolder), errors, validateAllProperties: true);

            if (!valid)
                throw new ValidationException("InputHolder " + inputHolder.Name + " not valid!");
        }
    }
}