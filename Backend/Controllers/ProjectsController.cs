using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly TodoListDbContext _context;

        public ProjectsController(TodoListDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects?teamId=5 (Если teamId не передан, вернет все доступные)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects([FromQuery] int? teamId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IQueryable<Project> query = _context.Projects;

            if (teamId.HasValue)
            {
                // Вернуть проекты конкретной команды
                // (Стоит добавить проверку, имеет ли юзер доступ к этой команде)
                query = query.Where(p => p.IdTeam == teamId.Value);
            }
            else
            {
                // Вернуть ВСЕ проекты тех команд, в которых состоит юзер
                // Это сложный запрос: Выбрать проекты, где IdTeam содержится в списке команд юзера
                var myTeamIds = _context.UsersCommands
                    .Where(uc => uc.IdUser == userId)
                    .Select(uc => uc.IdTeam);

                query = query.Where(p => myTeamIds.Contains(p.IdTeam.Value));
            }

            return await query.ToListAsync();
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Moderator,Teamlead,Admin")] // <---
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.IdProject)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        [HttpPost]
        [Authorize(Roles = "Teamlead,Admin")] // <---
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            // Важно: Фронт должен передавать IdTeam в теле запроса!
            if (project.IdTeam == null || project.IdTeam == 0)
            {
                return BadRequest("Проект должен быть привязан к команде (IdTeam обязателен).");
            }

            // Проверка: состоит ли создатель в этой команде?
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool access = await _context.UsersCommands
                .AnyAsync(uc => uc.IdTeam == project.IdTeam && uc.IdUser == userId);
            
            if (!access && !User.IsInRole("Admin")) 
            {
                return Forbid("Вы не состоите в этой команде и не можете создавать для нее проекты.");
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjects", new { id = project.IdProject }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teamlead,Admin")] // <---
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.IdProject == id);
        }
    }
}
