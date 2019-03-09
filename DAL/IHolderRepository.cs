using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kdgparking.BL.Domain;

namespace kdgparking.DAL
{
    public interface IHolderRepository : IDisposable
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




    }
}
