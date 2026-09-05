using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pegasus.Core.Entities
{
    public class Vehiculo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Serial/VIN es obligatorio")]
        [StringLength(50)]
        public string Serial { get; set; } = string.Empty;

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Range(1900, 2027, ErrorMessage = "Año fuera de rango válido")]
        public int Anio { get; set; }

        [StringLength(30)]
        public string Color { get; set; } = "No especificado";

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 10000000.00, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal Precio { get; set; }

        [Range(0, 1000000, ErrorMessage = "Kilometraje inválido")]
        public int Kilometro { get; set; }

        public bool Sincronico { get; set; }

        [StringLength(50)]
        public string Tipo { get; set; } = "Sedán";

        [StringLength(30)]
        public string Estado { get; set; } = "Disponible"; // Disponible, Reservado, Vendido

        public string? FotoUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}