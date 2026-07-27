using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CiudadController : ControllerBase
    {
        private readonly ICiudadService _service;

        public CiudadController(ICiudadService service)
        {
            _service = service;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> ObtenerCombo()
        {
            var ciudades =
                await _service.ObtenerComboAsync();

            return Ok(ciudades);
        }

        [HttpGet("provincia/{provinciaId}")]
        public async Task<IActionResult> ObtenerPorProvincia(int provinciaId)
        {
            var ciudades = await _service.ObtenerPorProvinciaAsync(provinciaId);

            return Ok(ciudades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var ciudad =
                await _service.ObtenerPorIdAsync(id);

            if (ciudad == null)
                return NotFound();

            return Ok(ciudad);
        }
    }
}
