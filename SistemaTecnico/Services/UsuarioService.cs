using Microsoft.EntityFrameworkCore;
using SistemaTecnico.DTO;
using SistemaTecnico.Models;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
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
                    Provincia = x.Provincia?.Nombre,
                    Ciudad = x.Ciudad?.Nombre,
                    Activo = x.Activo
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

        public async Task CrearAsync(UsuarioDTO usuario)
        {
            await _repository.AgregarAsync(usuario);
            await _repository.GuardarCambiosAsync();
        }

        public async Task<bool> ActualizarAsync(int id, UsuarioDTO usuario)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);

            if (existente == null)
                return false;

            await _repository.ActualizarAsync(existente);

            await _repository.GuardarCambiosAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var usuario = await _repository.ObtenerPorIdAsync(id);

            if (usuario == null)
                return false;

            await _repository.EliminarAsync(usuario);

            await _repository.GuardarCambiosAsync();

            return true;
        }

        public async Task<IEnumerable<ComboDTO>> ObtenerTecnicosAsync()
        {
            var usuarios = await _repository.ObtenerTecnicosAsync();

            return usuarios.Select(x => new ComboDTO
            {
                Id = x.Id,
                Nombre = x.NombreApellido
            });
        }
    }
}