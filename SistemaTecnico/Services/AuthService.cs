using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaTecnico.Data;
using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IPasswordHasher<Usuario> passwordHasher,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginDTO dto)
        {
            var usuario = await _context.Usuarios
                .Include(x => x.Perfil)
                .FirstOrDefaultAsync(x =>
                    x.UserName == dto.UserName &&
                    x.Activo);

            if (usuario == null)
                return null;

            bool passwordValida = BCrypt.Net.BCrypt.Verify(
                dto.Password,
                usuario.PasswordHash
            );

            if (!passwordValida)
                return null;

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    usuario.UserName),

                new Claim(
                    ClaimTypes.Role,
                    usuario.Perfil.Nombre)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            var tokenString =
                new JwtSecurityTokenHandler()
                    .WriteToken(token);

            return new LoginResponseDTO
            {
                Token = tokenString,
                IdUsuario = usuario.Id,
                UserName = usuario.UserName,
                NombreApellido = usuario.NombreApellido,
                IdPerfil = usuario.Perfil.Id,
                Perfil = usuario.Perfil.Nombre
            };
        }

        public async Task CambiarPasswordAsync(
            int idUsuario,
            CambiarPasswordDTO dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Id == idUsuario);

            if (usuario == null)
                throw new KeyNotFoundException(
                    "Usuario no encontrado.");

            // Verificar contraseña actual
            bool passwordCorrecta =
                BCrypt.Net.BCrypt.Verify(
                    dto.PasswordActual,
                    usuario.PasswordHash);

            if (!passwordCorrecta)
                throw new UnauthorizedAccessException(
                    "La contraseña actual es incorrecta.");

            // Verificar nueva contraseña
            if (dto.PasswordNueva != dto.ConfirmarPassword)
                throw new ArgumentException(
                    "Las nuevas contraseñas no coinciden.");

            // Evitar reutilizar la misma contraseña
            if (BCrypt.Net.BCrypt.Verify(
                dto.PasswordNueva,
                usuario.PasswordHash))
            {
                throw new ArgumentException(
                    "La nueva contraseña debe ser diferente a la actual.");
            }

            // Generar nuevo hash
            usuario.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    dto.PasswordNueva);

            await _context.SaveChangesAsync();
        }
    }
}
