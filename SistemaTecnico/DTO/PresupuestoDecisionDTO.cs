namespace SistemaTecnico.DTO
{
    public class PresupuestoDecisionDTO
    {
        public int IdUsuarioDecision { get; set; }
        public bool Rechazo { get; set; }
        public string? MotivoRechazo { get; set; }
    }
}
