using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;

namespace InventoryManagement_Backend.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
