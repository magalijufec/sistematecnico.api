namespace SistemaTecnico.DTO
{
    public class ClienteResponseDTO
    {
        public int Id { get; set; }
        public string NroCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodNombre { get; set; }
        public string Provincia { get; set; }
        public int ProvinciaId { get; set; }
        public string Ciudad { get; set; }
        public int CiudadId { get; set; }
        public string Direccion { get; set; }
        public string RazonSocial { get; set; }
    }
}
