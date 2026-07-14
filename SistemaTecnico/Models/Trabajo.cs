namespace SistemaTecnico.Models
{
    public class Trabajo
    {
        public int Id { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public DateTime? FechaTrabajo { get; set; }

        public Usuario Tecnico { get; set; } = null!;

        public Cliente Cliente { get; set; }

        //public Sector? Sector { get; set; }

        //public int IdTarea { get; set; }

        public Tarea Tarea { get; set; } = null!;

        public string? Comentarios { get; set; }

        public string? TrabajoRealizado { get; set; }

        public EstadoTrabajo Estado { get; set; } = null!;

        public string? Factura { get; set; }

        public DateTime? FechaPagado { get; set; }

        public DateTime FechaAlta { get; set; }

        public IList<Imagen> Imagenes { get; set; } = new List<Imagen>();

        //public ICollection<HistorialEstado> HistorialEstados { get; set; } = new List<HistorialEstado>();
    }
}
