using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly InventoryDbContext _context;

        public UserService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var customers = await _context.User.ToListAsync();

            return customers.Select(u => new UserReadDto
            {
                UserId = u.UserId,
                Name = u.Name,
                MobileNumber = u.MobileNumber,
                EmailID = u.EmailID,
                Role = u.Role,
                Address = u.Address
            }).ToList();
        }


        public async Task<UserByIDReadDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.User
                                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return null;

            return new UserByIDReadDto
            {
                UserId = user.UserId,
                Name = user.Name,
                MobileNumber = user.MobileNumber,
                EmailID = user.EmailID,
                Address = user.Address
                
            };

        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.User
                                         .FirstOrDefaultAsync(u => u.UserId == id && u.Role == "Customer");

            if (user == null) return false;

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
