using HealCheckAPI.DTOs;
using HealCheckAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealCheckAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterAsync(registerDto);

            if (result == null)
            {
                return BadRequest(new { message = "Username already exists" });
            }

            return Ok(result);
        }

        /// <summary>
        /// Login a user
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.LoginAsync(loginDto);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Update user
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserAsync(id, updateUserDto);

            if (result == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(result);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
