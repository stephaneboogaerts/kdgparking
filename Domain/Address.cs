using System.ComponentModel.DataAnnotations;

namespace kdgparking.BL.Domain
{
    public class Address
    {
        // Verhuist naar Holder klasse
        [Key]
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

    }
}
