﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kdgparking.BL.Domain;

namespace kdgparking.BL
{
    class ContractManager
    {
        public Contract AddContract(int holderId, string numberplate, DateTime begin, DateTime end)
        {
            Holder holder = this.GetHolder(holderId);
            Vehicle vehicle = this.GetVehicle(numberplate);
            List<Vehicle> vehicles = new List<Vehicle>();
            vehicles.Add(vehicle);

            Contract contract = new Contract
            {
                Holder = holder,
                StartDate = begin,
                EndDate = end,
                Vehicles = vehicles
            };

            return this.AddContract(contract);
        }

        public Contract AddContract(Holder holder, List<Vehicle> vehicles, DateTime begin, DateTime end)
        {
            Contract contract = new Contract
            {
                Holder = holder,
                StartDate = begin,
                EndDate = end,
                Vehicles = vehicles
            };

            return this.AddContract(contract);
        }

        public ContractHistory AddContractHistory(string contractId, Holder holder, DateTime begin, DateTime end,
            decimal warranty, decimal warrantyBadge)
        {
            ContractHistory contractH = new ContractHistory()
            {
                ContractId = contractId,
                Holder = holder,
                StartDate = begin,
                EndDate = end,
                Warranty = warranty,
                WarrantyBadge = warrantyBadge
            };
            return this.AddContractHistory(contractH);
        }

        private ContractHistory AddContractHistory(ContractHistory contractHistory)
        {
            return repo.CreateContractHistory(contractHistory);
        }

        private Contract AddContract(Contract contract)
        {
            // Validatie gebeurt in InputHolder
            return repo.CreateContract(contract);
        }

        public Contract GetContract(int Id)
        {
            return repo.ReadContract(Id);
        }

        //public Contract GetContract(string ContractId)
        //{
        //    return repo.ReadContract(ContractId);
        //}

        public Contract GetHolderContract(int HolderId)
        {
            return repo.ReadHolderContract(HolderId);
        }

        public Contract ChangeContract(Contract contract)
        {
            // Validatie gebeurt in InputHolder
            return repo.UpdateContract(contract);
        }

        public Vehicle AddVehicle(string vehicleName, string numberPlate)
        {
            Vehicle vehicle = new Vehicle()
            {
                VehicleName = vehicleName,
                Numberplate = numberPlate
            };

            return repo.CreateVehicle(vehicle);
        }

        public Vehicle GetVehicle(string numberplate)
        {
            return repo.ReadVehicle(numberplate);
        }

        public IEnumerable<Vehicle> GetVehicles()
        {
            return repo.ReadVehicles();
        }

        public IEnumerable<Vehicle> GetVehicles(string numberplate)
        {
            return repo.ReadVehicles(numberplate);
        }

    }
}
