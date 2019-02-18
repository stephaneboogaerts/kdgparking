using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.DAL;
using kdgparking.BL.Domain;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace kdgparking.BL
{
    public class Manager : IManager
    {
        private readonly IRepository repo;

        public Manager()
        {
            repo = new kdgparking.DAL.Repository();
        }

        public Holder AddHolder(string id, string name, string firstName, int phone, string email)
        {
            Holder h = new Holder
            {
                HolderNumber = id,
                Name = name,
                FirstName = firstName, // <-- te verplaatsen naar overload functie
                Phone = phone,
                Email = email
            };
            return this.AddHolder(h);
        }

        public Holder GetHolders()
        {
            throw new NotImplementedException();
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
    }
}
