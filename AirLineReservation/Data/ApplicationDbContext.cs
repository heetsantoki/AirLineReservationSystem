using Microsoft.EntityFrameworkCore;
using AirlineReservation.Models; // Ensure this is present

namespace AirlineReservation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Airport> Airports { get; set; } // Add this
        public DbSet<Flight> Flights { get; set; }   // Add this
        // You'll add DbSet for Bookings later

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships (optional, EF Core often infers, but explicit is good)
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DepartureAirport)
                .WithMany(a => a.DepartingFlights)
                .HasForeignKey(f => f.DepartureAirportId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete on airport if flights exist

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.ArrivalAirport)
                .WithMany(a => a.ArrivingFlights)
                .HasForeignKey(f => f.ArrivalAirportId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete on airport if flights exist

            base.OnModelCreating(modelBuilder);
        }
    }
}