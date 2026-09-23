namespace SistemaTecnico.DTO
{
    public class IncidenciaPendienteDto
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string Cliente { get; set; } = string.Empty;

        public string Asistencia { get; set; } = string.Empty;

        public string Incidencia { get; set; } = string.Empty;

        public string? Otro { get; set; }

        public string? Comentario { get; set; }

        public bool Guardia { get; set; }
    }
}
