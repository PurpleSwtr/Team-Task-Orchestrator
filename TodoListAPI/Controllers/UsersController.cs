using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TodoListAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListAPI.Controllers
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

        // Примечание: Логика создания (POST), обновления (PUT) и удаления (DELETE)
        // пользователей Identity сложнее и выполняется через методы UserManager,
        // (например, CreateAsync, UpdateAsync, DeleteAsync), а не напрямую через DbContext.
        // Этот базовый код для чтения данных теперь корректен.
    }
}