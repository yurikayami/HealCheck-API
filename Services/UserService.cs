using HealCheckAPI.Data;
using HealCheckAPI.DTOs;
using HealCheckAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HealCheckAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UserResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return null;
            }

            // Hash password
            var hashedPassword = HashPassword(registerDto.Password);

            var user = new User
            {
                Username = registerDto.Username,
                Password = hashedPassword,
                Email = registerDto.Email,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task<UserResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null)
            {
                return null;
            }

            // Verify password
            if (!VerifyPassword(loginDto.Password, user.Password))
            {
                return null;
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = "JWT_TOKEN_PLACEHOLDER", // You can implement JWT token generation here
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            });
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            // Update username if provided
            if (!string.IsNullOrEmpty(updateUserDto.Username))
            {
                // Check if new username already exists
                if (await _context.Users.AnyAsync(u => u.Username == updateUserDto.Username && u.Id != id))
                {
                    return null;
                }
                user.Username = updateUserDto.Username;
            }

            // Update email if provided
            if (updateUserDto.Email != null)
            {
                user.Email = updateUserDto.Email;
            }

            // Update password if provided
            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.Password = HashPassword(updateUserDto.Password);
            }

            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }
    }
}
