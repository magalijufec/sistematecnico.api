public class TrabajoFacturaDto
{
    public int Id { get; set; }
    public string RutaArchivo { get; set; } = string.Empty;
    public DateTime FechaCarga { get; set; }
    public DateTime? FechaPagado { get; set; }
}