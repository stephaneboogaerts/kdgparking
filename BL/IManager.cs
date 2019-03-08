using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using kdgparking.BL.Domain;

namespace kdgparking.BL
{
    public interface IManager
    {
        // Holder
        Holder ChangeHolder(int id, InputHolder updatedHolder);
        Holder GetHolder(int id);
        Holder GetHolder(string pNumber);
        Holder AddHolder(string name);
        Holder AddHolder(string name, string firstName, string holderNr, string email, string phone, string gsm, string city, string street, string post, Company company);
        Holder AddHolder(string name, string firstName, string phone, string email);
        Holder AddNewHolder(InputHolder holder);
        IEnumerable<Holder> GetHolders();
        IEnumerable<Holder> GetHolders(string searchString);
        IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles();
        IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles(string company);
        List<InputHolder> ProcessInputholderList(List<InputHolder> inputHolderList);
        void RemoveHolder(int id);
        //Contract
        Contract AddContract(int holderId, string numberplate, DateTime begin, DateTime end, decimal tarif, decimal warranty, decimal warrantyBadge);
        Contract AddContract(string contractId, Holder holder, List<Vehicle> vehicle, DateTime begin, DateTime end, decimal tarif, decimal warranty, decimal warrantyBadge);
        Contract GetContract(string ContractId);
        Contract GetContract(int Id);
        Contract GetHolderContract(int HolderId);
        void ChangeContract(Contract contract);
        //Vehicle
        Vehicle AddVehicle(string vehicleName, string numberPlate);
        Vehicle GetVehicle(string numberplate);
        IEnumerable<Vehicle> GetVehicles();
        IEnumerable<Vehicle> GetVehicles(string numberplate);
        //Company
        Company AddCompany(string companyName);
        Company GetCompany(string companyName);
        List<Company> GetCompanies(); // <-- Misschien handig voor een dropdown box

        //File Upload
        List<InputHolder> ProcessFile(HttpPostedFileBase file);
        string CsvExport(IEnumerable<Vehicle> vehicles);
    }
}
