namespace SistemaTecnico.Models
{
    public class EstadoTrabajo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; }
    }

    //public static class EstadosTrabajo
    //{
    //    public const int Pendiente = 1;
    //    public const int EnProceso = 2;
    //    public const int TrabajoFinalizado = 3;
    //    public const int Aprobado = 4;
    //    public const int PendientePago = 5;
    //    public const int Pagado = 6;
    //}

    public static class EstadosTrabajo
    {
        public const int PendienteRevisionSector = 1;
        public const int SolicitudRechazada = 2;
        public const int PendienteAsignacionTecnicos = 3;
        public const int PendientePresupuestos = 4;
        public const int PendienteAprobacionPresupuesto = 5;
        public const int PresupuestoAprobado = 6;
        public const int PendienteMateriales = 7;
        public const int MaterialesEnviados = 8;
        public const int EnProceso = 9;
        public const int PendienteAprobacionTrabajo = 10;
        public const int MejoraSolicitada = 11;
        public const int Aprobado = 12;
        public const int PendienteFacturacion = 13;
        public const int PendientePago = 14;
        public const int Finalizado = 15;
        public const int Cancelado = 16;
    }
}
