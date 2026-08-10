namespace SistemaTecnico.DTO
{
    public class TrabajoFinalizadoDTO
    {
        public int Id { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinalizado { get; set; }
        public DateTime? FechaPagado { get; set; }
        public string? Factura { get; set; }
        public int IdTecnico { get; set; }
        public string Tecnico { get; set; } = string.Empty;
        public int IdCliente { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public int IdTarea { get; set; }
        public string Tarea { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }
        public string? TrabajoRealizado { get; set; }
    }
}
