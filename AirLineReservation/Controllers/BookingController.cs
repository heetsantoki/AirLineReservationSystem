using AirLineReservation.Data;
using AirLineReservation.Models;
using AirLineReservation.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace AirLineReservation.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Select(int id)
        {
            var flight = _context.Flights.FirstOrDefault(f => f.Id == id);
            if (flight == null) return NotFound();

            var model = new BookingViewModel
            {
                FlightId = flight.Id,
                Airline = flight.Airline,
                FlightNumber = flight.FlightNumber,
                From = flight.From,
                To = flight.To,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Price = (decimal)flight.Price
            };

            return View("Book", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm(BookingViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Book", model);

            // ✅ Re-fetch flight price from DB to ensure correctness
            var flight = _context.Flights.FirstOrDefault(f => f.Id == model.FlightId);
            if (flight == null) return NotFound();

            // ✅ Calculate total price
            model.Price = (decimal)flight.Price * model.Passengers;

            return RedirectToAction("Payment", new
            {
                flightId = model.FlightId,
                price = model.Price,
                passengerName = model.PassengerName,
                passengerAge = model.PassengerAge,
                gender = model.Gender,
                passengers = model.Passengers
            });
        }


        [HttpGet]
        [Authorize]
        public IActionResult Payment(int flightId, decimal price, string passengerName, int passengers, int passengerAge, string gender)
        {
            var vm = new PaymentViewModel
            {
                FlightId = flightId,
                Price = price,
                PassengerName = passengerName,
                Passengers = passengers,
                PassengerAge = passengerAge,
                Gender = gender
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Payment(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            var booking = new Booking
            {
                UserId = userId,
                FlightId = model.FlightId,
                PassengerName = model.PassengerName,
                PassengerAge = model.PassengerAge,
                Gender = model.Gender,
                Passengers = model.Passengers,
                Price = model.Price * model.Passengers  
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("Success", new { id = booking.Id });
        }

        public IActionResult Success(int id)
        {
            var booking = _context.Bookings
                .Where(b => b.Id == id)
                .Join(_context.Flights,
                      b => b.FlightId,
                      f => f.Id,
                      (b, f) => new
                      {
                          b.Id,
                          b.PassengerName,
                          b.PassengerAge,
                          b.Gender,
                          b.Passengers,
                          b.Price,
                          f.Airline,
                          f.FlightNumber,
                          f.From,
                          f.To,
                          f.DepartureTime,
                          f.ArrivalTime
                      })
                .FirstOrDefault();

            if (booking == null) return NotFound();

            return View("Success", booking);
        }

        [Authorize]
        public IActionResult MyBookings()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            var bookings = _context.Bookings
                .Where(b => b.UserId == userId)
                .Join(_context.Flights,
                    b => b.FlightId,
                    f => f.Id,
                    (b, f) => new MyBookingsViewModel
                    {
                        BookingId = b.Id,
                        PassengerName = b.PassengerName,
                        Airline = f.Airline,
                        FlightNumber = f.FlightNumber,
                        From = f.From,
                        To = f.To,
                        DepartureTime = f.DepartureTime,
                        ArrivalTime = f.ArrivalTime,
                        Price = b.Price
                    }).ToList();

            return View(bookings);
        }
    }
}
