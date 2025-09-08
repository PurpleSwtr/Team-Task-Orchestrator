using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;

namespace TodoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TodoListDbContext _context;

        public TasksController(TodoListDbContext context)
        {
            _context = context;
        }

        /// Получает список всех задач.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Задачи>>> GetTasks()
        {
            return await _context.Задачиs.ToListAsync();
        }

        /// Находит задачу по её ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Задачи>> GetTask(int id)
        {
            var task = await _context.Задачиs.FindAsync(id);

            return task is null ? NotFound() : task;
        }

        /// Создает новую задачу.
        [HttpPost]
        public async Task<ActionResult<Задачи>> PostTask(Задачи task)
        {
            _context.Задачиs.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.IdTask }, task);
        }

        /// Обновляет существующую задачу.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Задачи task)
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
                if (!_context.Задачиs.Any(e => e.IdTask == id))
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

        /// Удаляет задачу по её ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Задачиs.FindAsync(id);
            if (task is null)
            {
                return NotFound();
            }

            _context.Задачиs.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}