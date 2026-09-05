using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pegasus.Core.DTOs;
using Pegasus.Core.Entities;
using Pegasus.Core.Interfaces;
using Pegasus.Infrastructure.Data;

namespace Pegasus.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var emailLimpio = dto.Email.Trim().ToLower();

            if (await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == emailLimpio))
                return false;

            using var sha256 = SHA256.Create();
            var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));

            // Asignamos "Cliente" por defecto si no viene rol en el DTO
            var rolAsignado = string.IsNullOrWhiteSpace(dto.Rol) ? "Cliente" : dto.Rol;

            var usuario = new Usuario
            {
                NombreCompleto = string.IsNullOrWhiteSpace(dto.Nombre) ? emailLimpio.Split('@')[0] : dto.Nombre,
                Email = emailLimpio,
                PasswordHash = passwordHash,
                Rol = rolAsignado
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var emailLimpio = dto.Email.Trim().ToLower();

            using var sha256 = SHA256.Create();
            var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == emailLimpio && u.PasswordHash == passwordHash);

            if (usuario == null) return null;

            return GenerateJwtToken(usuario);
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "SuperSecretKeyPegasus2026_MustBeLongEnough!";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Validamos que ningún claim reciba un string nulo
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto ?? usuario.Email),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Cliente")
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}