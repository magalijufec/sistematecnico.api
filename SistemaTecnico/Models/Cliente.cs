namespace SistemaTecnico.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string NroCliente { get; set; }

        public string Nombre { get; set; } 

        public string? RazonSocial { get; set; }

        public string? Direccion { get; set; }

        public string? AddressShipToCode { get; set; }

        public int ProvinciaId { get; set; }
        public Provincia Provincia { get; set; }
        public int CiudadId { get; set; }
        public Ciudad Ciudad { get; set; }

        public IList<Trabajo> Trabajos { get; set; } = new List<Trabajo>();
    }
}
