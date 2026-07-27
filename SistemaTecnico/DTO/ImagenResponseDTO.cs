namespace SistemaTecnico.DTO
{
    public class ImagenResponseDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string NombreArchivo { get; set; } = string.Empty;
        public string RutaArchivo { get; set; } = string.Empty;
        public string? Extension { get; set; }
        public long Tamanio { get; set; }
        public DateTime FechaCarga { get; set; }    
    }
}
