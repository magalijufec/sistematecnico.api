using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.DTO;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoTrabajoController : ControllerBase
    {
        private readonly IEstadoService _service;

        public EstadoTrabajoController(IEstadoService service)
        {
            _service = service;
        }

        [HttpGet("combo")]
        public async Task<ActionResult<IEnumerable<ComboDTO>>> Combo()
        {
            return Ok(await _service.ObtenerComboAsync());
        }

        [HttpGet("siguientes/{idTrabajo}")]
        public async Task<IActionResult> ObtenerEstadosSiguientes(int idTrabajo)
        {
            var estados = await _service.ObtenerEstadosSiguientes(idTrabajo);
            return Ok(estados);
        }
    }
}
