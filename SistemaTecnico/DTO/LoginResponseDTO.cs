namespace SistemaTecnico.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;

        public int IdUsuario { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string NombreApellido { get; set; } = string.Empty;

        public int IdPerfil { get; set; }

        public string Perfil { get; set; } = string.Empty;
        public int? ClienteId { get; set; } 
    }
}
