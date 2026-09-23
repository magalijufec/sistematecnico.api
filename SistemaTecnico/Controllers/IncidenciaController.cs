using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.DTO;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class IncidenciasController : ControllerBase
    {
        private readonly IIncidenciaService _service;

        public IncidenciasController(IIncidenciaService service)
        {
            _service = service;  
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerPendientes()
        {
            var resultado = await _service.ObtenerPendientesAsync();

            return Ok(resultado);
        }

        [HttpGet("finalizadas")]
        public async Task<IActionResult> ObtenerFinalizadas()
        {
            var resultado = await _service.ObtenerFinalizadasAsync();

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearIncidenciaDto dto)
        {
            await _service.CrearAsync(dto);
            return Ok();
        }

        [HttpPut("{id}/finalizar")]
        public async Task<IActionResult> Finalizar(int id, [FromBody] FinalizarIncidenciaDto dto)
        {
            try
            {
                await _service.FinalizarAsync(id, dto.TrabajoRealizado);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("catalogos")]
        public async Task<IActionResult> ObtenerCatalogos()
        {
            var resultado = await _service.ObtenerCatalogosAsync();

            return Ok(resultado);
        }
    }
}
