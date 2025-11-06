using System; // For DateTime
using System.Diagnostics;
using System.Linq; // For .Where(), .Select()
using System.Threading.Tasks; // For async/await
using AirlineReservation.Data;
using AirlineReservation.Models;
using AirLineReservation.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // For .Include() and async queries


namespace AirlineReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Inject DbContext

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
                // Optionally pre-populate airports for dropdowns if you use them
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
            // Re-populate AvailableAirports for the dropdown if needed on postback
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
                // Find airport IDs based on IATA codes
                var departureAirport = await _context.Airports.FirstOrDefaultAsync(a => a.IATACode == model.DepartureAirportIATA);
                var arrivalAirport = await _context.Airports.FirstOrDefaultAsync(a => a.IATACode == model.ArrivalAirportIATA);

                if (departureAirport == null || arrivalAirport == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid departure or arrival airport code.");
                    return View(model);
                }

                // Query for flights
                var flights = await _context.Flights
                    .Include(f => f.DepartureAirport)
                    .Include(f => f.ArrivalAirport)
                    .Where(f => f.DepartureAirportId == departureAirport.AirportId &&
                                f.ArrivalAirportId == arrivalAirport.AirportId &&
                                f.DepartureTime.Date == model.DepartureDate.Date && // Match date only
                                f.AvailableSeats >= model.NumberOfPassengers)
                    .OrderBy(f => f.DepartureTime)
                    .ToListAsync();

                // Map Flight entities to FlightResultViewModel
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}