using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Backend.Models; // <-- Важно: добавляем using для доступа к вашим моделям
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeamsController : ControllerBase
    {
        private readonly TodoListDbContext _context;
        
        public TeamsController(TodoListDbContext context)
        {
            _context = context;
        }

        // POST: api/teams
        /// <summary>
        /// Создает новую команду. Доступно только для Тимлидов и Администраторов.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Teamlead,Admin")]
        public async Task<ActionResult<Team>> CreateTeam([FromBody] TeamCreateDto teamDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newTeam = new Team
            {
                TeamName = teamDto.TeamName,
                Description = teamDto.Description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeamById), new { id = newTeam.IdTeam }, newTeam);
        }

        // GET: api/teams/{id}
        /// <summary>
        /// Получает информацию о команде по ее ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeamById(int id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return NotFound($"Команда с ID {id} не найдена.");
            }

            // В будущем здесь можно добавить проверку, состоит ли текущий пользователь в этой команде
            
            return Ok(team);
        }

        // POST: api/teams/{teamId}/users
        /// <summary>
        /// Добавляет пользователя в команду. Доступно Тимлиду (этой команды) и Админу.
        /// </summary>
        [HttpPost("{teamId}/users")]
        [Authorize(Roles = "Teamlead,Admin")]
        public async Task<IActionResult> AddUserToTeam(int teamId, [FromBody] UserToTeamDto dto)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
            {
                return NotFound("Команда не найдена.");
            }

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            // Проверка, что пользователь еще не в команде
            var userExists = await _context.UsersCommands
                .AnyAsync(uc => uc.IdTeam == teamId && uc.IdUser == dto.UserId);

            if (userExists)
            {
                return BadRequest("Пользователь уже состоит в этой команде.");
            }
            
            var userCommand = new UsersCommand
            {
                IdUser = dto.UserId,
                IdTeam = teamId
            };

            _context.UsersCommands.Add(userCommand);
            await _context.SaveChangesAsync();

            return Ok("Пользователь успешно добавлен в команду.");
        }
    }
}

