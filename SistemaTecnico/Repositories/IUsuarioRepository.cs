using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();

        Task<Usuario?> ObtenerPorIdAsync(int id);

        Task<Usuario?> ObtenerPorIdActivoAsync(int id);

        Task<List<Usuario>?> ObtenerPorClienteAsync(int clienteId);

        Task<Usuario?> ObtenerPorUsuarioAsync(string userName);

        Task AgregarAsync(UsuarioDTO  usuario);

        Task ActualizarAsync(Usuario usuario);

        Task EliminarAsync(Usuario usuario);

        Task<bool> ExisteAsync(int id);

        Task GuardarCambiosAsync();

        Task<IEnumerable<Usuario>> ObtenerTecnicosAsync();
        Task<IEnumerable<Usuario>> ObtenerPorPerfil(int idPerfil);
    }
}