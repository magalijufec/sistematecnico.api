namespace SistemaTecnico.DTO
{
    public class PresupuestoDetalleDTO
    {
        public int Id { get; set; }

        public string? RutaArchivo { get; set; }

        public string? Descripcion { get; set; }

        public string Tecnico { get; set; } = string.Empty;

        public DateTime FechaCarga { get; set; }

        public int TrabajoId { get; set; }

        public int EstadoId { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}
