namespace AirlineReservation.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Store hashed passwords!
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; } // New property


        // Add other properties like FirstName, LastName, PhoneNumber, etc.
    }
}