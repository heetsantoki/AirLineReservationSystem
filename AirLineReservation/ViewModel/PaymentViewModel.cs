using System.ComponentModel.DataAnnotations;

namespace AirLineReservation.ViewModel
{
    public class PaymentViewModel
    {
        public int FlightId { get; set; }
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Passenger name is required")]
        public string PassengerName { get; set; }

        public int Passengers { get; set; }
        public int PassengerAge { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Card Holder Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters allowed")]
        public string CardHolder { get; set; }

        [Required(ErrorMessage = "Card Number is required")]
        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Card number must be 16 digits")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "Expiry must be in MM/YY format")]
        public string Expiry { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "CVV must be 3 digits")]
        public string Cvv { get; set; }
    }
}
