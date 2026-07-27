using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [ApiController]
    [Route("api/trabajo-imagen-comparacion")]
    //[Authorize]
    public class TrabajoImagenComparacionController : ControllerBase
    {
        private readonly ITrabajoImagenComparacionService _service;

        public TrabajoImagenComparacionController(ITrabajoImagenComparacionService service)
        {
            _service = service;
        }

        [HttpGet("trabajo/{idTrabajo}")]
        public async Task<IActionResult> ObtenerPorTrabajo(int idTrabajo)
        {
            var resultado = await _service.ObtenerPorTrabajoAsync(idTrabajo);

            return Ok(resultado);
        }

        [HttpPost("trabajo/{idTrabajo}")]
        public async Task<IActionResult>Crear(int idTrabajo)
        {
            var resultado = await _service.CrearAsync(idTrabajo);

            return Ok(resultado);
        }

        [HttpPost("{idComparacion}/antes")]
        public async Task<IActionResult> SubirAntes(int idComparacion, IFormFile archivo)
        {
            await _service.SubirImagenAntesAsync(
                    idComparacion,
                    archivo);

            return Ok();
        }

        [HttpPost("{idComparacion}/despues")]
        public async Task<IActionResult> SubirDespues(int idComparacion, IFormFile archivo)
        {
            await _service.SubirImagenDespuesAsync(idComparacion, archivo);
            return Ok();
        }

        [HttpDelete("{idComparacion}")]
        public async Task<IActionResult> Eliminar(int idComparacion)
        {
            await _service.EliminarAsync(idComparacion);

            return NoContent();
        }
    }
}
