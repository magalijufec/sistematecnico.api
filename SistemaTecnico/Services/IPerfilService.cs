using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface IPerfilService
    {
        Task<IEnumerable<ComboDTO>> ObtenerComboAsync();
    }
}
