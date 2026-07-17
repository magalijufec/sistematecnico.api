using SistemaTecnico.DTO;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ComboDto>> ObtenerComboAsync()
        {
            var clientes = await _repository.ObtenerTodosAsync();

            return clientes
                .OrderBy(x => x.Nombre)
                .Select(x => new ComboDto
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                });
        }
    }
}
