using System;
using System.ComponentModel.DataAnnotations;

namespace AirLineReservation.ViewModel
{
    public class BookingViewModel
    {
        // Flight details shown to user (read-only)
        public int FlightId { get; set; }
        public string Airline { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }

        // Passenger details entered by the user
        [Required]
        public string PassengerName { get; set; } = string.Empty;

        [Required]
        public int PassengerAge { get; set; }

        [Required]
        public string Gender { get; set; } = "";

        [Required]
        public int Passengers { get; set; } = 1;
    }
}
