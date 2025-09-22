using InventoryManagement_Backend.Dtos;

namespace InventoryManagement_Backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
        Task<UserByIDReadDto?> GetUserByIdAsync(int id);
        //Task<CustomerReadDto> CreateCustomerAsync(CreateCustomerDto dto);
        //Task<bool> UpdateCustomerAsync(int id, UpdateCustomerDto dto);
        Task<bool> DeleteUserAsync(int id);
    }
}

