namespace SistemaTecnico.DTO
{
    public class TrabajoImagenComparacionDTO
    {
        public int Id { get; set; }
        public int TrabajoId { get; set; }
        public ImagenResponseDto? ImagenAntes { get; set; }
        public ImagenResponseDto? ImagenDespues { get; set; }
    }
}
