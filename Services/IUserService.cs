using HealCheckAPI.DTOs;

namespace HealCheckAPI.Services
{
    public interface IUserService
    {
        Task<UserResponseDto?> RegisterAsync(RegisterDto registerDto);
        Task<UserResponseDto?> LoginAsync(LoginDto loginDto);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
