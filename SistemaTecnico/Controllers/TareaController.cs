using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareaController : ControllerBase
    {
        private readonly ITareaService _service;

        public TareaController(ITareaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.ObtenerTodasAsync());
        }

    }
}
