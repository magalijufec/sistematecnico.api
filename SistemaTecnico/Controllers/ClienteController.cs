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

        [HttpGet("combo")]
        public async Task<IActionResult> ObtenerCombo()
        {
            var clientes = await _clienteService.ObtenerComboAsync();
            return Ok(clientes);
        }
    }
}
