using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return Ok(clientes);
        }

        [HttpGet("combo")]
        public async Task<IActionResult> ObtenerCombo()
        {
            var clientes = await _clienteService.ObtenerComboAsync();
            return Ok(clientes);
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
