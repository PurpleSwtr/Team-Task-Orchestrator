using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TodoListDbContext _context;
        public UsersController(UserManager<ApplicationUser> userManager, TodoListDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithRolesDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(user => new UserWithRolesDto
                {
                    Id = user.Id,
                    ShortName = user.ShortName,
                    Email = user.Email,
                    Gender = user.Gender,
                    Roles = (from userRole in _context.UserRoles
                            join role in _context.Roles on userRole.RoleId equals role.Id
                            where userRole.UserId == user.Id
                            select role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithRolesDto>> GetUser(string id) 
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userWithRoles = new UserWithRolesDto
            {
                Id = user.Id,
                ShortName = user.ShortName,
                Email = user.Email,
                Gender = user.Gender,
                RegistrationTime = user.RegistrationTime,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                Roles = roles
            };

            return Ok(userWithRoles);
        }

        // DELETE: api/Users/DeleteAll
        [HttpDelete("DeleteAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                foreach (var user in users)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return BadRequest($"Failed to delete user {user.UserName}: {errors}");
                    }
                }
                return Ok($"Successfully deleted {users.Count} users");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("ChangeRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserWithRolesDto>> ChangeUserRole([FromBody] UserRoleDto newRoles)
        {
            var user = await _userManager.FindByIdAsync(newRoles.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to remove user roles");
            }

            result = await _userManager.AddToRolesAsync(user, newRoles.Roles);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to add user roles");
            }

            var updatedRoles = await _userManager.GetRolesAsync(user);

            var userWithRoles = new UserWithRolesDto
            {
                Id = user.Id,
                ShortName = user.ShortName,
                Email = user.Email,
                Gender = user.Gender,
                RegistrationTime = user.RegistrationTime,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                Roles = updatedRoles
            };

            return Ok(userWithRoles);
        }
        // добавить логику создания (POST), обновления (PUT) и удаления (DELETE)

    }
}