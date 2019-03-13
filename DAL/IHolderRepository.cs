using kdgparking.BL.Domain;
using kdgparking.DAL.EF;
using System;
using System.Collections.Generic;

namespace kdgparking.DAL
{
    public interface IHolderRepository : IDisposable
    {
        OurDbContext ctx { get; }

        Holder CreateHolder(Holder persoon);
        Holder ReadHolder(int holderId);
        Holder ReadHolderByMifareSerial(string MyfareSerial);
        Holder ReadHolder(string pNumber);
        IEnumerable<Holder> ReadHolders();
        IEnumerable<Holder> ReadHolders(string searchString);
        IEnumerable<Holder> ReadHoldersWithContractsAndVehicles();
        void UpdateHolder(Holder holder);
        void DeleteHolder(Holder holder);
        IEnumerable<Holder> ReadHoldersWithContractsAndVehicles(string company);
        Holder ReadHolderWithBadges(int holderId);

        Vehicle CreateVehicle(Vehicle vehicle);
        Vehicle ReadVehicle(string numberplate);
        IEnumerable<Vehicle> ReadVehicles();
        IEnumerable<Vehicle> ReadVehicles(string numberplate);

        void ExecuteViewQuery();
    }
}
