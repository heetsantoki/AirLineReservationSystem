namespace AirLineReservation.ViewModel
{
    public class FlightResultViewModel
    {
        public int FlightId { get; set; }
        public string AirlineName { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public string DepartureAirportCode { get; set; } = null!;
        public string DepartureAirportName { get; set; } = null!;
        public string ArrivalAirportCode { get; set; } = null!;
        public string ArrivalAirportName { get; set; } = null!;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
    }
}
