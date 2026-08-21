namespace SistemaTecnico.Models
{
    public class ErrorLog
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string? Mensaje { get; set; }

        public string? StackTrace { get; set; }

        public string? InnerException { get; set; }

        public string? Endpoint { get; set; }

        public string? Metodo { get; set; }

        public string? Usuario { get; set; }

        public string? Ip { get; set; }
    }
}
