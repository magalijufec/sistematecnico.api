using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class ImagenRepository : IImagenRepository
    {
        private readonly AppDbContext _context;

        public ImagenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Imagen>> ObtenerPorTrabajoAsync(int idTrabajo)
        {
            return _context.Imagenes
                .Where(x => x.Trabajo.Id == idTrabajo)
                .ToList();
        }

        public async Task<Imagen?> ObtenerPorIdAsync(int id)
        {
            return _context.Imagenes
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task AgregarAsync(Imagen imagen)
        {
            await _context.Imagenes.AddAsync(imagen);
        }

        public Task EliminarAsync(Imagen imagen)
        {
            _context.Imagenes.Remove(imagen);
            return Task.CompletedTask;
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
