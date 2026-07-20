using SistemaTecnico.DTO;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class EstadoService : IEstadoService
    {
        private readonly IEstadoRepository _repository;

        public EstadoService(IEstadoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ComboDTO>> ObtenerComboAsync()
        {
            var estados = await _repository.ObtenerTodosAsync();

            return estados.Select(x => new ComboDTO
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
        }

        public async Task<List<ComboDTO>> ObtenerEstadosSiguientes(int idTrabajo)
        {
            return await _repository.ObtenerEstadosSiguientes(idTrabajo);
        }
    }
}
