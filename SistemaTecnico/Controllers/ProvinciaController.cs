using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvinciaController : ControllerBase
    {
        private readonly IProvinciaService _service;

        public ProvinciaController(IProvinciaService service)
        {
            _service = service;
        }

        [HttpGet("combo")]
        public async Task<IActionResult> ObtenerCombo()
        {
            var provincias = await _service.ObtenerComboAsync();

            return Ok(provincias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var provincia =
                await _service.ObtenerPorIdAsync(id);

            if (provincia == null)
                return NotFound();

            return Ok(provincia);
        }
    }
}
