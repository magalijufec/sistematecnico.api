using SistemaTecnico.DTO;
using SistemaTecnico.Models;
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

        public async Task<ClienteResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var cliente = await _repository
                .ObtenerPorIdAsync(id);

            if (cliente == null)
                return null;

            return new ClienteResponseDTO
            {
                Id = cliente.Id,
                NroCliente = cliente.NroCliente,
                Nombre = cliente.Nombre,
                Email = cliente.Email ?? string.Empty,
                Direccion = cliente.Direccion ?? string.Empty,
                ProvinciaId = cliente.ProvinciaId,
                Provincia = cliente.Provincia.Nombre,
                CiudadId = cliente.CiudadId,
                Ciudad = cliente.Ciudad.Nombre,
                Activo = cliente.Activo
            };
        }

        public async Task<ClienteResponseDTO> CrearAsync(
            ClienteDTO dto)
        {
            var cliente = new Cliente
            {
                NroCliente = dto.NroCliente,
                Nombre = dto.Nombre,
                Email = dto.Email,
                Direccion = dto.Direccion,
                ProvinciaId = dto.ProvinciaId,
                CiudadId = dto.CiudadId,
                Activo = true
            };

            var creado = await _repository
                .CrearAsync(cliente);

            var clienteCompleto = await _repository
                .ObtenerPorIdAsync(creado.Id);

            return new ClienteResponseDTO
            {
                Id = clienteCompleto!.Id,
                NroCliente = clienteCompleto.NroCliente,
                Nombre = clienteCompleto.Nombre,
                //Email = clienteCompleto.Email,
                Direccion = clienteCompleto.Direccion,
                ProvinciaId = clienteCompleto.ProvinciaId,
                Provincia = clienteCompleto.Provincia.Nombre,
                CiudadId = clienteCompleto.CiudadId,
                Ciudad = clienteCompleto.Ciudad.Nombre
            };
        }

        public async Task<bool> ActualizarAsync(
            int id,
            ClienteDTO dto)
        {
            var cliente = await _repository
                .ObtenerPorIdAsync(id);

            if (cliente == null)
                return false;

            cliente.NroCliente = dto.NroCliente;
            cliente.Nombre = dto.Nombre;
            cliente.Email = dto.Email;
            cliente.Direccion = dto.Direccion;
            cliente.ProvinciaId = dto.ProvinciaId;
            cliente.CiudadId = dto.CiudadId;

            return await _repository
                .ActualizarAsync(cliente);
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
