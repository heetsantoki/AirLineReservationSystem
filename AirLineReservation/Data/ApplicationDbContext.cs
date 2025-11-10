using AirLineReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace AirLineReservation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Booking> Bookings { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed a few airports & flights for demo
            builder.Entity<Airport>().HasData(
                new Airport { IATA = "DEL", Name = "Indira Gandhi Intl", City = "Delhi" },
                new Airport { IATA = "BOM", Name = "Chhatrapati Shivaji Intl", City = "Mumbai" },
                new Airport { IATA = "BLR", Name = "Bengaluru Intl", City = "Bengaluru" }
            );

            // Note: sample flights use Ids; departure/arrival times will be created at runtime for search but
            // you can seed static flights if you want.
        }
    }
}
