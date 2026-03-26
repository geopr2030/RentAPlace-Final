using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAPlace.API.Interfaces;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET /api/admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            // don't send password hashes to client
            var result = users.Select(u => new
            {
                u.UserId,
                u.FullName,
                u.Email,
                u.Role,
                u.PhoneNumber,
                u.CreatedAt
            });
            return Ok(result);
        }

        // DELETE /api/admin/users/{id}
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool deleted = await _adminService.DeleteUserAsync(id);
            if (!deleted)
                return NotFound(new { message = "User not found." });

            return Ok(new { message = "User deleted." });
        }

        // GET /api/admin/properties
        [HttpGet("properties")]
        public async Task<IActionResult> GetAllProperties()
        {
            var properties = await _adminService.GetAllPropertiesAsync();
            return Ok(properties);
        }

        // DELETE /api/admin/properties/{id}
        [HttpDelete("properties/{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            bool deleted = await _adminService.DeletePropertyAsync(id);
            if (!deleted)
                return NotFound(new { message = "Property not found." });

            return Ok(new { message = "Property deleted." });
        }
    }
}
