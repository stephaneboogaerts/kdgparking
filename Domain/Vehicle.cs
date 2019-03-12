
using System.ComponentModel.DataAnnotations;

namespace kdgparking.BL.Domain
{
    public class Vehicle
    {
        [Key]
        public int VoertuidId { get; set; }
        public string VehicleName { get; set; }
        public string Numberplate { get; set; }

        public Contract Contract { get; set; }

        public Vehicle(string vehicleName, string numberplate)
        {
            VehicleName = vehicleName;
            Numberplate = numberplate;
        }
    }
}
