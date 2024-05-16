using System.ComponentModel.DataAnnotations;

namespace Catalog.Identity
{
    public class RegistrationRequest
    {
        [Required]
        [MinLength(5), MaxLength(20)]
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? PasswordRepeat { get; set; }
    }
}
