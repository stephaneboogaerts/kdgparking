using kdgparking.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL
{
    public interface ICompanyManager
    {
        //Company
        Company AddCompany(string companyName);
        Company GetCompany(string companyName);
        List<Company> GetCompanies(string searchString);
        List<Company> GetCompanies(); // <-- Misschien handig voor een dropdown box
    }
}
