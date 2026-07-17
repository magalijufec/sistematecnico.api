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
        public async Task<IEnumerable<ComboDto>> ObtenerTodasAsync()
        {
            var tareas = await _repository.ObtenerTodasAsync();

            return tareas.Select(x => new ComboDto
            {
                Id = x.Id,
                Nombre = x.Descripcion
            });
        }
    }
}
