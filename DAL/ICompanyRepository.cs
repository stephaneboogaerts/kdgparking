using kdgparking.BL.Domain;
using kdgparking.DAL.EF;
using System;
using System.Collections.Generic;

namespace kdgparking.DAL
{
    public interface ICompanyRepository : IDisposable
    {
        OurDbContext ctx { get; }
        Company CreateCompany(Company company);
        Company ReadCompany(string companyName);
        Company ReadCompany(int id);
        Company UpdateCompany(Company company);
        List<Company> ReadCompanies(string searchString);
        List<Company> ReadCompanies();
    }
}
