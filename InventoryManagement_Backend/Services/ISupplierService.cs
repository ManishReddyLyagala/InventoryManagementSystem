using InventoryManagement_Backend.Dtos;

namespace InventoryManagement_Backend.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierReadDto>> GetAllAsync();
        Task<SupplierbyIDReadDto?> GetByIdAsync(int id);
        Task<SupplierReadDto> CreateAsync(SupplierCreateDto dto);
        Task<bool> PatchAsync(int id, SupplierUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
