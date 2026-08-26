using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("no-finalizados")]
    public async Task<IActionResult> GetNoFinalizados()
    {
        var trabajos =
            await _trabajoService.ObtenerTrabajosNoFinalizadosAsync();

        return Ok(trabajos);
    }

    [HttpGet("pendiente-pago")]
    public async Task<IActionResult> GetPendientesPago()
    {
        var trabajos =
            await _trabajoService.ObtenerTrabajosPendientesPagoAsync();

        return Ok(trabajos);
    }

    [HttpGet("finalizados")]
    public async Task<IActionResult> GetFinalizados()
    {
        var trabajos =
            await _trabajoService.ObtenerTrabajosFinalizadosAsync();

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

    [Authorize(Roles = "Administrador,Sistemas")]
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] TrabajoCreateDto dto)
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

    //[Authorize(Roles = "Administrador,Tecnico")]
    //[HttpPost("{id}/imagenes")]
    //public async Task<IActionResult> SubirImagenes(int id, [FromForm] bool antes, [FromForm] List<IFormFile> archivos)
    //{
    //    await _imagenService.SubirImagenes(id, antes, archivos);
    //    return Ok();
    //}

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

    
    [Authorize(Roles = "Administrador,Tecnico")]
    [HttpPost("{id}/factura")]
    public async Task<IActionResult> SubirFactura(int id, IFormFile archivo)
    {
        try
        {
            await _trabajoService.SubirFacturaAsync(id, archivo);

            return Ok(new
            {
                mensaje = "Factura cargada correctamente"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                mensaje = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrador,Pagos,Farmacia")]
    [HttpPut("{id}/registrar-pago")]
    public async Task<IActionResult> RegistrarPago(int id)
    {
        try
        {
            var resultado = await _trabajoService.RegistrarPagoAsync(id);

            if (!resultado)
                return NotFound();

            return Ok(new
            {
                mensaje = "Pago registrado correctamente."
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPut("{id}/iniciar")]
    [Authorize(Roles = "Tecnico")]
    public async Task<IActionResult> IniciarTrabajo(int id)
    {
        try
        {
            var resultado =
                await _trabajoService.IniciarTrabajoAsync(id);

            if (!resultado)
                return NotFound();

            return Ok(new
            {
                mensaje = "Trabajo iniciado correctamente."
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPut("{id}/finalizar")]
    [Authorize(Roles = "Tecnico")]
    public async Task<IActionResult> FinalizarTrabajo(int id, TrabajoRealizadoDTO dto)
    {
        try
        {
            var resultado = await _trabajoService.FinalizarTrabajoAsync(id, dto);

            if (!resultado)
                return NotFound();

            return Ok(new
            {
                mensaje = "Trabajo enviado a revisión."
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPut("{id}/aprobar")]
    [Authorize(Roles = "Sistemas,Administrador")]
    public async Task<IActionResult> AprobarTrabajo(int id)
    {
        try
        {
            var resultado =
                await _trabajoService.AprobarTrabajoAsync(id);

            if (!resultado)
                return NotFound();

            return Ok(new
            {
                mensaje = "Trabajo aprobado correctamente."
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPut("{id}/solicitar-mejora")]
    [Authorize(Roles = "Sistemas")]
    public async Task<IActionResult> SolicitarMejora(int id,SolicitarMejoraDTO dto)
    {
        try
        {
            var resultado =
                await _trabajoService.SolicitarMejoraAsync(id, dto);

            if (!resultado)
                return NotFound();

            return Ok(new
            {
                mensaje = "Se solicitó una mejora al técnico."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpGet("{id}/informe-pdf")]
    public async Task<IActionResult> GenerarInformePdf(int id)
    {
        try
        {
            var pdf = await _trabajoService.GenerarInformePdfAsync(id);

            return File(
                pdf,
                "application/pdf",
                $"Informe-Trabajo-{id}.pdf"
            );
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                mensaje = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }
}