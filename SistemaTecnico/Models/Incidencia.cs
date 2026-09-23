using System.ComponentModel.DataAnnotations;

namespace SistemaTecnico.Models
{
    public class Incidencia
    {
        public int Id { get; set; }

        // Quién registró la incidencia
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public int DestinoId { get; set; }
        public Destino Destino { get; set; }
        public int TareaId { get; set; }
        public Tarea Tarea { get; set; }
        public int AsistenciaId { get; set; }
        public Asistencia Asistencia { get; set; }
        public int EstadoIncidenciaId { get; set; }
        public EstadoIncidencia EstadoIncidencia { get; set; }
        [MaxLength(500)]
        public string? Otro { get; set; }
        [MaxLength(4000)]
        public string? TrabajoRealizado { get; set; }
        // Quién resolvió la incidencia
        public int? UsuarioFinalizadoId { get; set; }
        public Usuario? UsuarioFinalizado { get; set; }
        public DateTime? FechaFinalizado { get; set; }
        [MaxLength(4000)]
        public string? Comentario { get; set; }
        public bool Guardia { get; set; }
    }

    public class EstadoIncidencia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class Asistencia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class Destino
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

}
