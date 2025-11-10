using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirLineReservation.Data;
using AirLineReservation.Models;      // your EF entities
using AirLineReservation.ViewModel;   // your SearchResultsModel + FlightViewModel

namespace AirLineReservation.Controllers
{
    public class FlightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Optional landing page: /Flight
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Views/Flight/Index.cshtml (optional)
        }

        // 👇 THIS is the endpoint your URL needs: /Flight/SearchResults?from=..&to=..&date=..&passengers=..
        [HttpGet("Flight/SearchResults")]
        public IActionResult SearchResults(string from, string to, string date, int passengers)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return Content("From and To are required.");
            }

            var flights = _context.Flights
                .Where(f => f.From == from && f.To == to && f.IsActive == true)
                .OrderBy(f => f.DepartureTime)
                .ToList();  // <-- Notice: We return real Flight model, not ViewModel

            var model = new SearchResultsModel
            {
                From = from,
                To = to,
                Date = date,
                Passengers = passengers,
                Flights = flights // <-- This matches the ViewModel type
            };

            return View(model);
        }

    }
}
