using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly TodoListDbContext _context;

        public CommandsController(TodoListDbContext context)
        {
            _context = context;
        }

        // GET: api/Commands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Command>>> GetCommands()
        {
            return await _context.Commands.ToListAsync();
        }

        // GET: api/Commands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Command>> GetCommand(int id)
        {
            var command = await _context.Commands.FindAsync(id);

            if (command == null)
            {
                return NotFound();
            }

            return command;
        }

        // PUT: api/Commands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommand(int id, Command command)
        {
            if (id != command.IdTeam)
            {
                return BadRequest();
            }

            _context.Entry(command).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandExists(id))
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

        // POST: api/Commands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Command>> PostCommand(Command command)
        {
            _context.Commands.Add(command);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommand", new { id = command.IdTeam }, command);
        }

        // DELETE: api/Commands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommand(int id)
        {
            var command = await _context.Commands.FindAsync(id);
            if (command == null)
            {
                return NotFound();
            }

            _context.Commands.Remove(command);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommandExists(int id)
        {
            return _context.Commands.Any(e => e.IdTeam == id);
        }
    }
}
