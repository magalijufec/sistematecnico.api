namespace SistemaTecnico.Models
{
    public class EstadoTrabajo
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; }

        //public IList<Trabajo> Trabajos { get; set; } = new List<Trabajo>();

        //public ICollection<HistorialEstado> HistorialEstados { get; set; } = new List<HistorialEstado>();
    }
}
