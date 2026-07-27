using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilService _perfilService;

        public PerfilController(IPerfilService perfilService)
        {
            _perfilService = perfilService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var perfiles = await _perfilService.ObtenerComboAsync();

            return Ok(perfiles);
        }
    }
}
