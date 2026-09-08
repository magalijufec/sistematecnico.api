using SistemaTecnico.DTO;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class SectorService : ISectorService
    {
        private readonly ISectorRepository _repository;

        public SectorService(ISectorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ComboDTO>> ObtenerTodasAsync()
        {
            var sectores = await _repository.ObtenerTodasAsync();

            return sectores.Select(x => new ComboDTO
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
        }
    }
}
