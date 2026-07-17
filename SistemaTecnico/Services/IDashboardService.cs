using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface IDashboardService
    {
        Task<DashboardResponseDto> ObtenerDashboardAsync();
    }
}