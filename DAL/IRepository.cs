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
        Holder ReadHolder(string holderId);
        IEnumerable<Holder> ReadHolders();
        IEnumerable<Holder> ReadHoldersWithContractsAndVehicles();

        Contract CreateContract(Contract contract);
        Contract ReadContract(int contractNr);

        Vehicle CreateVehicle(Vehicle vehicle);
        Vehicle ReadVehicle(string numberplate);
    }
}
