using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AirlineReservation.Data;
using AirlineReservation.Models;
using AirlineReservation.ViewModels; // Adjusted to ViewModels folder
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectListItem
using Microsoft.EntityFrameworkCore;

namespace AirlineReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new FlightSearchViewModel
            {
                DepartureDate = DateTime.Today,
                NumberOfPassengers = 1,
                AvailableAirports = await _context.Airports
                                                .OrderBy(a => a.City)
                                                .Select(a => new SelectListItem
                                                {
                                                    Value = a.IATACode,
                                                    Text = $"{a.City} ({a.IATACode}) - {a.Name}"
                                                })
                                                .ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(FlightSearchViewModel model)
        {
            // Re-populate AvailableAirports for the dropdown if needed on postback or for initial load
            model.AvailableAirports = await _context.Airports
                                                .OrderBy(a => a.City)
                                                .Select(a => new SelectListItem
                                                {
                                                    Value = a.IATACode,
                                                    Text = $"{a.City} ({a.IATACode}) - {a.Name}"
                                                })
                                                .ToListAsync();

            if (ModelState.IsValid)
            {
                var departureAirport = await _context.Airports.FirstOrDefaultAsync(a => a.IATACode == model.DepartureAirportIATA);
                var arrivalAirport = await _context.Airports.FirstOrDefaultAsync(a => a.IATACode == model.ArrivalAirportIATA);

                if (departureAirport == null)
                {
                    ModelState.AddModelError(nameof(model.DepartureAirportIATA), "Invalid departure airport code.");
                }
                if (arrivalAirport == null)
                {
                    ModelState.AddModelError(nameof(model.ArrivalAirportIATA), "Invalid arrival airport code.");
                }

                if (!ModelState.IsValid)
                {
                    return View(model); // Re-show form with specific airport errors
                }

                var flights = await _context.Flights
                    .Include(f => f.DepartureAirport)
                    .Include(f => f.ArrivalAirport)
                    .Where(f => f.DepartureAirportId == departureAirport.AirportId &&
                                f.ArrivalAirportId == arrivalAirport.AirportId &&
                                f.DepartureTime.Date == model.DepartureDate.Date &&
                                f.AvailableSeats >= model.NumberOfPassengers)
                    .OrderBy(f => f.DepartureTime)
                    .ToListAsync();

                model.SearchResults = flights.Select(f => new FlightResultViewModel
                {
                    FlightId = f.FlightId,
                    FlightNumber = f.FlightNumber,
                    Airline = f.Airline,
                    DepartureAirportCode = f.DepartureAirport.IATACode,
                    DepartureAirportName = f.DepartureAirport.Name,
                    ArrivalAirportCode = f.ArrivalAirport.IATACode,
                    ArrivalAirportName = f.ArrivalAirport.Name,
                    DepartureTime = f.DepartureTime,
                    ArrivalTime = f.ArrivalTime,
                    Price = f.Price,
                    AvailableSeats = f.AvailableSeats
                }).ToList();
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}