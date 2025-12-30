using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
        public class LoginUserDTO
        {
                [Required(ErrorMessage = "Email is required")]
                [EmailAddress(ErrorMessage = "Invalid email format")]
                [StringLength(100)]
                public string Email { get; set; } = string.Empty;
                [Required(ErrorMessage = "Password is required")]
                public string Password { get; set; } = string.Empty;

        }
}