using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.DAL;
using kdgparking.BL.Domain;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Diagnostics;

namespace kdgparking.BL
{
    public class Manager : IManager
    {
        private readonly IRepository repo;

        public Manager()
        {
            repo = new kdgparking.DAL.Repository();
        }

        public Holder AddHolder(string name, string firstName, string phone, string email)
        {
            Holder h = new Holder
            {
                Name = name,
                FirstName = firstName, // <-- te verplaatsen naar overload functie (als organisatie geen aparte klasse wordt)
                Phone = phone,
                Email = email
            };
            return this.AddHolder(h);
        }

        public Holder GetHolder(int id)
        {
            return repo.ReadHolder(id);
        }

        public IEnumerable<Holder> GetHolders()
        {
            return repo.ReadHolders();
        }

        private Holder AddHolder(Holder holder)
        {
            this.Validate(holder);
            return repo.CreateHolder(holder);
        }

        private void Validate(Holder holder)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(holder, new ValidationContext(holder), errors, validateAllProperties: true);

            if (!valid)
                throw new ValidationException("Holder not valid!");
        }

        public void AddNewHolder(InputHolder inputHolder)
        {
            Holder createdHolder = this.AddHolder(inputHolder.naam, inputHolder.voornaam, inputHolder.phone, inputHolder.email);
            this.AddContract(createdHolder.Id, inputHolder.nummerplaat, inputHolder.startDate, inputHolder.endDate);
            return;
        }

        public Contract AddContract(int holderId, string numberplate, DateTime begin, DateTime end, decimal tarif = 0, decimal warranty = 0, decimal warrantyBadge = 0)
        {
            Holder holder = this.GetHolder(holderId);
            Vehicle vehicle = this.GetVehicle(numberplate);
            if (vehicle == null)
            {

            }

            Contract contract = new Contract
            {
                Holder = holder,
                StartDate = begin,
                EndDate = end,
                Tarif = tarif,
                Warranty = warranty,
                WarrantyBadge = warranty,
                Vehicle = vehicle
            };

            return contract;
        }

        public Vehicle GetVehicle(string numberplate)
        {
            return repo.ReadVehicle(numberplate);
        }

    }
}
