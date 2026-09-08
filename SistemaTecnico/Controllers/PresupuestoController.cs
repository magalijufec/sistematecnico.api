using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.DTO;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresupuestoController : ControllerBase
    {

        private readonly IPresupuestoService _presupuestoService;
        public PresupuestoController(IPresupuestoService presupuestoService)
        {
            _presupuestoService = presupuestoService;
        }

        [HttpGet("trabajo/{idTrabajo:int}")]
        [HttpGet]
        public async Task<IActionResult> GetByTrabjo(int idTrabajo)
        {
            var presupuestos = await _presupuestoService.ObtenerPresupuestosByTrabajoAsync(idTrabajo);

            return Ok(presupuestos);
        }

        [HttpPost("{idTrabajo:int}")]
        public async Task<IActionResult> Agregar(int idTrabajo, PresupuestoDTO presupuestoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var presupuesto = await _presupuestoService.CrearAsync(presupuestoDto);

            if (presupuesto == null)
                return NotFound();

            return Ok(new
            {
                mensaje = "Presupuesto cargado correctamente."
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] PresupuestoDecisionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actualizado = await _presupuestoService.ActualizarAsync(id, dto);

            if (actualizado == null)
                return NotFound();

            return NoContent();
        }
    }
}
