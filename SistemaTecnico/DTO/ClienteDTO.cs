namespace SistemaTecnico.DTO
{
    public class ClienteDTO
    {
        public string NroCliente { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Direccion { get; set; }

        public int ProvinciaId { get; set; }

        public int CiudadId { get; set; }
    }
}
