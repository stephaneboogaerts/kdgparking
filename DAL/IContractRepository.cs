using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kdgparking.BL.Domain;

namespace kdgparking.DAL
{
    public interface IContractRepository : IDisposable
    {
        Contract CreateContract(Contract contract);
        //Contract ReadContract(string contractId);
        Contract ReadContract(int Id);
        Contract ReadHolderContract(int holderId);
        Contract UpdateContract(Contract contract);
        void DeleteContract(Contract contract);

        ContractHistory CreateContractHistory(ContractHistory contractHist);

        Vehicle CreateVehicle(Vehicle vehicle);
        Vehicle ReadVehicle(string numberplate);
        IEnumerable<Vehicle> ReadVehicles();
        IEnumerable<Vehicle> ReadVehicles(string numberplate);
    }
}
