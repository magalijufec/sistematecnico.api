using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.DTO;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrabajoController : ControllerBase
{
    private readonly ITrabajoService _trabajoService;
    private readonly IImagenService _imagenService;

    public TrabajoController(ITrabajoService trabajoService, IImagenService imagenService)
    {
        _trabajoService = trabajoService;
        _imagenService = imagenService;
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

    [HttpPost("{id}/imagenes")]
    public async Task<IActionResult> SubirImagenes(
        int id,
        [FromForm] bool antes,
        [FromForm] List<IFormFile> archivos)
    {
        await _imagenService.SubirImagenes(id, antes, archivos);

        return Ok();
    }

    [HttpGet("{id}/imagenes")]
    public async Task<IActionResult> Obtener(int id)
    {
        return Ok(await _imagenService.ObtenerPorTrabajo(id));
    }

    [HttpDelete("imagenes/{idImagen}")]
    public async Task<IActionResult> Eliminar(int idImagen)
    {
        await _imagenService.EliminarImagenAsync(idImagen);

        return NoContent();
    }

    [HttpPut("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(
        int id,
        CambiarEstadoDTO dto)
    {
        await _trabajoService.CambiarEstadoAsync(id, dto);

        return NoContent();
    }

    [HttpPut("{id}/trabajo-realizado")]
    public async Task<IActionResult> GuardarTrabajoRealizado(
    int id,
    TrabajoRealizadoDTO dto)
    {
        await _trabajoService.GuardarTrabajoRealizado(id, dto);
        return NoContent();
    }
}