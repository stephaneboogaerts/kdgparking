using kdgparking.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL
{
    public interface IContractManager
    {
        //Contract
        Contract AddContract(Contract contract);
        Contract AddContract(int holderId, string numberplate, DateTime begin, DateTime end);
        Contract AddContract(Holder holder, List<Vehicle> vehicle, DateTime begin, DateTime end);
        //Contract GetContract(string ContractId);
        Contract GetContract(int Id);
        Contract GetHolderContract(int HolderId);
        Contract ChangeContract(Contract contract);
        //ContractHistory
        ContractHistory AddContractHistory(string contractId, Holder holder, DateTime begin, DateTime end, decimal warranty, decimal warrantyBadge);
        //Vehicle
        Vehicle AddVehicle(string vehicleName, string numberPlate);
        Vehicle GetVehicle(string numberplate);
        IEnumerable<Vehicle> GetVehicles();
        IEnumerable<Vehicle> GetVehicles(string numberplate);
    }
}
