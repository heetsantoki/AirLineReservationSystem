using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AirLineReservation.Data;
using AirLineReservation.Models;

namespace AirLineReservation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context) => _context = context;

        public IActionResult Flights() => View(_context.Flights.ToList());

        [HttpGet]
        public IActionResult AddFlight() => View();

        [HttpPost]
        public IActionResult AddFlight(Flight flight)
        {
            _context.Flights.Add(flight);
            _context.SaveChanges();
            return RedirectToAction("Flights");
        }

        [HttpGet]
        public IActionResult EditFlight(int id) => View(_context.Flights.Find(id));

        [HttpPost]
        public IActionResult EditFlight(Flight flight)
        {
            _context.Flights.Update(flight);
            _context.SaveChanges();
            return RedirectToAction("Flights");
        }

        [HttpPost]
        public IActionResult DeleteFlight(int id)
        {
            var f = _context.Flights.Find(id);
            _context.Flights.Remove(f);
            _context.SaveChanges();
            return RedirectToAction("Flights");
        }

        public IActionResult Bookings() => View(_context.Bookings.ToList());
    }
}
