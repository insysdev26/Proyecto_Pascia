using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pegasus.Core.DTOs;
using Pegasus.Core.Interfaces;
using Pegasus.Infrastructure.Data;

namespace Pegasus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public AuthController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _authService.RegisterAsync(dto);
            if (!success) return BadRequest(new { Message = "El correo ya está registrado." });

            return Ok(new { Message = "Usuario registrado exitosamente." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _authService.LoginAsync(dto);
            if (token == null) return Unauthorized(new { Message = "Credenciales inválidas." });

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.Trim().ToLower());

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, 
                Expires = DateTimeOffset.UtcNow.AddHours(8)
            });

            return Ok(new
            {
                Message = "Inicio de sesión exitoso.",
                User = new
                {
                    nombre = usuario?.NombreCompleto ?? dto.Email.Split('@')[0],
                    email = usuario?.Email ?? dto.Email,
                    rol = usuario?.Rol ?? "Cliente"
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { Message = "Sesión cerrada." });
        }
    }
}