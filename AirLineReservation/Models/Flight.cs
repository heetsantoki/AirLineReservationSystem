using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // For ForeignKey attribute

namespace AirlineReservation.Models
{
    public class Flight
    {
        public int FlightId { get; set; }

        [Required]
        [StringLength(10)] // e.g., AA100, BA245
        public string FlightNumber { get; set; }

        // Foreign keys for Airport
        public int DepartureAirportId { get; set; }
        [ForeignKey("DepartureAirportId")]
        public Airport DepartureAirport { get; set; }

        public int ArrivalAirportId { get; set; }
        [ForeignKey("ArrivalAirportId")]
        public Airport ArrivalAirport { get; set; }

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