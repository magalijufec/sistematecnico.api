namespace SistemaTecnico.DTO
{
    public class TrabajoCreateDto
    {
        public int IdCliente { get; set; }

        public int IdTecnico { get; set; }

        public int IdTarea { get; set; }

        public DateTime FechaTrabajo { get; set; }

        public string? Comentarios { get; set; }
    }
}
