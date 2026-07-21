namespace SistemaTecnico.DTO
{
    public class ImagenTrabajoDto
    {
        public IFormFile Archivo { get; set; } = null!;
        public bool EsAntes { get; set; }
    }
}
