using System;
using System.ComponentModel.DataAnnotations;

namespace AirlineReservation.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)] // Store hashed passwords
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow; // Default to current UTC time
    }
}