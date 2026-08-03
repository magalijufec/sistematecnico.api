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

        public async Task<IEnumerable<ClienteResponseDTO>> ObtenerTodosAsync()
        {
            var clientes = await _repository.ObtenerTodosAsync();

            return clientes.Select(c => new ClienteResponseDTO
            {
                Id = c.Id,
                NroCliente = c.NroCliente,
                Nombre = c.Nombre,
                Provincia = c.Provincia.Nombre,
                ProvinciaId = c.ProvinciaId,
                Ciudad = c.Ciudad.Nombre,
                CiudadId = c.CiudadId,
                Direccion = c.Direccion ?? string.Empty,
                RazonSocial = c.RazonSocial ?? string.Empty
            });
        }

        public async Task<IEnumerable<ClienteComboDto>> ObtenerComboAsync()
        {
            var clientes = await _repository.ObtenerTodosAsync();

            return clientes
                .OrderBy(x => x.Nombre)
                .Select(x => new ClienteComboDto
                {
                    Id = x.Id,
                    Nombre = x.NroCliente + " "+ x.Nombre,
                    ProvinciaId = x.ProvinciaId,
                    CiudadId = x.CiudadId
                });
        }

        public async Task<IEnumerable<ClienteComboDto>> ObtenerPorProvinciaCiudadAsync(
            int provinciaId,
            int ciudadId)
        {
            var clientes =
                await _repository
                    .ObtenerPorProvinciaCiudadAsync(
                        provinciaId,
                        ciudadId
                    );

            return clientes.Select(x => new ClienteComboDto
            {
                Id = x.Id,
                Nombre = x.Nombre,
                ProvinciaId = x.ProvinciaId,
                CiudadId = x.CiudadId
            });
        }
    }
}
