namespace SistemaTecnico.DTO
{
    public class TrabajoResponseDto
    {
        public int Id { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinalizado { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string EstadoColor { get; set; }
        public int IdEstado { get; set; }
        public List<int> IdsTecnicos { get; set; }
        public List<string>? TecnicosAsignados { get; set; }
        public string Tecnico { get; set; } = string.Empty;
        public int IdCliente { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Sector { get; set; }
        public int IdTarea { get; set; }
        public string Tarea { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string? Comentarios { get; set; }
        public string? TrabajoRealizado { get; set; }
        public List<TrabajoFacturaDto> Facturas { get; set; } = new ();
        public bool TieneFactura { get; set; }
        public string Solicitante { get; set; }
        public List<ImagenDTO>? ImagenesSolicitud { get; set; }
        public string? Materiales { get; set; }
        public PresupuestoAprobadoDTO? PresupuestoAprobado { get; set; }
    }
}
