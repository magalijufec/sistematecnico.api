namespace SistemaTecnico.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string UserName { get; set; } 

        public string PasswordHash { get; set; } 

        public string NombreApellido { get; set; } 

        public string? Email { get; set; }

        public string? NumeroCelular { get; set; }

        public bool Activo { get; set; }

        public Perfil Perfil { get; set; } = null!;

        public Provincia? Provincia { get; set; }

        public Ciudad? Ciudad { get; set; }

        //public IList<Trabajo> Trabajos { get; set; } = new List<Trabajo>();

        //ublic ICollection<HistorialEstado> HistorialEstados { get; set; } = new List<HistorialEstado>();
    }
}
