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

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _repository.ObtenerTodosAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _repository.ObtenerPorIdAsync(id);
        }

        public async Task CrearAsync(UsuarioDTO usuario)
        {
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);

            await _repository.AgregarAsync(usuario);

            await _repository.GuardarCambiosAsync();

            //return usuario;
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
    }
}