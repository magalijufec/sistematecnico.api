namespace SistemaTecnico.DTO
{
    public class ClienteComboDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public int ProvinciaId { get; set; }

        public int CiudadId { get; set; }
    }
}
