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

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            // Получаем пользователей через UserManager
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id) // ID теперь строка
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // DELETE: api/Users/DeleteAll
        [HttpDelete("DeleteAll")]
        [Authorize] // Ограничиваем доступ только для администраторов
        public async Task<IActionResult> DeleteAllUsers()
        {
            try
            {
                // Получаем всех пользователей
                var users = await _userManager.Users.ToListAsync();
                // Удаляем каждого пользователя
                foreach (var user in users)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        // Логируем ошибки, если удаление не удалось
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
       
        // добавить логику создания (POST), обновления (PUT) и удаления (DELETE)

    }
}