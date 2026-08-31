namespace SistemaTecnico.Models;

public class Perfil
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public static class Perfiles
{
    public const int Administrador = 6;
    public const int Tecnico = 7;
    public const int Farmacia = 8;
    public const int Pagos = 9;
    public const int Sistemas = 10;
}