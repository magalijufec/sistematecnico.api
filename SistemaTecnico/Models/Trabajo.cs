namespace SistemaTecnico.Models
{
    public class Trabajo
    {
        public int Id { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinalizado { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public int TecnicoId { get; set; }
        public Usuario Tecnico { get; set; } = null!;

        public int UsuarioCreacionId { get; set; }

        public Usuario UsuarioCreacion { get; set; }

        public int EstadoId { get; set; }
        public EstadoTrabajo Estado { get; set; } = null!;

        public int TareaId { get; set; }
        public Tarea Tarea { get; set; } = null!;
        public string? Comentarios { get; set; }
        public string? TrabajoRealizado { get; set; }
        public string? Factura { get; set; }
        public DateTime? FechaPagado { get; set; }
        public IList<Imagen> Imagenes { get; set; } = new List<Imagen>();
        public ICollection<TrabajoImagenComparacion> ComparacionesImagenes { get; set; }
            = new List<TrabajoImagenComparacion>();
    }
}
