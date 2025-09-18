using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement_Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly InventoryDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();
        private readonly JwtSettings _jwtSettings;
        private readonly string _adminSecret;

        public AuthService(InventoryDbContext context,IOptions<JwtSettings> jwtOptions, IConfiguration config)
        {
            _context = context;
            _jwtSettings = jwtOptions.Value;
            _adminSecret = config["AdminSecret"] ?? string.Empty;
        }
        

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Email and password required");

            // If trying to register as Admin, verify the admin secret
            if (!string.IsNullOrWhiteSpace(dto.Role) &&
                dto.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(_adminSecret) || dto.AdminSecret != _adminSecret)
                    throw new UnauthorizedAccessException("Invalid admin secret code.");
            }

            // Duplicate email check
            if (await _context.User.AnyAsync(u => u.EmailID == dto.Email))
                throw new InvalidOperationException("Email already registered.");

            var user = new User
            {
                Name = dto.UserName,
                EmailID = dto.Email,
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "Customer" : dto.Role
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.EmailID == dto.Email);
            if (user == null) return null;

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verify == PasswordVerificationResult.Failed) return null;

            //token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.EmailID),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new AuthResponseDto
            {
                Token = jwt,
                UserName = user.Name,
                Role = user.Role,
                ExpiresAt = tokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)
            };
        }
    }
}
