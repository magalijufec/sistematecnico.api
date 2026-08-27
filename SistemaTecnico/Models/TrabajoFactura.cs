namespace SistemaTecnico.Models
{
    public class TrabajoFactura
    {
        public int Id { get; set; }
        public int TrabajoId { get; set; }
        public Trabajo Trabajo { get; set; } = null!;
        public string RutaArchivo { get; set; } = string.Empty;
        public DateTime FechaCarga { get; set; }
        public DateTime? FechaPagado { get; set; }
    }
}