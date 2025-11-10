using System.ComponentModel.DataAnnotations;

namespace AirLineReservation.ViewModel
{
    public class SearchViewModel
    {
        [Required, StringLength(3)]
        public string Departure { get; set; } = null!;

        [Required, StringLength(3)]
        public string Arrival { get; set; } = null!;

        [Required]
        public DateTime DepartureDate { get; set; } = DateTime.UtcNow.Date;

        [Range(1, 9)]
        public int Passengers { get; set; } = 1;
    }
}
