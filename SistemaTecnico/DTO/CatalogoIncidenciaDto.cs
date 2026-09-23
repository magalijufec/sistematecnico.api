namespace SistemaTecnico.DTO
{
    public class CatalogosIncidenciaDto
    {
        public List<ComboDTO> Asistencias { get; set; } = [];
        public List<ComboDTO> Destinos { get; set; } = [];
        public List<ComboDTO> Tareas { get; set; } = [];
    }
}
