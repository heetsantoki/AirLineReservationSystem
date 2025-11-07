using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineReservation.Models
{
    public class Flight
    {
        public int FlightId { get; set; }

        [Required]
        [StringLength(10)] // e.g., AA100, BA245
        public string FlightNumber { get; set; }

        public int DepartureAirportId { get; set; }
        [ForeignKey("DepartureAirportId")]
        public Airport DepartureAirport { get; set; } // Navigation property

        public int ArrivalAirportId { get; set; }
        [ForeignKey("ArrivalAirportId")]
        public Airport ArrivalAirport { get; set; } // Navigation property

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Stores price with 2 decimal places
        public decimal Price { get; set; }

        public int AvailableSeats { get; set; }

        [Required]
        [StringLength(50)]
        public string Airline { get; set; }
    }
}