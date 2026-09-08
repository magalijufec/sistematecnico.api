namespace SistemaTecnico.DTO
{
    public class TrabajoCreateDto
    {
        public int IdCliente { get; set; }
        public List<int>? IdsTecnicos { get; set; }
        public int IdSector { get; set; }
        public int IdTarea { get; set; }
        public string? Comentarios { get; set; }
        public List<IFormFile>? Archivos { get; set; }
    }
}
