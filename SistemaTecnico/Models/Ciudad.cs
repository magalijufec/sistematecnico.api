namespace SistemaTecnico.Models
{
    public class Ciudad
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        //public int IdProvincia { get; set; }

        public Provincia Provincia { get; set; } = null!;

        //public IList<Usuario> Usuarios { get; set; } = new List<Usuario>();

        //public IList<Cliente> Clientes { get; set; } = new List<Cliente>();
    }
}
