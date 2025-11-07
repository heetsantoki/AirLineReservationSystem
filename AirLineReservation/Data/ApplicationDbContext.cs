using Microsoft.EntityFrameworkCore;
using AirlineReservation.Models; // Ensure this is present
using System.Collections.Generic; // For ICollection

namespace AirlineReservation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        // public DbSet<Booking> Bookings { get; set; } // Will add this later

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships for Flight and Airport
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DepartureAirport)
                .WithMany(a => a.DepartingFlights)
                .HasForeignKey(f => f.DepartureAirportId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.ArrivalAirport)
                .WithMany(a => a.ArrivingFlights)
                .HasForeignKey(f => f.ArrivalAirportId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            base.OnModelCreating(modelBuilder);
        }
    }
}