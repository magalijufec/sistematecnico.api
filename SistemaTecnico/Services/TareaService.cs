using SistemaTecnico.DTO;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class TareaService : ITareaService
    {
        private readonly ITareaRepository _repository;
        public TareaService(ITareaRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ComboDTO>> ObtenerTodasAsync()
        {
            var tareas = await _repository.ObtenerTodasAsync();

            return tareas.Select(x => new ComboDTO
            {
                Id = x.Id,
                Nombre = x.Descripcion
            });
        }

        public async Task<IEnumerable<ComboDTO>> ObtenerPorSectorAsync(int sectorId)
        {
            var tareas = await _repository.ObtenerPorSectorAsync(sectorId);

            return tareas.Select(x => new ComboDTO
            {
                Id = x.Id,
                Nombre = x.Descripcion
            });
        }
    }
}
