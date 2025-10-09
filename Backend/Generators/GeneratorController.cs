using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Linq; 
using Backend.Models;
using Backend.Generators;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.AspNetCore.Identity; 
namespace Backend.Controllers
{
    [ApiController]
    [Authorize (Roles = "Admin")]
    [Route("api/[controller]")]
    public class GeneratorController : ControllerBase
    {
        private readonly TodoListDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GeneratorController(
            TodoListDbContext context, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public class GenerationRequest
        {
            public string? GeneratorTable { get; set; }
            public int CountGenerations { get; set; }
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

        // ВРОДЕ ХОРОШАЯ ИДЕЯ, НО МНЕ ПОКАЗАЛОСЬ ИЗБЫТОЧНО, И МОЖНО ОГРАНИЧИТЬСЯ СВИТЧ-КЕЙСОМ ДЛЯ НЕ ТАКОГО БОЛЬШОГО
        // КОЛИЧЕСТВА ТАБЛИЦ, НО В ТЕОРИИ ЭТО КОНЕЧНО МОЖНО БЫЛО БЫ ВЫНЕСТИ В ОТДЕЛЬНУЮ ФАБРИКУ

        // private Dictionary<string, object> _generators = new Dictionary<string, object>();

        // private void GeneratorsInit()
        // {
        //     _generators["Tasks"] = new Generators.DataGeneratorTask();
        // }

        [HttpPost]
        public async Task<IActionResult> StartGeneration([FromBody] GenerationRequest request)
        {
            switch (request.GeneratorTable?.ToLower())
            {
                case "tasks":
                    var taskGenerator = new DataGeneratorTask();
                    await taskGenerator.Generate(_context, request.CountGenerations);
                    break;
                case "aspnetusers":
                    var userGenerator = new DataGeneratorUser();
                    await userGenerator.Generate(_context, request.CountGenerations, _userManager, _roleManager);
                    break;
                default:
                    return NotFound($"Генератор для '{request.GeneratorTable}' не найден.");

            }
            return Ok("Генерация завершена.");
        }
    }
}