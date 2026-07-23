namespace SistemaTecnico.DTO
{
    public class CambiarPasswordDTO
    {
        public string PasswordActual { get; set; } = string.Empty;

        public string PasswordNueva { get; set; } = string.Empty;

        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}
