using System.Collections.Generic; // For ICollection
using System.ComponentModel.DataAnnotations;

namespace AirlineReservation.Models
{
    public class Airport
    {
        public int AirportId { get; set; }

        [Required]
        [StringLength(3)] // e.g., JFK, LHR
        public string IATACode { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        // Navigation properties for flights
        public ICollection<Flight> DepartingFlights { get; set; } = new List<Flight>();
        public ICollection<Flight> ArrivingFlights { get; set; } = new List<Flight>();
    }
}