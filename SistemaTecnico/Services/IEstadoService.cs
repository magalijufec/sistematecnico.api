using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface IEstadoService
    {
        Task<IEnumerable<ComboDTO>> ObtenerComboAsync();
        Task<List<ComboDTO>> ObtenerEstadosSiguientes(int idTrabajo);
    }
}
