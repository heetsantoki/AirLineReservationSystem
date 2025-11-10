using System.ComponentModel.DataAnnotations;

namespace AirLineReservation.ViewModel
{
    public class RegisterViewModel
    {
        [Required, StringLength(100)]
        public string Username { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
