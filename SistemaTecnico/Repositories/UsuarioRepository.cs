using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _context.Usuarios
                .Include(x => x.Perfil)
                .Include(x => x.Provincia)
                .Include(x => x.Ciudad)
                //.Include(x => x.Sector)
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(x => x.Perfil)
                .Include(x => x.Provincia)
                .Include(x => x.Ciudad)
                //.Include(x => x.Sector)
                .FirstOrDefaultAsync(x => x.Id == id && x.Activo);
        }

        public async Task<Usuario?> ObtenerPorUsuarioAsync(string userName)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(x => x.UserName == userName && x.Activo);
        }

        public async Task AgregarAsync(UsuarioDTO usuario)
        {
            var user = new Usuario
            {
                NombreApellido = usuario.NombreApellido,
                UserName = usuario.UserName,
                PasswordHash = usuario.PasswordHash,
                Email = usuario.Email,
                NumeroCelular = usuario.NumeroCelular,
                Activo = true,
                Perfil = await _context.Perfiles.FindAsync(usuario.IdPerfil),
                Provincia = usuario.IdProvincia.HasValue ? await _context.Provincias.FindAsync(usuario.IdProvincia.Value) : null,
                Ciudad = usuario.IdCiudad.HasValue ? await _context.Ciudades.FindAsync(usuario.IdCiudad.Value) : null
            };
            await _context.Usuarios.AddAsync(user);
        }

        public Task ActualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            return Task.CompletedTask;
        }

        public Task EliminarAsync(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            return Task.CompletedTask;
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(x => x.Id == id && x.Activo);
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> ObtenerTecnicosAsync()
        {
            return await _context.Usuarios
                .Where(x => x.Perfil.Id == 2 && x.Activo)
                .OrderBy(x => x.NombreApellido)
                .ToListAsync();
        }
    }
}
