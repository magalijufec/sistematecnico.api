using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface ITareaService
    {
        Task<IEnumerable<ComboDto>> ObtenerTodasAsync();
    }
}
