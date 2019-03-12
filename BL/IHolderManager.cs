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
    public interface IHolderManager
    {
        // Holder
        Holder ChangeHolder(int id, InputHolder updatedHolder);
        Holder GetHolder(int id);
        Holder GetHolder(string pNumber);
        Holder AddHolder(string name);
        Holder AddHolder(string name, string firstName, string holderNr, string email, string phone, string gsm, string city, string street, string post, string company);
        Holder AddHolder(string name, string firstName, string phone, string email);
        Holder AddNewHolder(InputHolder holder);
        IEnumerable<Holder> GetHolders();
        IEnumerable<Holder> GetHolders(string searchString);
        IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles();
        IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles(string company);
        void RemoveHolder(int id);
        void ChangeHolder(Holder holder);
        Holder GetHolderWithBadges(int holderId);

        //Vehicle
        Vehicle AddVehicle(string vehicleName, string numberPlate);
        Vehicle GetVehicle(string numberplate);
        IEnumerable<Vehicle> GetVehicles();
        IEnumerable<Vehicle> GetVehicles(string numberplate);

        void GetSQLView();
    }
}
