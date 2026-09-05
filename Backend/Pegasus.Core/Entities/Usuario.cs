using System.ComponentModel.DataAnnotations;

namespace Pegasus.Core.Entities
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = "Cliente"; // Roles: Cliente, Vendedor, Gerente

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}