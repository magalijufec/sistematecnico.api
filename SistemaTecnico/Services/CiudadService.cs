using SistemaTecnico.DTO;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class CiudadService : ICiudadService
    {
        private readonly ICiudadRepository _repository;

        public CiudadService(
            ICiudadRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ComboDTO>> ObtenerComboAsync()
        {
            var ciudades =
                await _repository.ObtenerTodasAsync();

            return ciudades.Select(x => new ComboDTO
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
        }

        public async Task<IEnumerable<ComboDTO>> ObtenerPorProvinciaAsync(
            int provinciaId)
        {
            var ciudades =
                await _repository.ObtenerPorProvinciaAsync(
                    provinciaId);

            return ciudades.Select(x => new ComboDTO
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
        }

        public async Task<ComboDTO?> ObtenerPorIdAsync(int id)
        {
            var ciudad =
                await _repository.ObtenerPorIdAsync(id);

            if (ciudad == null)
                return null;

            return new ComboDTO
            {
                Id = ciudad.Id,
                Nombre = ciudad.Nombre
            };
        }
    
    }
}
