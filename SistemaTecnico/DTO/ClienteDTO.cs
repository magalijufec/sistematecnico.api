namespace SistemaTecnico.DTO
{
    public class ClienteDTO
    {
        public string NroCliente { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Direccion { get; set; }

        public int IdProvincia { get; set; }

        public int IdCiudad { get; set; }
    }
}
