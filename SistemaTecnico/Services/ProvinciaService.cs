using SistemaTecnico.DTO;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class ProvinciaService : IProvinciaService
    {
        private readonly IProvinciaRepository _repository;

        public ProvinciaService(
            IProvinciaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ComboDTO>> ObtenerComboAsync()
        {
            var provincias =
                await _repository.ObtenerTodasAsync();

            return provincias.Select(x => new ComboDTO
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
        }

        public async Task<ComboDTO?> ObtenerPorIdAsync(int id)
        {
            var provincia =
                await _repository.ObtenerPorIdAsync(id);

            if (provincia == null)
                return null;

            return new ComboDTO
            {
                Id = provincia.Id,
                Nombre = provincia.Nombre
            };
        }
    }
}
