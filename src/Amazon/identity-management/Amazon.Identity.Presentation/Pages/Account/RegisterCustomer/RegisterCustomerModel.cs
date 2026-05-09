using System.ComponentModel.DataAnnotations;

namespace VidGuard.Platform.Authentication.Pages.Account.RegisterCustomer
{
    public class RegisterCustomerModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        [Required]
        public string PhoneNumber { get; set; }

        public string? ReturnUrl { get; set; }

        public string? Button { get; set; }
    }
    
    public class RegisterCustomerResult
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
