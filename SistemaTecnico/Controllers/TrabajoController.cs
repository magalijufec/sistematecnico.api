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

    [HttpGet("solicitudes")]
    public async Task<IActionResult> GetSolicitudesDeTrabajo()
    {
        var trabajos = await _trabajoService.ObtenerSolicitudesDeTrabajoAsync();
        return Ok(trabajos);
    }

    [HttpGet("no-finalizados")]
    public async Task<IActionResult> GetNoFinalizados()
    {
        var trabajos = await _trabajoService.ObtenerTrabajosNoFinalizadosAsync();

        return Ok(trabajos);
    }

    [HttpGet("pendiente-pago")]
    public async Task<IActionResult> GetPendientesPago()
    {
        var trabajos = await _trabajoService.ObtenerTrabajosPendientesPagoAsync();

        return Ok(trabajos);
    }

    [HttpGet("pagados")]
    public async Task<IActionResult> GetPagados()
    {
        var trabajos = await _trabajoService.ObtenerTrabajosPagadosAsync();

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

    [Authorize(Roles = "Administrador,Sistemas,Monitoreo,Mantenimiento,Farmacia")]
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

    [HttpPut("{idTrabajo:int}/decision-solicitud")]
    public async Task<IActionResult>RevisarSolicitud(int idTrabajo, [FromBody] RevisarSolicitudDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var actualizado = await _trabajoService.CambiarEstadoTrabajoAsync(idTrabajo, dto.Aprobado);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{idTrabajo:int}/materiales-enviados")]
    public async Task<IActionResult> MaterialesEnviados(int idTrabajo)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var actualizado = await _trabajoService.MaterialesEnviadosAsync(idTrabajo);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{idTrabajo:int}/asignar-tecnicos")]
    public async Task<IActionResult> AsignarTecnicos(int idTrabajo, List<int> tecnicosIds)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var actualizado = await _trabajoService.AsignarTecnicosAsync(idTrabajo, tecnicosIds);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{idTrabajo:int}/materiales")]
    public async Task<IActionResult> CargarMateriales(int idTrabajo, [FromBody] MaterialesDTO dto)
    {
        var actualizado = await _trabajoService.CargarMaterialesAsync(idTrabajo, dto.Materiales);

        if (!actualizado) return NotFound();

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
    [HttpPost("{id:int}/facturas")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SubirFacturas(int id, [FromForm] IFormFile[] archivos)
    {
        try
        {
            await _trabajoService.SubirFacturasAsync(id, archivos);     

            return Ok(new
            {                
                mensaje = "Facturas cargadas correctamente"
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
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "No se pudieron cargar las facturas."
                }
            );
        }
    }

    [Authorize(Roles = "Administrador,Pagos,Farmacia")]
    [HttpPut("{idTrabajo:int}/facturas/{idFactura:int}/registrar-pago")]
    public async Task<IActionResult> RegistrarPagoFactura(int idTrabajo, int idFactura)
    {
        try
        {
            var resultado =
                await _trabajoService
                    .RegistrarPagoAsync(
                        idTrabajo,
                        idFactura
                    );

            return Ok(resultado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                mensaje = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    mensaje = ex.Message
                }
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje =
                        "No se pudo registrar el pago de la factura."
                }
            );
        }
    }       

    [HttpPut("{id}/finalizar")]
    [Authorize(Roles = "Tecnico")]
    public async Task<IActionResult> FinalizarTrabajo(int id, TrabajoRealizadoDTO dto)
    {
        try
        {
            if (dto.FechaFin < dto.FechaInicio)
            {
                return BadRequest(new
                {
                    mensaje = "La fecha de finalización no puede ser anterior a la fecha de inicio."
                });
            }

            var resultado = await _trabajoService.PendienteAprobacionTrabajoAsync(id, dto);

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
    [Authorize(Roles = "Sistemas,Mantenimiento,Monitoreo,Administrador")]
    public async Task<IActionResult> AprobarTrabajo(int id)
    {
        try
        {
            var resultado = await _trabajoService.AprobarTrabajoAsync(id);

            if (!resultado)
                return NotFound();

            return Ok(new { mensaje = "Trabajo aprobado correctamente."});
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
    [Authorize(Roles = "Sistemas,Mantenimiento,Monitoreo")]
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

            return File(pdf, "application/pdf", $"Informe-Trabajo-{id}.pdf");
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