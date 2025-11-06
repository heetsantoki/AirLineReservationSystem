using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectListItem

namespace AirLineReservation.ViewModel
{
    public class FlightSearchViewModel
    {
        [Required(ErrorMessage = "Departure airport is required.")]
        [Display(Name = "From")]
        public string DepartureAirportIATA { get; set; }

        [Required(ErrorMessage = "Arrival airport is required.")]
        [Display(Name = "To")]
        public string ArrivalAirportIATA { get; set; }

        [Required(ErrorMessage = "Departure date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Departure Date")]
        public DateTime DepartureDate { get; set; }

        [Range(1, 9, ErrorMessage = "Number of passengers must be between 1 and 9.")]
        [Display(Name = "Passengers")]
        public int NumberOfPassengers { get; set; } = 1; // Default to 1 passenger

        // Property to hold search results
        public List<FlightResultViewModel> SearchResults { get; set; } = new List<FlightResultViewModel>();

        // Optional: For dropdown lists of airports (if you choose that UI approach)
        public IEnumerable<SelectListItem> AvailableAirports { get; set; }
    }

    public class FlightResultViewModel
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DepartureAirportName { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string ArrivalAirportName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public TimeSpan Duration => ArrivalTime - DepartureTime;
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
    }
}