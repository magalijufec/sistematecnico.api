using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<ComboDto>> ObtenerComboAsync();
    }
}
