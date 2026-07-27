namespace SistemaTecnico.DTO
{
    public class UsuarioDetalleDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string NombreApellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? NumeroCelular { get; set; }
        public bool Activo { get; set; }
        public int PerfilId { get; set; }
        public int ProvinciaId { get; set; }
        public int CiudadId { get; set; }
        public int? ClienteId { get; set; }
    }
}
