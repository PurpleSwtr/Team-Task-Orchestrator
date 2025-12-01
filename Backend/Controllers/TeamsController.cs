using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using System.Security.Claims;

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

        // 1. ПОЛУЧИТЬ МОИ КОМАНДЫ
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<Team>>> GetMyTeams()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ищем команды, где есть этот пользователь через связующую таблицу UsersCommands
            var teams = await _context.Teams
                .Where(t => t.UsersCommands.Any(uc => uc.IdUser == userId))
                .ToListAsync();

            return Ok(teams);
        }

        // 2. СОЗДАТЬ КОМАНДУ (И СРАЗУ ВСТУПИТЬ В НЕЕ)
        [HttpPost]
        [Authorize(Roles = "Teamlead,Admin")] // <--- Добавили ограничение
        public async Task<ActionResult<Team>> CreateTeam([FromBody] TeamCreateDto teamDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newTeam = new Team
            {
                TeamName = teamDto.TeamName,
                Description = teamDto.Description,
                // Поля CreatedAt и CreatedBy заполнятся сами через наш новый DbContext!
            };

            // Используем транзакцию, чтобы создать и команду, и связь одновременно
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Teams.Add(newTeam);
                    await _context.SaveChangesAsync(); // Сначала сохраняем, чтобы получить ID команды

                    // Автоматически добавляем создателя в команду
                    var memberLink = new UsersCommand
                    {
                        IdUser = userId,
                        IdTeam = newTeam.IdTeam
                    };
                    _context.UsersCommands.Add(memberLink);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetTeamById), new { id = newTeam.IdTeam }, newTeam);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeamById(int id)
        {
            var team = await _context.Teams
                .Include(t => t.UsersCommands) // Подгружаем участников
                .ThenInclude(uc => uc.IdUserNavigation) // Подгружаем данные юзеров (имена и т.д.)
                .FirstOrDefaultAsync(t => t.IdTeam == id);

            if (team == null) return NotFound("Команда не найдена.");

            // Тут можно добавить проверку доступа (состоит ли юзер в этой команде)

            return Ok(team);
        }

        // 3. ДОБАВИТЬ ПОЛЬЗОВАТЕЛЯ В КОМАНДУ
        [HttpPost("{teamId}/add-member")]
        [Authorize(Roles = "Teamlead,Admin")] // <--- Добавили ограничение
        public async Task<IActionResult> AddMember(int teamId, [FromBody] string emailToAdd)
        {
            // 1. Ищем пользователя по Email
            var userToAdd = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailToAdd);
            if (userToAdd == null) return NotFound("Пользователь с таким Email не найден.");

            // 2. Проверяем, существует ли команда
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null) return NotFound("Команда не найдена.");

            // 3. Проверяем, не состоит ли он уже там
            bool alreadyInTeam = await _context.UsersCommands
                .AnyAsync(uc => uc.IdTeam == teamId && uc.IdUser == userToAdd.Id);
            
            if (alreadyInTeam) return BadRequest("Пользователь уже в команде.");

            // 4. Добавляем
            var link = new UsersCommand
            {
                IdTeam = teamId,
                IdUser = userToAdd.Id
            };

            _context.UsersCommands.Add(link);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Пользователь успешно добавлен", User = userToAdd.ShortName });
        }
    }
}