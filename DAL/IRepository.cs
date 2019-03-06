using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.BL.Domain;

namespace kdgparking.DAL
{
    public interface IRepository
    {
        Holder CreateHolder(Holder persoon);
        Holder ReadHolder(int holderId);
        Holder ReadHolder(string pNumber);
        IEnumerable<Holder> ReadHolders();
        IEnumerable<Holder> ReadHolders(string searchString);
        IEnumerable<Holder> ReadHoldersWithContractsAndVehicles();
        void UpdateHolder(Holder holder);
        void DeleteHolder(Holder holder);
        IEnumerable<Holder> ReadHoldersWithContractsAndVehicles(string company);

        Contract CreateContract(Contract contract);
        Contract ReadContract(string contractId);
        Contract ReadContract(int Id);
        Contract ReadHolderContract(int holderId);
        void UpdateContract(Contract contract);
        void DeleteContract(Contract contract);

        Vehicle CreateVehicle(Vehicle vehicle);
        Vehicle ReadVehicle(string numberplate);
        IEnumerable<Vehicle> ReadVehicles(string numberplate);

        Company CreateCompany(Company company);
        Company ReadCompany(string companyName);
        List<Company> ReadCompanies();
    }
}
