using LearningManagementSystem.Dto;
using LearningManagementSystem.Models;
using LearningManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Only Admin can access these endpoints
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserManagementController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/UserManagement/{userId}/role
        [HttpPost("{userId}/role")]
        public async Task<ActionResult> AssignRoleToUser(int userId, [FromForm] string role)
        {
            try
            {
                var result = await _userService.AssignRoleAsync(userId, role);
                if (!result)
                {
                    return BadRequest(new { Message = $"Failed to assign role '{role}' to user with ID {userId}." });
                }

                return Ok(new { Message = $"Role '{role}' assigned successfully to user with ID {userId}." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/UserManagement/roles/{role}
        [HttpGet("roles/{role}")]
        public async Task<ActionResult<List<GetUserDto>>> GetUsersByRole(string role)
        {
            try
            {
                var users = await _userService.GetUsersByRoleAsync(role);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/UserManagement/{userId}/role
        [HttpDelete("{userId}/role")]
        public async Task<ActionResult> RemoveRoleFromUser(int userId, [FromForm] string role)
        {
            try
            {
                var result = await _userService.RemoveRoleAsync(userId, role);
                if (!result)
                {
                    return BadRequest(new { Message = $"Failed to remove role '{role}' from user with ID {userId}." });
                }

                return Ok(new { Message = $"Role '{role}' removed successfully from user with ID {userId}." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }




    }
}
