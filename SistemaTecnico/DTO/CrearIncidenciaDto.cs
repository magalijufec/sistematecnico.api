namespace SistemaTecnico.DTO
{
    public class CrearIncidenciaDto
    {
        public int UsuarioId { get; set; }
        public int ClienteId { get; set; }

        public int DestinoId { get; set; }

        public int TareaId { get; set; }

        public int AsistenciaId { get; set; }

        public int EstadoIncidenciaId { get; set; }

        public string? Otro { get; set; }

        public string? TrabajoRealizado { get; set; }
        public bool Guardia { get; set; }
        public string? Comentario { get; set; }
    }

}
