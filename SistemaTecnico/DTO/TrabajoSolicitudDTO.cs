namespace SistemaTecnico.DTO
{
    public class TrabajoSolicitudDTO
    {
        public int Id { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int IdCliente { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public int IdSector { get; set; }
        public string Sector { get; set; }
        public int IdTarea { get; set; }
        public string Tarea { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public string EstadoColor { get; set; }
    }
}
