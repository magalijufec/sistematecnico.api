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
        public async Task<IEnumerable<ComboDTO>> ObtenerComboAsync()
        {
            var clientes = await _repository.ObtenerTodosAsync();

            return clientes
                .OrderBy(x => x.Nombre)
                .Select(x => new ComboDTO
                {
                    Id = x.Id,
                    Nombre = x.NroCliente + " "+ x.Nombre
                });
        }
    }
}
