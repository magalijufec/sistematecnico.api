using SistemaTecnico.Models;

namespace SistemaTecnico.DTO
{
    public class PresupuestoDTO
    {
        public int TecnicoId { get; set; }
        public int? UsuarioDecisionId { get; set; }
        public string? Descripcion { get; set; }
        public IFormFile Archivo { get; set; }
        public int TrabajoId { get; set; }
    }
}
