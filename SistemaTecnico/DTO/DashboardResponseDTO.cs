namespace SistemaTecnico.DTO
{
    public class DashboardResponseDto
    {
        public int Pendientes { get; set; }

        public int EnProceso { get; set; }

        public int PendientePago { get; set; }

        public int Finalizados { get; set; }

        public int TotalTrabajos { get; set; }

        public int TotalClientes { get; set; }

        public int TotalTecnicos { get; set; }

        public int TrabajosHoy { get; set; }

        public int TrabajosMes { get; set; }
    }
}
