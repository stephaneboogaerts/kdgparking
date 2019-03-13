using kdgparking.BL.Domain;
using kdgparking.DAL;
using kdgparking.DAL.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public Holder ChangeHolder(int id, InputHolder updatedHolder)
        {
            CompanyManager compMng = new CompanyManager(repo.ctx);
            ContractManager contMng = new ContractManager(repo.ctx);

            Holder changedHolder = contMng.HandleBadgeAssignment(id, updatedHolder.Badge, updatedHolder.StartDate, updatedHolder.EndDate);

            changedHolder.Name = updatedHolder.Name;
            changedHolder.FirstName = updatedHolder.FirstName;
            changedHolder.Email = updatedHolder.Email;

            //Add new Company indien nodig
            if (!updatedHolder.Company.Equals(changedHolder.Company.CompanyName))
            {
                Company HolderCompany = compMng.CheckAndCreateCompany(updatedHolder.Company);
                changedHolder.Company = HolderCompany;
            }
            repo.UpdateHolder(changedHolder);
            return changedHolder;
        }

        public void ChangeHolder(Holder holder)
        {
            repo.UpdateHolder(holder);
        }

        public Holder AddNewHolder(InputHolder inputHolder)
        {
            // Bij het toevoegen van een nieuwe holder heb je een badge nodig om een termijn toe te voegen
            CompanyManager CompMng = new CompanyManager(repo.ctx);
            ContractManager ContMng = new ContractManager(repo.ctx);
            Company HolderCompany = CompMng.CheckAndCreateCompany(inputHolder.Company);
            Holder CreatedHolder = new Holder(inputHolder);
            if (GetHolderByMifareSerial(CreatedHolder.MifareSerial) != null) {
                throw new ArgumentException("Mifare serial already exists"); 
            }

            CreatedHolder.Company = HolderCompany;
            CreatedHolder.Contracts = new List<Contract>();
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

        public Holder GetHolderByMifareSerial(string MifareSerial)
        {
            return repo.ReadHolderByMifareSerial(MifareSerial);
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
            return repo.CreateHolder(holder);
        }

        //Addholder gebruikt door Excel
        public Holder AddHolder(string name, string firstName, string holderNr, string email,
            string phone, string gsm, string city, string street, string post, string companyName)
        {
            CompanyManager CompMng = new CompanyManager(repo.ctx);
            Company company = CompMng.CheckAndCreateCompany(companyName);

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
            string ContractId = ContMng.GetHolderContract(id).ContractId;

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
            Vehicle vehicle = new Vehicle(vehicleName, numberPlate);

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

        //SQL view van de website voor debugging
        public void GetSQLView()
        {
            repo.ExecuteViewQuery();
            return;

        }

        //Creert de InputHolder klasse van een holder om deze te displayen bij een view
        public InputHolder ComposeInputHolder(Holder holder)
        {
            IContractManager ContMng = new ContractManager();
            Contract holderContract = ContMng.GetHolderContract(holder.Id);

            return new InputHolder()
            {
                HolderId = holder.Id,
                Name = holder.Name,
                FirstName = holder.FirstName,
                Company = holder.Company.CompanyName,
                Email = holder.Email,
                StartDate = holderContract.StartDate,
                EndDate = holderContract.EndDate,
            };
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