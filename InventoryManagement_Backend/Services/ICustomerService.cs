using InventoryManagement_Backend.Dtos;

namespace InventoryManagement_Backend.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerReadDto>> GetAllCustomersAsync();
        Task<CustomerByIDReadDto?> GetCustomerByIdAsync(int id);
        //Task<CustomerReadDto> CreateCustomerAsync(CreateCustomerDto dto);
        //Task<bool> UpdateCustomerAsync(int id, UpdateCustomerDto dto);
        Task<bool> DeleteCustomerAsync(int id);
    }
}

