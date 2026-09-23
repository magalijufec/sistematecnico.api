namespace SistemaTecnico.DTO
{
    public class IncidenciaFinalizadaDto
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string UsuarioFinalizado { get; set; } = "-";

        public DateTime? FechaFinalizado { get; set; }

        public string Cliente { get; set; } = string.Empty;

        public string Asistencia { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;

        public string Incidencia { get; set; } = string.Empty;

        public string? Otro { get; set; }

        public string? TrabajoRealizado { get; set; }

        public bool Guardia { get; set; }
    }
}
