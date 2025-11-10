namespace AirLineReservation.ViewModel
{
    public class MyBookingsViewModel
    {
        public int BookingId { get; set; }
        public string PassengerName { get; set; }
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
    }
}
