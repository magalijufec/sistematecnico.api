namespace SistemaTecnico.Models
{
    public class Presupuesto
    {
        public int Id { get; set; }
        public DateTime FechaCarga { get; set; }
        public int TecnicoId { get; set; }
        public Usuario Tecnico { get; set; }
        public int? UsuarioDecisionId { get; set; }
        public Usuario? UsuarioDecision { get; set; }
        public DateTime? FechaDecision { get; set; }
        public EstadoPresupuesto Estado { get; set; }
        public int EstadoId { get; set; }
        public string? MotivoRechazo { get; set; }
        public string? Descripcion { get; set; }
        public string? RutaArchivo { get; set; } 
        public int TrabajoId { get; set; }
        public Trabajo Trabajo { get; set; }
    }

    public class EstadoPresupuesto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

    public static class EstadosPresupuesto
    {
        public const int EnRevision = 1;
        public const int Aprobado = 2;
        public const int Rechazado = 3;
        public const int AprobacionRevocada = 4;
        public const int RetiradoPorTecnico = 5;
    }
}
