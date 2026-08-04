namespace SistemaTecnico.DTO
{
    public class UsuarioActualizarDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string NombreApellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? NumeroCelular { get; set; }

        public bool Activo { get; set; }

        public int IdPerfil { get; set; }
        public int? IdProvincia { get; set; }
        public int? IdCiudad { get; set; }
        public int? IdCliente { get; set; }

        public bool CambiarPassword { get; set; }

        public string? Password { get; set; }
    }
}
