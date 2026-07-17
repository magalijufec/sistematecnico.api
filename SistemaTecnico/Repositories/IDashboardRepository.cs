using SistemaTecnico.DTO;

namespace SistemaTecnico.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardResponseDto> ObtenerDashboardAsync();
    }
}
