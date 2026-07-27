namespace SistemaTecnico.Models
{
    public class TrabajoImagenComparacion
    {
        public int Id { get; set; }
        public int TrabajoId { get; set; }
        public Trabajo Trabajo { get; set; } = null!;
        public Imagen? ImagenAntes { get; set; }
        public int? ImagenAntesId { get; set; }
        public Imagen? ImagenDespues { get; set; }
        public int? ImagenDespuesId { get; set; }
    }
}
