using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Task = Backend.Models.Task;
using System.Security.Claims; // Не забудь добавить

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly TodoListDbContext _context;

        public TasksController(TodoListDbContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Task task)
        {
            if (id != task.IdTask)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // Назначить исполнителя (Это будет новый метод)
        [HttpPost("{taskId}/assign/{userId}")]
        [Authorize(Roles = "Moderator,Teamlead,Admin")]
        public async Task<IActionResult> AssignUser(int taskId, string userId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return NotFound();

            // Проверяем, существует ли уже такая связь
            bool exists = await _context.TasksUsers
                .AnyAsync(tu => tu.IdTask == taskId && tu.IdUser == userId);

            if (exists) return BadRequest("Пользователь уже назначен на эту задачу.");

            var link = new TasksUser
            {
                IdTask = taskId,
                IdUser = userId,
                IdAssignees = Guid.NewGuid().ToString() // Твой PK в таблице - строка
            };

            _context.TasksUsers.Add(link);
            await _context.SaveChangesAsync();

            return Ok("Исполнитель назначен");
        }


        // Создать задачу - Модератор, Тимлид, Админ
        [HttpPost]
        [Authorize(Roles = "Moderator,Teamlead,Admin")]
        public async Task<ActionResult<Task>> PostTask(Task task)
        {
            // При создании задачи нужно знать ID создателя (это уже делает наш DbContext)
            // Но нужно еще проверить, чтобы Модератор не создавал задачу в чужом проекте (это advanced уровень)
            // Пока ограничимся проверкой роли.

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.IdTask }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();


            return NoContent();
        }

        // Смена статуса - Доступно ВСЕМ (но с логикой)
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            var userRole = User.FindFirstValue(ClaimTypes.Role);

            // Пример логики ограничений
            if (userRole == "User")
            {
                // Пользователь не может перевести задачу в статус "Завершена" (finished), 
                // только в "Ожидает проверки" (waiting)
                if (newStatus == "finished") 
                {
                    return Forbid("Пользователь не может завершать задачи. Отправьте на проверку.");
                }
            }

            task.Status = newStatus;
            // task.UpdatedAt обновится само
            await _context.SaveChangesAsync();

            return Ok(new { message = "Статус обновлен", status = newStatus });
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.IdTask == id);
        }
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearAllTasks()
        {
            try
            {
                var entityType = _context.Model.FindEntityType(typeof(Task));
                if (entityType == null)
                {
                    return NotFound("Не удалось найти метаданные для сущности Task.");
                }
                
                var tableName = entityType.GetTableName();
                if (string.IsNullOrEmpty(tableName))
                {
                    return StatusCode(500, "Не удалось определить имя таблицы для сущности Task.");
                }

                string sqlCommand = $"DELETE FROM [{tableName}]";

                await _context.Database.ExecuteSqlRawAsync(sqlCommand);

                return Ok(new { Message = $"Все записи в таблице '{tableName}' были успешно удалены." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Произошла внутренняя ошибка сервера.", Error = ex.Message });
            }
        }
    }
}
