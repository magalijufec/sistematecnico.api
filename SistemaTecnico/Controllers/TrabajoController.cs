using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.DTO;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrabajoController : ControllerBase
{
    private readonly ITrabajoService _trabajoService;

    public TrabajoController(ITrabajoService trabajoService)
    {
        _trabajoService = trabajoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var trabajos = await _trabajoService.ObtenerTodosAsync();
        return Ok(trabajos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var trabajo = await _trabajoService.ObtenerPorIdAsync(id);

        if (trabajo == null)
            return NotFound();

        return Ok(trabajo);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TrabajoCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var trabajo = await _trabajoService.CrearAsync(dto);

        return CreatedAtAction(
            nameof(Get),
            new { id = trabajo.Id },
            trabajo);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] TrabajoUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var actualizado = await _trabajoService.ActualizarAsync(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _trabajoService.EliminarAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}