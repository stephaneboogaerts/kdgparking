using kdgparking.BL.Domain;
using System.Collections.Generic;

namespace kdgparking.BL
{
    public interface ICompanyManager
    {
        //Company
        Company AddCompany(string companyName);
        Company GetCompany(string companyName);
        Company GetCompany(int id);
        Company UpdateCompany(Company company);
        List<Company> GetCompanies(string searchString);
        Company CheckAndCreateCompany(string companyName);
        List<Company> GetCompanies(); // <-- Misschien handig voor een dropdown box
    }
}
