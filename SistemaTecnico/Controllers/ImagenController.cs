using Microsoft.AspNetCore.Mvc;
using SistemaTecnico.Services;

namespace SistemaTecnico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagenController : ControllerBase
    {
        private readonly IImagenService _service;

        public ImagenController(IImagenService service)
        {
            _service = service;
        }

        [HttpPost("{id}/imagenes")]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> SubirImagenes(
        int id,
        [FromForm] bool antes,
        [FromForm] List<IFormFile> archivos)
        {
            try
            {
                await _service.SubirImagenes(
                    id,
                    antes,
                    archivos
                );

                return Ok(new
                {
                    mensaje = "Imágenes cargadas correctamente."
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
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        mensaje =
                            "Error al guardar las imágenes.",
                        detalle = ex.Message
                    }
                );
            }
        }


        [HttpGet("{id}/imagenes")]
        public async Task<IActionResult> Obtener(int id)
        {
            return Ok(await _service.ObtenerPorTrabajo(id));
        }

        [HttpDelete("imagenes/{idImagen}")]
        public async Task<IActionResult> Eliminar(int idImagen)
        {
            await _service.EliminarImagenAsync(idImagen);

            return NoContent();
        }
    }
}
