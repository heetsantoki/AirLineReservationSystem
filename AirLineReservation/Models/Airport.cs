using System.ComponentModel.DataAnnotations;

namespace AirLineReservation.Models
{
    public class Airport
    {
        [Key]
        [StringLength(3)]
        public string IATA { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
