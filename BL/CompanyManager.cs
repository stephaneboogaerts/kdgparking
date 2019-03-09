using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL
{
    class CompanyManager
    {
        public Company AddCompany(string companyName)
        {
            Company company = new Company()
            {
                CompanyName = companyName
            };
            return this.AddCompany(company);
        }

        public Company GetCompany(string companyName)
        {
            return repo.ReadCompany(companyName);
        }

        public List<Company> GetCompanies()
        {
            return repo.ReadCompanies();
        }

        public List<Company> GetCompanies(string searchString)
        {
            return repo.ReadCompanies(searchString);
        }

        private Company AddCompany(Company company)
        {
            // Validatie gebeurt in InputHolder
            return repo.CreateCompany(company);
        }

    }
}
