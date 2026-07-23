using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.DTO;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var resultado = await _authService.LoginAsync(dto);

            if (resultado == null)
            {
                return Unauthorized(new
                {
                    mensaje = "Usuario o contraseña incorrectos"
                });
            }

            return Ok(resultado);
        }

        [Authorize]
        [HttpPost("cambiar-password")]
        public async Task<IActionResult> CambiarPassword(
            [FromBody] CambiarPasswordDTO dto)
        {
            try
            {
                var idUsuarioClaim =
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(idUsuarioClaim))
                    return Unauthorized();

                if (!int.TryParse(idUsuarioClaim, out int idUsuario))
                    return Unauthorized();

                await _authService.CambiarPasswordAsync(
                    idUsuario,
                    dto);

                return Ok(new
                {
                    mensaje = "Contraseña cambiada correctamente."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    mensaje = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    mensaje = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }
    }
}

