using SistemaTecnico.DTO;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _repository;

        public PerfilService(IPerfilRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ComboDTO>> ObtenerComboAsync()
        {
            var perfiles = await _repository.ObtenerTodos();

            return perfiles.Select(x => new ComboDTO
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
        }
    }
}
