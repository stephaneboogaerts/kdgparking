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
        private readonly IHolderRepository HolderRepo;

        public HolderManager()
        {
            HolderRepo = new HolderRepository();
        }

        public HolderManager(OurDbContext context)
        {
            HolderRepo = new HolderRepository(context);
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
            CompanyManager CompMng = new CompanyManager(HolderRepo.ctx);
            ContractManager ContMng = new ContractManager(HolderRepo.ctx);
            Holder ChangedHolder = this.GetHolder(id);
            ChangedHolder.Name = updatedHolder.Name;
            ChangedHolder.FirstName = updatedHolder.FirstName;
            ChangedHolder.Email = updatedHolder.Email;
            ChangedHolder.Contract.StartDate = updatedHolder.StartDate;
            ChangedHolder.Contract.EndDate = updatedHolder.EndDate;
            //Add new Company indien nodig
            if (!updatedHolder.Company.Equals(ChangedHolder.Company.CompanyName))
            {
                Company HolderCompany = CompMng.GetCompany(updatedHolder.Company);
                if (HolderCompany == null)
                {
                    HolderCompany = CompMng.AddCompany(updatedHolder.Company);
                }
                ChangedHolder.Company = HolderCompany;
            }
            HolderRepo.UpdateHolder(ChangedHolder);
            ContMng.ChangeContract(ChangedHolder.Contract);
            return ChangedHolder;
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
            CompanyManager CompMng = new CompanyManager(HolderRepo.ctx);
            ContractManager ContMng = new ContractManager(HolderRepo.ctx);
            Company HolderCompany = CompMng.GetCompany(inputHolder.Company);
            if (HolderCompany == null)
            {
                HolderCompany = CompMng.AddCompany(inputHolder.Company);
            }
            Contract NewContract = new Contract()
            {
                StartDate = inputHolder.StartDate,
                EndDate = inputHolder.EndDate
            };
            Holder CreatedHolder = new Holder()
            {
                Name = inputHolder.Name,
                FirstName = inputHolder.FirstName,
                Email = inputHolder.Email,
                Company = HolderCompany,
                Contract = NewContract
                //CompanyId = HolderCompany.CompanyId
            };
            CreatedHolder = this.AddHolder(CreatedHolder);
            //NewContract.HolderId = CreatedHolder.Id;
            return CreatedHolder;
        }

        public Holder GetHolder(int id)
        {
            return HolderRepo.ReadHolder(id);
        }

        public Holder GetHolder(string pNumber)
        {
            return HolderRepo.ReadHolder(pNumber);
        }

        public IEnumerable<Holder> GetHolders()
        {
            return HolderRepo.ReadHolders();
        }

        public IEnumerable<Holder> GetHolders(string searchString)
        {
            return HolderRepo.ReadHolders(searchString);
        }

        public IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles()
        {
            return HolderRepo.ReadHoldersWithContractsAndVehicles();
        }

        public IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles(string company)
        {
            return HolderRepo.ReadHoldersWithContractsAndVehicles(company);
        }

        private Holder AddHolder(Holder holder)
        {
            // Validatie gebeurt in InputHolder
            return HolderRepo.CreateHolder(holder);
        }

        public Holder AddHolder(string name, string firstName, string holderNr, string email,
            string phone, string gsm, string city, string street, string post, string companyName)
        {
            CompanyManager CompMng = new CompanyManager(HolderRepo.ctx);
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
                Company = company
            };
            return this.AddHolder(holder);
        }

        public void RemoveHolder(int id)
        {
            ContractManager ContMng = new ContractManager(HolderRepo.ctx);
            int ContractId = ContMng.GetHolderContract(id).Id;
            HolderRepo.DeleteHolder(this.GetHolder(id));
            ContMng.DeleteContract(ContMng.GetContract(ContractId));
            return;
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