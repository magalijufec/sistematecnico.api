using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.DTO;
using SistemaTecnico.Models;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _service.ObtenerTodosAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var usuario = await _service.ObtenerPorIdAsync(id);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsuarioDTO usuario)
        {
            await _service.CrearAsync(usuario);

            return Ok(usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UsuarioDTO usuario)
        {
            var actualizado = await _service.ActualizarAsync(id, usuario);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.EliminarAsync(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}
