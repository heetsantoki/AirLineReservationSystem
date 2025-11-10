// File: YourProjectName/ViewModels/FlightSearchViewModel.cs

using System;
using System.ComponentModel.DataAnnotations;
using AirLineReservation.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering; // Good for validation attributes

namespace AirlineReservation.ViewModel // Adjust your namespace
{
    public class FlightSearchViewModel
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DepartureAirportName { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string ArrivalAirportName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
        public int NumberOfPassengers { get; set; }


        [Required(ErrorMessage = "Departure airport is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "IATA code must be 3 characters.")]
        [Display(Name = "From (IATA Code)")]
        public string DepartureAirportIATA { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arrival airport is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "IATA code must be 3 characters.")]
        [Display(Name = "To (IATA Code)")]
        public string ArrivalAirportIATA { get; set; } = string.Empty;

        [Required(ErrorMessage = "Departure date is required.")]
        [DataType(DataType.Date)]

        [Range(1, 9, ErrorMessage = "Number of passengers must be between 1 and 9.")]
        public List<SelectListItem>? AvailableAirports { get; set; } // ✅ add this if missing.
        public List<FlightResultViewModel>? SearchResults { get; set; }



        // You might have other properties here, like:
        // public string FlightClass { get; set; } // Economy, Business, First
    }
}