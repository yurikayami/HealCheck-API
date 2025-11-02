namespace HealCheckAPI.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
    }

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
