using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface IErrorLogRepository
    {
        Task RegistrarAsync(ErrorLog error);
    }
}
