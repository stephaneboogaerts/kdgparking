
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace kdgparking.BL.Domain
{
    public class Vehicle
    {
        [Key]
        public int VoertuidId { get; set; }
        [DisplayName("Model")]
        public string VehicleName { get; set; }
        [DisplayName("Nummerplaat")]
        public string Numberplate { get; set; }

        public Contract Contract { get; set; }

        public Vehicle() { }
        public Vehicle(string vehicleName, string numberplate)
        {
            VehicleName = vehicleName;
            Numberplate = numberplate;
        }
    }
}
