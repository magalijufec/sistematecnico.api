using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.DTO;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cliente =
                await _clienteService.ObtenerPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }


        [HttpGet("combo")]
        public async Task<IActionResult> ObtenerCombo()
        {
            var clientes = await _clienteService.ObtenerComboAsync();
            return Ok(clientes);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteDTO dto)
        {
            var cliente =
                await _clienteService.CrearAsync(dto);

            return Ok(cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClienteDTO dto)
        {
            var actualizado =
                await _clienteService.ActualizarAsync(
                    id,
                    dto
                );

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        [HttpGet("provincia/{provinciaId}/ciudad/{ciudadId}")]
        public async Task<IActionResult> ObtenerPorProvinciaCiudad(
        int provinciaId,
        int ciudadId)
        {
            var clientes =
                await _clienteService
                    .ObtenerPorProvinciaCiudadAsync(
                        provinciaId,
                        ciudadId
                    );

            return Ok(clientes);
        }
    }
}
