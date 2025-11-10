using AirLineReservation.Models;

namespace AirLineReservation.ViewModel
{
    public class SearchResultsModel
    {
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public DateTime TravelDate { get; set; }   
        public string From { get; set; }
        public string To { get; set; }
        public string Date { get; set; }
        public int Passengers { get; set; }

        public IEnumerable<Flight> Flights { get; set; } // ✅ Must be Flight, not FlightViewModel
    }
}
