namespace SistemaTecnico.DTO
{
    public class TrabajoUpdateDto
    {
        public DateTime FechaTrabajo { get; set; }

        public int IdTecnico { get; set; }

        public int IdCliente { get; set; }

        //public int? IdSector { get; set; }

        public int IdTarea { get; set; }

        public string? Comentarios { get; set; }

        public string? TrabajoRealizado { get; set; }
    }
}
