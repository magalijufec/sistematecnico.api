using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using SistemaTecnico.DTO;
using SistemaTecnico.Models;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IPerfilRepository _perfilRepository;
        private readonly IProvinciaRepository _provinciaRepository;
        private readonly ICiudadRepository _ciudadRepository;
        private readonly IClienteRepository _clienteRepository;

        public UsuarioService(IUsuarioRepository repository, IPerfilRepository perfilRepository, IProvinciaRepository provinciaRepository,
            ICiudadRepository ciudadRepository, IClienteRepository clienteRepository)
        {
            _repository = repository;
            _perfilRepository = perfilRepository;
            _provinciaRepository = provinciaRepository;
            _ciudadRepository = ciudadRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<UsuarioResponseDTO>> ObtenerTodosAsync()
        {
            var users = await _repository.ObtenerTodosAsync();
            return users
                .Select(x => new UsuarioResponseDTO
                {
                    Id = x.Id,
                    NombreApellido = x.NombreApellido,
                    UserName = x.UserName,
                    Email = x.Email,
                    Perfil = x.Perfil.Nombre,
                    PerfilId = x.Perfil.Id,
                    Provincia = x.Provincia?.Nombre,
                    ProvinciaId = x.Provincia.Id,
                    Ciudad = x.Ciudad?.Nombre,
                    CiudadId = x.Ciudad.Id,
                    Activo = x.Activo,
                    Cliente = x.Cliente != null ? $"{x.Cliente.NroCliente} - {x.Cliente.Nombre}" : " - ",
                    ClienteId = x.Cliente?.Id

                });
        }

        public async Task<UsuarioDetalleDTO?> ObtenerPorIdAsync(int id)
        {
            var user = await _repository.ObtenerPorIdAsync(id);
            if (user == null) return null;

            return new UsuarioDetalleDTO
            {
                Id = user.Id,
                NombreApellido = user.NombreApellido,
                UserName = user.UserName,
                Email = user.Email,
                PerfilId = user.Perfil.Id,
                ProvinciaId = user.Provincia.Id,
                CiudadId = user.Ciudad.Id,
                ClienteId = user.Cliente != null ? user.Cliente.Id : null,
                Activo = user.Activo
            };
        }

        public async Task<UsuarioDetalleDTO?> ObtenerPorIdActivoAsync(int id)
        {
            var user = await _repository.ObtenerPorIdActivoAsync(id);
            if (user == null) return null;

            return new UsuarioDetalleDTO
            {
                Id = user.Id,
                NombreApellido = user.NombreApellido,
                UserName = user.UserName,
                Email = user.Email,
                PerfilId = user.Perfil.Id,
                ProvinciaId = user.Provincia.Id,
                CiudadId = user.Ciudad.Id,
                ClienteId = user.Cliente != null ? user.Cliente.Id : null,
                Activo = user.Activo
            };
        }

        public async Task CrearAsync(UsuarioDTO usuario)
        {
            await _repository.AgregarAsync(usuario);
            await _repository.GuardarCambiosAsync();
        }

        public async Task<bool> ActualizarAsync(int id, UsuarioDTO usuario)
        {
            var existente = await _repository.ObtenerPorIdActivoAsync(id);

            if (existente == null)
                return false;

            existente.UserName = usuario.UserName;
            existente.NombreApellido = usuario.NombreApellido;
            existente.NumeroCelular = usuario.NumeroCelular;
            existente.Email = usuario.Email;
            existente.Activo = usuario.Activo;
            if (usuario.IdPerfil != null)
                existente.Perfil = await _perfilRepository.ObtenerPorIdAsync(usuario.IdPerfil);
            if (usuario.IdProvincia != null)
                existente.Provincia = await _provinciaRepository.ObtenerPorIdAsync(usuario.IdProvincia.Value);
            if (usuario.IdCiudad != null)
                existente.Ciudad = await _ciudadRepository.ObtenerPorIdAsync(usuario.IdCiudad.Value);
            if (usuario.IdCliente != null)
                existente.Cliente = await _clienteRepository.ObtenerPorIdAsync(usuario.IdCliente.Value);

            await _repository.ActualizarAsync(existente);

            await _repository.GuardarCambiosAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var usuario = await _repository.ObtenerPorIdActivoAsync(id);

            if (usuario == null)
                return false;

            await _repository.EliminarAsync(usuario);

            await _repository.GuardarCambiosAsync();

            return true;
        }

        public async Task<IEnumerable<TecnicoComboDTO>> ObtenerTecnicosAsync()
        {
            var usuarios = await _repository.ObtenerTecnicosAsync();

            return usuarios.Select(x => new TecnicoComboDTO
            {
                Id = x.Id,
                Nombre = x.NombreApellido,
                ProvinciaId = x.Provincia.Id
            });
        }
    }
}