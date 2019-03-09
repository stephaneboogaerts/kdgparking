using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kdgparking.BL.Domain;
using kdgparking.DAL.EF;

namespace kdgparking.DAL
{
    public interface IContractRepository : IDisposable
    {
        OurDbContext ctx { get; }
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
