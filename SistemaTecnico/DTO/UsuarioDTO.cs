using SistemaTecnico.Models;

namespace SistemaTecnico.DTO
{
    public class UsuarioDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NombreApellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? NumeroCelular { get; set; }
        public bool Activo { get; set; }
        public int IdPerfil { get; set; }

        //public int? IdSector { get; set; }

        //public Sector? Sector { get; set; }
        public int? IdProvincia { get; set; }
        public int? IdCiudad { get; set; }
        public int? ClienteId { get; set; }
    }
}
