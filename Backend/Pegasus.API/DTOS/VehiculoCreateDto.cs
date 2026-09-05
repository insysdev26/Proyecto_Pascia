using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Pegasus.API.DTOs
{
    public class VehiculoCreateDto
    {
        [Required(ErrorMessage = "El Serial/VIN es obligatorio")]
        public string Serial { get; set; } = string.Empty;

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        public int Anio { get; set; }

        public string Color { get; set; } = "No especificado";

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public int Kilometro { get; set; }

        public bool Sincronico { get; set; }

        public string Tipo { get; set; } = "Sedán";

        public string Estado { get; set; } = "Disponible";

        // Aquí recibimos la imagen física desde Vue
        public IFormFile? Foto { get; set; }
    }
}