using System.ComponentModel.DataAnnotations;

namespace Pegasus.Core.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string Rol { get; set; } = "Cliente";
    }
}