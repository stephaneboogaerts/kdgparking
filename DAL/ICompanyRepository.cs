using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kdgparking.BL.Domain;

namespace kdgparking.DAL
{
    public interface ICompanyRepository : IDisposable
    {
        Company CreateCompany(Company company);
        Company ReadCompany(string companyName);
        List<Company> ReadCompanies(string searchString);
        List<Company> ReadCompanies();
    }
}
