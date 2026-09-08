namespace SistemaTecnico.Models
{
    public class Sector
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; } = null!;
    }
}
