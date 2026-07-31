namespace SistemaTecnico.Models
{
    public class EstadoTrabajo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; }
    }

    public static class EstadosTrabajo
    {
        public const int Pendiente = 1;
        public const int EnProceso = 2;
        public const int TrabajadoFinalizado = 3;
        public const int Aprobado = 4;
        public const int PendientePago = 5;
        public const int Finalizado = 6;
    }
}
