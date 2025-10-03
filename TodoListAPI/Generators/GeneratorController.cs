using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Linq;                 
using TodoListAPI.Models;

namespace TodoListAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class GeneratorController : ControllerBase
    {
        private readonly TodoListDbContext _context;

        public GeneratorController(TodoListDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllTableNames()
        {
            var tableNames = _context.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Where(name => name != null && (!name.StartsWith("AspNet") || name == "AspNetUsers") && !name.Contains("-"))
                .ToList();
            return Ok(tableNames);
        }
    }
}