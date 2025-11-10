using System;
using System.ComponentModel.DataAnnotations;

namespace AirLineReservation.Models
{
    public class Flight
    {
        public int Id { get; set; }

        [Required]
        public string Airline { get; set; } = "";

        [Required]
        public string FlightNumber { get; set; } = "";

        [Required]
        public string From { get; set; } = "";

        [Required]
        public string To { get; set; } = "";

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public double Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
