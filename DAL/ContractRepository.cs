using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kdgparking.BL.Domain;
using kdgparking.DAL.EF;


namespace kdgparking.DAL
{
    public class ContractRepository : IContractRepository
    {
        public OurDbContext ctx { get; }

        public ContractRepository()
        {
            ctx = new OurDbContext();
            ctx.Database.Initialize(false);
        }

        public ContractRepository(OurDbContext context)
        {
            ctx = context;
        }

        public Contract CreateContract(Contract contract)
        {
            ctx.Contracts.Add(contract);
            ctx.SaveChanges();

            return contract;
        }

        //public Contract ReadContract(string contractId)
        //{
        //    Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.ContractId == contractId);
        //    return contract;
        //}

        public Contract ReadContract(int Id)
        {
            Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.Id == Id);
            return contract;
        }

        public Contract ReadHolderContract(int holderId)
        {
            //Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.HolderId == holderId);
            Contract contract = ctx.Contracts.Include("Vehicles").FirstOrDefault(c => c.Holder.Id == holderId);
            return contract;
        }

        public void DeleteContract(Contract contract)
        {
            ctx.Contracts.Remove(contract);
            ctx.SaveChanges();
        }

        public Contract UpdateContract(Contract contract)
        {
            ctx.Entry(contract).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();

            return contract;
        }

        public ContractHistory CreateContractHistory(ContractHistory contractHist)
        {
            ctx.ContractHistories.Add(contractHist);
            ctx.SaveChanges();

            return contractHist;
        }

        public Vehicle CreateVehicle(Vehicle vehicle)
        {
            ctx.Vehicles.Add(vehicle);
            ctx.SaveChanges();

            return vehicle;
        }

        public Vehicle ReadVehicle(string numberplate)
        {
            Vehicle vehicle = ctx.Vehicles.FirstOrDefault(v => v.Numberplate == numberplate);
            return vehicle;
        }

        public IEnumerable<Vehicle> ReadVehicles()
        {
            IEnumerable<Vehicle> vehicles = ctx.Vehicles.Include("Contract.Holder").ToList<Vehicle>();
            return vehicles;
        }

        public IEnumerable<Vehicle> ReadVehicles(string numberplate)
        {
            IEnumerable<Vehicle> vehicles = ctx.Vehicles.Include("Contract.Holder").
                Where(v => (v.Numberplate.ToLower()).Contains(numberplate.ToLower())).ToList<Vehicle>();
            return vehicles;
        }
        
        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
