
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "The username is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "The email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The password is required")]
        public string? Password { get; set; }
    }
}
