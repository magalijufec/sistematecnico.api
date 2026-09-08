namespace SistemaTecnico.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public int SectorId { get; set; }
        public Sector Sector { get; set; }
    }
}
