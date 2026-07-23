using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDTO?> LoginAsync(LoginDTO dto);
    }
}
