using kdgparking.BL.Domain;
using System.Collections.Generic;

namespace kdgparking.BL
{
    public interface IHolderManager
    {
        // Holder
        Holder ChangeHolder(int id, InputHolder updatedHolder);
        Holder GetHolder(int id);
        Holder GetHolder(string pNumber);
        Holder AddHolder(string name, string firstName, string holderNr, string email, string phone, string gsm, string city, string street, string post, string company);
        Holder AddNewHolder(InputHolder holder);
        IEnumerable<Holder> GetHolders();
        IEnumerable<Holder> GetHolders(string searchString);
        IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles();
        IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles(string company);
        void RemoveHolder(int id);
        void ChangeHolder(Holder holder);
        Holder GetHolderWithBadges(int holderId);
        InputHolder ComposeInputHolder(Holder holder)

        //Vehicle
        Vehicle AddVehicle(string vehicleName, string numberPlate);
        Vehicle GetVehicle(string numberplate);
        IEnumerable<Vehicle> GetVehicles();
        IEnumerable<Vehicle> GetVehicles(string numberplate);

        void GetSQLView();
    }
}
