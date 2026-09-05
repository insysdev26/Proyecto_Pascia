using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pegasus.Core.Entities;
using Pegasus.Core.Interfaces;
using Pegasus.API.DTOs;

namespace Pegasus.API.Controllers
{
    [Authorize] // Protege todo el controlador
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : ControllerBase
    {
        private readonly IVehiculoRepository _repository;
        private readonly IWebHostEnvironment _env;

        // Inyectamos el repo y el entorno web (para guardar las fotos)
        public VehiculosController(IVehiculoRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehiculos = await _repository.GetAllAsync();
            return Ok(vehiculos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VehiculoCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string? fotoUrl = null;

            // Procesar la imagen si el usuario subió una
            if (dto.Foto != null && dto.Foto.Length > 0)
            {
                // Ruta: wwwroot/uploads
                var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Nombre único para no sobreescribir imágenes con el mismo nombre
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.Foto.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Foto.CopyToAsync(fileStream);
                }

                // Esta es la URL que guardaremos en la Base de Datos y enviaremos a Vue
                fotoUrl = $"/uploads/{uniqueFileName}";
            }

            // Mapeamos el DTO a la Entidad final
            var vehiculo = new Vehiculo
            {
                Serial = dto.Serial,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Anio = dto.Anio,
                Color = dto.Color,
                Precio = dto.Precio,
                Kilometro = dto.Kilometro,
                Sincronico = dto.Sincronico,
                Tipo = dto.Tipo,
                Estado = dto.Estado,
                FotoUrl = fotoUrl
            };

            await _repository.AddAsync(vehiculo);
            return CreatedAtAction(nameof(GetAll), new { id = vehiculo.Id }, vehiculo);
        }
    }
}