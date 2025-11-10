using System.ComponentModel.DataAnnotations;

namespace AirLineReservation.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FlightId { get; set; }
        public string PassengerName { get; set; }
        public int PassengerAge { get; set; }
        public string Gender { get; set; }
        public int Passengers { get; set; }
        public string? FareType { get; set; }

        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public Flight Flight { get; set; }

    }
}
