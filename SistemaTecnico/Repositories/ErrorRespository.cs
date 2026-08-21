using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly AppDbContext _context;

        public ErrorLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(ErrorLog error)
        {
            await _context.ErrorLogs.AddAsync(error);
            await _context.SaveChangesAsync();
        }
    }
}
