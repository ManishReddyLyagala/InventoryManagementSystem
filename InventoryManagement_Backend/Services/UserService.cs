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
            var users = await _context.User.ToListAsync();

            return users.Select(u => new UserReadDto
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
                Address = user.Address,
                Role= user.Role
                
            };

        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.User
                                         .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return false;

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserByIDReadDto> UpdateUserAsync(int userId, UpdateCustomerDto userDetail)
        {
            var userData = await _context.User.FindAsync(userId);
            if (userData == null) return null;
            userData.Name = userDetail.Name;
            userData.EmailID = userDetail.Email;
            userData.MobileNumber = userDetail.MobileNumber;
            userData.Address = userDetail.Address;
            await _context.SaveChangesAsync();
            return new UserByIDReadDto
            {
                UserId = userData.UserId,
                Name = userData.Name,
                MobileNumber = userData.MobileNumber,
                EmailID = userData.EmailID,
                Address = userData.Address,
                Role = userData.Role,
                CreatedAt = userData.CreatedAt

            };
        }
    }
}
