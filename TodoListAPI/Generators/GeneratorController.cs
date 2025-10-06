using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Linq; 
using TodoListAPI.Models;
using TodoListAPI.Generators;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

        public class GenerationRequest
        {
            public string? GeneratorTable { get; set; }
            public int CountGenerations { get; set; }
        }
        private Dictionary<string, object> _generators = new Dictionary<string, object>();

        public void GeneratorsInit()
        {
            _generators["Tasks"] = new Generators.DataGeneratorTask();
        }

        [HttpGet]
        public IActionResult GetAllTableNames()
        {
            var tableNames = _context.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Where(name => name != null && (!name.StartsWith("AspNet") || name == "AspNetUsers") && !name.Contains('-'))
                .ToList();
            return Ok(tableNames);
        }

        [HttpPost]
        public async Task<IActionResult> StartGeneration([FromBody] GenerationRequest request)
        {
            switch (request.GeneratorTable?.ToLower())
            {
                case "tasks":
                    var taskGenerator = new DataGeneratorTask();
                    await taskGenerator.Generate(_context, request.CountGenerations);
                    break;
                default:
                    return NotFound($"Генератор для '{request.GeneratorTable}' не найден.");
            }
            return Ok("Генерация завершена.");
        }

    }
}