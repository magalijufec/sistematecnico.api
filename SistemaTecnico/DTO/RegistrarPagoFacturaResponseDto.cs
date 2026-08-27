namespace SistemaTecnico.DTO
{
    public class RegistrarPagoFacturaResponseDto
    {
        public int TrabajoId { get; set; }

        public int FacturaId { get; set; }

        public DateTime FechaPagadoFactura { get; set; }

        public bool TrabajoFinalizado { get; set; }

        public int CantidadFacturas { get; set; }

        public int CantidadFacturasPagadas { get; set; }

        public int CantidadFacturasPendientes { get; set; }

        public string Mensaje { get; set; }
            = string.Empty;
    }
}
