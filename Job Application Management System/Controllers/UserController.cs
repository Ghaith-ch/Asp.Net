using System.Security.Claims;
using AutoMapper;
using Job_Application_Management_System.Dto;
using Job_Application_Management_System.Repositories.Interfaces;
using Job_Application_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Job_Application_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IUserService userService, IMapper mapper)
        {
            _userRepository = userRepository;
            _userService = userService;
            _mapper = mapper;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromForm] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = await _userService.CreateUserAsync(createUserDto, "Applicant");
                return CreatedAtAction(nameof(GetUserById), new { userId }, new { Message = "User registered successfully.", UserId = userId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromForm] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _userService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/User/{userId}
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<ActionResult<GetUserDto>> GetUserById(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = "User not found." });

            var roles = await _userRepository.GetUserRolesAsync(user);
            var userDto = _mapper.Map<GetUserDto>(user);
            userDto.Roles = roles;
            return Ok(userDto);
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = new List<GetUserDto>();

            foreach (var user in users)
            {
                var userDto = _mapper.Map<GetUserDto>(user);
                userDto.Roles = await _userRepository.GetUserRolesAsync(user); // Avoid .Result
                userDtos.Add(userDto);
            }

            return Ok(userDtos);
        }

        // PUT: api/User/{userId}
        [HttpPut("{userId}")]
        [Authorize]
        public async Task<ActionResult> UpdateUser(int userId, [FromForm] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUserId != userId.ToString())
                return Forbid(); // Non-admins cannot update others' data

            try
            {
                var result = await _userService.UpdateUserAsync(userId, updateUserDto);
                if (!result)
                    return NotFound(new { Message = "User not found or could not be updated." });

                return Ok(new { Message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/User/{userId}
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var result = await _userRepository.DeleteUserAsync(userId);
            if (!result)
                return NotFound(new { Message = "User not found or could not be deleted." });

            return Ok(new { Message = "User deleted successfully." });
        }

        // POST: api/User/assign-role
        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AssignRole(int userId, [FromForm] string role)
        {
            var validRoles = new[] { "Admin", "Recruiter", "Applicant" };
            if (!validRoles.Contains(role))
                return BadRequest(new { Message = "Invalid role provided." });

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = "User not found." });

            var result = await _userService.AssignRoleAsync(userId, role);
            if (!result)
                return BadRequest(new { Message = "Failed to assign role." });

            return Ok(new { Message = $"Role '{role}' assigned successfully to user {userId}." });
        }
    }
}
