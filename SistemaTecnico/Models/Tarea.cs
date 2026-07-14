namespace SistemaTecnico.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        //public ICollection<Trabajo> Trabajos { get; set; } = new List<Trabajo>();
    }
}
