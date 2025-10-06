
---

### 📄 `Program.cs`

```csharp
using TodoListAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TodoListAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Task = System.Threading.Tasks.Task;


using TodoListAPI.Generators; // <-- ДОБАВЬ ЭТУ СТРОКУ В САМОМ ВЕРХУ ФАЙЛА

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("--- ЗАПУСК ТЕСТА ГЕНЕРАТОРА ---");

var taskGenerator = new DataGeneratorTask();
var new_task = taskGenerator.GetTask();
Console.WriteLine(new_task);
Console.WriteLine("--- ТЕСТ ГЕНЕРАТОРА ЗАВЕРШЕН ---");
return;
var configuration = builder.Configuration;

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TodoListDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<TodoListDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["jwtToken"];
            return Task.CompletedTask;
        }
    };

    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["Jwt:Audience"],
        ValidIssuer = configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 
        });
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TodoListDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка во время миграции базы данных.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("VueApp");
// ----------------------------------------

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

### 📄 `TodoListAPI.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.6" />
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.6" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.20" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.20">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.20" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.20">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="8.0.6" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
    
  </ItemGroup>
  <ItemGroup>
    <Content Include="Generators\Files\**\*.*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
</Project>
```

---

### 📄 `TodoListAPI.sln`

```text
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.5.2.0
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "TodoListAPI", "TodoListAPI.csproj", "{4747A7DF-3B89-89FA-E0FA-75003B50B2C4}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{4747A7DF-3B89-89FA-E0FA-75003B50B2C4}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{4747A7DF-3B89-89FA-E0FA-75003B50B2C4}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{4747A7DF-3B89-89FA-E0FA-75003B50B2C4}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{4747A7DF-3B89-89FA-E0FA-75003B50B2C4}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {5C8192E4-A95F-45CE-964F-8CE1553A0CB2}
	EndGlobalSection
EndGlobal
```

---

### 📄 `appsettings.Development.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

### 📄 `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Jwt": {
  "Key": "",
  "Issuer": "http://localhost:8080",
  "Audience": "http://localhost:8080"
},
  "AllowedHosts": "*"
}
```

---

### 📄 `Авторизация.md`

```markdown
Авторизация. И на сервере и на клиенте.

Разные права у пользователей.

Всё в отдельной базе.
```

---

### 📄 `.config/dotnet-tools.json`

```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "dotnet-aspnet-codegenerator": {
      "version": "8.0.6",
      "commands": [
        "dotnet-aspnet-codegenerator"
      ],
      "rollForward": false
    }
  }
}
```

---

### 📄 `Controllers/AuthController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoListAPI.Services;
using TodoListAPI.Models.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.RegisterUserAsync(model.Email, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Пользователь создан!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _authService.LoginUserAsync(model.Email, model.Password);

            if (token != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(2)
                };

                Response.Cookies.Append("jwtToken", token, cookieOptions);

                return Ok(new { Message = "Успешный вход в систему" });
            }

            return Unauthorized(new { Message = "Неверные учетные данные" });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return Ok(new { Message = "Выход выполнен успешно" });
        }
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // или Sub

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            return Ok(new { email = userEmail, id = userId });
        }
    }
}
```

---

### 📄 `Controllers/ProjectsController.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;
using Microsoft.AspNetCore.Authorization;
namespace TodoListAPI.Controllers
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

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.IdProject }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
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
```

---

### 📄 `Controllers/StatussController.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;
using Microsoft.AspNetCore.Authorization;
namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatussController : ControllerBase
    {
        private readonly TodoListDbContext _context;

        public StatussController(TodoListDbContext context)
        {
            _context = context;
        }

        // GET: api/Statuss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Status>>> GetStatuses()
        {
            return await _context.Statuses.ToListAsync();
        }

        // GET: api/Statuss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Status>> GetStatus(int id)
        {
            var status = await _context.Statuses.FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return status;
        }

        // PUT: api/Statuss/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus(int id, Status status)
        {
            if (id != status.IdStatus)
            {
                return BadRequest();
            }

            _context.Entry(status).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
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

        // POST: api/Statuss
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Status>> PostStatus(Status status)
        {
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatus", new { id = status.IdStatus }, status);
        }

        // DELETE: api/Statuss/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatusExists(int id)
        {
            return _context.Statuses.Any(e => e.IdStatus == id);
        }
    }
}
```

---

### 📄 `Controllers/TasksController.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Task = TodoListAPI.Models.Task;


namespace TodoListAPI.Controllers
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

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask(Task task)
        {
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

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.IdTask == id);
        }
    }
}
```

---

### 📄 `Controllers/UsersController.cs`

```csharp
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
```

---

### 📄 `Generators/GeneratorController.cs`

```csharp
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
            public string? Table { get; set; }
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
        public IActionResult StartGeneration([FromBody] GenerationRequest request)
        {
            // Тут у нас будет универсальный вызов генераций
        }

    }


}
```

---

### 📄 `Generators/ProjectsGenerator.cs`

```csharp

```

---

### 📄 `Generators/TaskGenerator.cs`

```csharp
// Generators/TaskGenerator.cs
using System;
using System.IO;
using Microsoft.CodeAnalysis.Diagnostics;
using TodoListAPI.Controllers;
using TodoListAPI.Models;

namespace TodoListAPI.Generators
{
    public class DataGeneratorTask
    {
        private string[]? _fileData;

        private static readonly Random _random = new Random();

        public void ReadData()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "Generators", "Files", "Tasks.md");

            _fileData = File.ReadAllLines(filePath);

        }
        public string GetTask()
        {
            if (_fileData == null)
            {
                ReadData();
            }

            int taskIndex = _random.Next(_fileData!.Length);
            return _fileData[taskIndex];
        }

        public void Generate(int CntGen)
        {
            
        }

    }
}
```

---

### 📄 `Generators/TeamsGenerator.cs`

```csharp

```

---

### 📄 `Generators/UserGenerator.cs`

```csharp

```

---

### 📄 `Generators/Files/Projects.md`

```markdown
Запуск нового продукта на рынок
Полный редизайн и перезапуск корпоративного сайта
Организация ежегодной клиентской конференции "Future Forward 2026"
Внедрение системы электронного документооборота (СЭД)
Выход на рынок стран Юго-Восточной Азии
Построение корпоративного хранилища данных (DWH) и внедрение BI-аналитики
Разработка и запуск программы лояльности для клиентов
Сертификация производства по стандарту качества ISO 9001
Создание единого омниканального центра поддержки клиентов
Разработка и внедрение ESG-стратегии компании
Автоматизация процесса подбора и найма персонала (внедрение ATS)
Слияние и поглощение компании "Конкурент-Плюс" (проект M&A)
```

---

### 📄 `Generators/Files/Tasks.md`

```markdown
Сформировать квартальный финансовый отчет
Рассчитать заработную плату сотрудников за Октябрь
Обработать банковские выписки от 28.09.2025
Проверить контрагента "ООО Ромашка" на благонадежность
Подготовить счета-фактуры для клиента №789
Согласовать бюджет для нового маркетингового проекта
Провести инвентаризацию основных средств
Оформить возврат НДС за 3 квартал
Загрузить платежные поручения в систему 1С
Утвердить авансовый отчет менеджера Иванова
Подготовить коммерческое предложение для "Альфа-групп"
Обзвонить базу "холодных" клиентов
Внести данные о новом лиде в CRM-систему
Провести демонстрацию продукта для потенциального клиента
Составить отчет по воронке продаж за неделю
Согласовать договор поставки с "ТехноПром"
Продлить подписку на сервис для клиента №123
Обработать входящие заявки с сайта
Запланировать встречу с ключевым клиентом
Актуализировать контактные данные в базе клиентов
Запустить рекламную кампанию в Google Ads
Подготовить контент-план для социальных сетей на Ноябрь
Написать пресс-релиз о выпуске нового продукта
Организовать участие в отраслевой выставке "Экспо-2025"
Проанализировать эффективность email-рассылки
Разработать макеты для новой рекламной брошюры
Провести A/B тестирование посадочной страницы
Заказать сувенирную продукцию с логотипом компании
Обновить информацию в разделе "Новости" на корпоративном сайте
Согласовать статью для публикации в отраслевом журнале
Развернуть тестовый сервер для нового приложения
Провести плановое обновление корпоративной почты
Устранить ошибку авторизации в личном кабинете
Провести резервное копирование базы данных
Настроить права доступа для нового сотрудника
Протестировать новый функционал CRM-системы
Осуществить миграцию данных на новый хостинг
Установить SSL-сертификат на домен компании
Настроить интеграцию с платежным шлюзом
Провести аудит информационной безопасности
Опубликовать вакансию "Ведущий разработчик"
Провести первичное собеседование с кандидатом Сидоровым
Оформить приказ о приеме на работу нового сотрудника
Организовать корпоративный тренинг по тайм-менеджменту
Составить график отпусков на следующий год
Провести оценку производительности сотрудников (performance review)
Подготовить документы для оформления ДМС
Рассчитать премию по итогам квартала
Организовать выездное корпоративное мероприятие
Сформировать кадровый резерв на руководящие позиции
Согласование договора аренды офиса
Заказать канцелярские товары на склад
Организовать командировку в филиал г. Санкт-Петербург
Подготовить презентацию для совета директоров
Забронировать переговорную комнату для встречи
Ответить на официальный запрос от партнёров
Проверить и оплатить счета за коммунальные услуги
Организовать доставку корреспонденции
Провести плановый инструктаж по технике безопасности
Разработать регламент по работе с входящими документами
Разработать устав проекта "Сигма"
Провести стартовое совещание (kick-off) с командой
Создать декомпозицию работ (WBS) для проекта
Оценить риски и составить план реагирования
Распределить задачи в Asana/Jira
Провести еженедельный статус-митинг по проекту
Подготовить отчет для управляющего комитета
Согласовать изменение объема работ с заказчиком
Провести демонстрацию прототипа стейкхолдерам
Подготовить итоговую документацию по завершению проекта
Провести правовую экспертизу договора с поставщиком
Подготовить исковое заявление в арбитражный суд
Зарегистрировать новый товарный знак
Внести изменения в уставные документы общества
Подготовить доверенность на представителя в суде
Проконсультировать отдел маркетинга по закону "О рекламе"
Ответить на официальную претензию от клиента
Разработать пользовательское соглашение для нового сервиса
Провести мониторинг изменений в трудовом законодательстве
Согласовать политику обработки персональных данных (GDPR/ФЗ-152)
Разместить заказ на поставку комплектующих
Отследить статус доставки груза №AF-7812
Провести тендер на выбор транспортной компании
Оптимизировать маршруты доставки для сокращения издержек
Оформить таможенную декларацию на импорт
Провести инвентаризацию склада №3
Провести переговоры с поставщиком о снижении цен
Составить план закупок на I квартал
Проанализировать неликвидные складские остатки
Оформить возврат бракованной партии товара
Провести плановое техническое обслуживание станка №14
Разработать технологическую карту для нового изделия
Проконтролировать качество выпускаемой продукции
Запустить в производство партию товара "Модель-X"
Проанализировать причины возникновения брака на линии
Провести инструктаж по технике безопасности для новой смены
Оптимизировать производственный процесс для повышения эффективности
Составить заявку на закупку нового оборудования
Протестировать опытный образец продукта
Внедрить систему менеджмента качества ISO 9001
Провести регрессионное тестирование нового релиза приложения
Написать тест-кейсы для нового функционала "Личный кабинет"
Зарегистрировать дефект №5821 в баг-трекинговой системе
Провести нагрузочное тестирование сервера
Проверить исправление критической уязвимости
Разработать план автоматизации тестирования
Проанализировать отчет о результатах тестирования
Провести аудит соответствия продукта техническому заданию
Проверить локализацию интерфейса на немецкий язык
Составить матрицу покрытия требований тестами
Обработать тикет №TICK-9901 от VIP-клиента
Ответить на запрос об интеграции через API
Классифицировать и эскалировать проблему с сервером разработчикам
Обновить статью в базе знаний "Как сбросить пароль"
Провести телефонную консультацию по настройке оборудования
Проанализировать статистику обращений за неделю
Составить отчет об уровне удовлетворенности клиентов (CSAT)
Позвонить клиенту для подтверждения решения проблемы
Подготовить шаблон ответа на часто задаваемый вопрос
Провести обучение стажера по работе с Help Desk системой
Разработать парадигму синергетического взаимодействия
Провести мозговой штурм по поиску "голубого океана"
Синхронизировать часы перед началом квантового скачка
Выйти из зоны комфорта и войти в зону турбулентности
Сформулировать миссию для миссии компании
Провести стратегическую сессию по диверсификации рисков прокрастинации
Декомпозировать невыполнимую задачу на серию невозможных
Назначить ответственного за поиск виноватых
Мыслить нестандартно в рамках утвержденных стандартов
Создать дорожную карту по выходу из тупика
Провести ритуал изгнания багов из продакшена
Умиротворить разбушевавшийся сервер с помощью перезагрузки
Убедить принтер, что ему действительно нужно печатать
Найти и обезвредить причину, почему "вчера всё работало"
Погадать на кофейной гуще о сроках сдачи проекта
Перевести ТЗ с языка "бизнеса" на язык "разработчиков"
Настроить Wi-Fi так, чтобы он ловил даже в мыслях
Провести обряд очищения кэша
Написать код, который поймет даже твой кот (но это не точно)
Закрыть 40 вкладок в браузере для ментальной перезагрузки
Назначить всем сотрудникам хорошее настроение приказом
Организовать обязательный турнир по кикеру для укрепления командного духа
Найти "ниндзя-разработчика" с 15-летним опытом в технологии, которой 3 года
Разработать KPI для офисного фикуса
Напомнить всем, что мы не просто коллеги, а одна большая семья (особенно в переработки)
Провести тренинг по борьбе с выгоранием с помощью раскрасок
Составить список именинников и не забыть купить торт
Организовать "День комплиментов" в Slack-чате
Провести выходное интервью с увольняющимся, чтобы узнать все сплетни
Обновить описание вакансии, добавив слова "динамичная" и "амбициозная"
Сделать логотип на 10% крупнее
Найти идеальный оттенок синего, который повысит конверсию на 200%
Написать текст, который будет одновременно "продающим", "душевным" и "инновационным"
Создать вирусный ролик с котиками и нашим продуктом
Провести A/B/C/D/E-тестирование кнопки "Зарегистрироваться"
Придумать слоган, который изменит мир (или хотя бы наш квартальный отчет)
Запустить челлендж в TikTok, чтобы показать, что мы "на волне"
Заказать мерч, который сотрудники действительно будут носить
Добавить больше блеска и градиентов в презентацию
Убедить всех, что "миллениалы это оценят"
Вычислить, кто постоянно забирает последнюю капсулу кофе
Провести расследование по делу об исчезновении еды из холодильника
Возглавить экспедицию на склад в поисках нужной коробки
Полить фикус по имени Игорь
Выиграть спор за право управлять кондиционером
Организовать тайный сбор средств на подарок боссу
Найти свободную переговорку в час пик
Перезапустить роутер втайне от сисадмина
Мастерски сделать вид, что очень занят, когда мимо проходит начальник
Стать хранителем негласных правил офисной кухни
```

---

### 📄 `Generators/Files/Teams.md`

```markdown
Финансовый департамент
Финансово-экономический отдел
Департамент продаж
Коммерческий отдел
Департамент маркетинга и коммуникаций
Департамент информационных технологий (IT)
Департамент по управлению персоналом (HR)
Административно-хозяйственный отдел (АХО)
Проектный офис
Департамент управления проектами (PMO)
Юридический департамент
Отдел закупок и логистики
Производственно-технический отдел
Отдел контроля качества (ОКК / QA)
Служба технической поддержки клиентов
Дополнительные и специализированные отделы
Отдел исследований и разработок (R&D)
Отдел разработки ПО
Департамент стратегического развития
Отдел аналитики
Отдел по работе с ключевыми клиентами (Key Account Management)
Служба безопасности
Отдел Кибербезопасности
Отдел внутреннего аудита и контроля
Департамент эксплуатации и инфраструктуры
Отдел по связям с общественностью (PR)
Отдел обучения и развития персонала
```

---

### 📄 `Generators/Files/Users/Female/first.md`

```markdown

```

---

### 📄 `Generators/Files/Users/Female/second.md`

```markdown

```

---

### 📄 `Generators/Files/Users/Female/third.md`

```markdown

```

---

### 📄 `Generators/Files/Users/Male/first.md`

```markdown

```

---

### 📄 `Generators/Files/Users/Male/second.md`

```markdown

```

---

### 📄 `Generators/Files/Users/Male/third.md`

```markdown

```

---

### 📄 `Migrations/20250929211742_InitialCreate.Designer.cs`

```csharp
﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoListAPI.Models;

#nullable disable

namespace TodoListAPI.Migrations
{
    [DbContext(typeof(TodoListDbContext))]
    [Migration("20250929211742_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TodoListAPI.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdUserStatus")
                        .HasColumnType("int");

                    b.Property<int?>("IdUserStatusNavigationIdStatus")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatronymicName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("RegistrationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecondName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("IdUserStatusNavigationIdStatus");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("TodoListAPI.Models.Project", b =>
                {
                    b.Property<int>("IdProject")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_project");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProject"));

                    b.Property<string>("CreatedAt")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("created_at")
                        .IsFixedLength();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("created_by")
                        .IsFixedLength();

                    b.Property<string>("Descryption")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("descryption");

                    b.Property<string>("EditedAt")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("edited_at")
                        .IsFixedLength();

                    b.Property<string>("EditedBy")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("edited_by")
                        .IsFixedLength();

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime")
                        .HasColumnName("end_date");

                    b.Property<int?>("IdTeam")
                        .HasColumnType("int")
                        .HasColumnName("id_team");

                    b.Property<string>("Notes")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("notes")
                        .IsFixedLength();

                    b.Property<string>("ProjectName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("project_name");

                    b.Property<string>("ProjectType")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("project_type");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime")
                        .HasColumnName("start_date");

                    b.HasKey("IdProject")
                        .HasName("PK_Проекты");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("TodoListAPI.Models.Status", b =>
                {
                    b.Property<int>("IdStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id-status");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdStatus"));

                    b.Property<string>("Название")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("IdStatus")
                        .HasName("PK_Статус");

                    b.ToTable("Status", (string)null);
                });

            modelBuilder.Entity("TodoListAPI.Models.Task", b =>
                {
                    b.Property<int>("IdTask")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_task");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTask"));

                    b.Property<DateTime?>("CompleteDate")
                        .HasColumnType("datetime")
                        .HasColumnName("complete_date");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int")
                        .HasColumnName("created_by");

                    b.Property<DateTime?>("DeadlineDate")
                        .HasColumnType("datetime")
                        .HasColumnName("deadline_date");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("description");

                    b.Property<DateTime?>("EditedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("edited_at");

                    b.Property<string>("EditedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("edited_by");

                    b.Property<int?>("IdProject")
                        .HasColumnType("int")
                        .HasColumnName("id_project");

                    b.Property<string>("Notes")
                        .HasMaxLength(50)
                        .HasColumnType("nchar(50)")
                        .HasColumnName("notes")
                        .IsFixedLength();

                    b.Property<string>("Priority")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("priority");

                    b.Property<string>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("status");

                    b.Property<string>("TaskName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("task_name");

                    b.HasKey("IdTask")
                        .HasName("PK_Задачи");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TodoListAPI.Models.TasksProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdProject")
                        .HasColumnType("int")
                        .HasColumnName("id-project");

                    b.Property<int>("IdTask")
                        .HasColumnType("int")
                        .HasColumnName("id-task");

                    b.HasKey("Id")
                        .HasName("PK_ЗадачаПроект");

                    b.HasIndex("IdProject");

                    b.HasIndex("IdTask");

                    b.ToTable("Tasks - Projects", (string)null);
                });

            modelBuilder.Entity("TodoListAPI.Models.TasksUser", b =>
                {
                    b.Property<string>("IdAssignees")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("id_assignees");

                    b.Property<int>("IdTask")
                        .HasColumnType("int")
                        .HasColumnName("id_task");

                    b.Property<string>("IdUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id_user");

                    b.HasKey("IdAssignees")
                        .HasName("PK_Задачи - Пользователи");

                    b.HasIndex("IdTask");

                    b.HasIndex("IdUser");

                    b.ToTable("Tasks - Users", (string)null);
                });

            modelBuilder.Entity("TodoListAPI.Models.Team", b =>
                {
                    b.Property<int>("IdTeam")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_team");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTeam"));

                    b.Property<string>("CratedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("crated_by");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<string>("EditedAt")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("edited_at");

                    b.Property<string>("EditedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("edited_by");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("notes");

                    b.Property<string>("TeamName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("team_name");

                    b.Property<string>("UserAccess")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("user_access");

                    b.HasKey("IdTeam")
                        .HasName("PK_Команды");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("TodoListAPI.Models.UsersCommand", b =>
                {
                    b.Property<int>("IdConnection")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_connection");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdConnection"));

                    b.Property<int>("IdTeam")
                        .HasColumnType("int")
                        .HasColumnName("id_team");

                    b.Property<string>("IdUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id_user");

                    b.HasKey("IdConnection")
                        .HasName("PK_Пользователи - Команды");

                    b.HasIndex("IdTeam");

                    b.HasIndex("IdUser");

                    b.ToTable("Users - Commands", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TodoListAPI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TodoListAPI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoListAPI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TodoListAPI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TodoListAPI.Models.ApplicationUser", b =>
                {
                    b.HasOne("TodoListAPI.Models.Status", "IdUserStatusNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdUserStatusNavigationIdStatus");

                    b.Navigation("IdUserStatusNavigation");
                });

            modelBuilder.Entity("TodoListAPI.Models.TasksProject", b =>
                {
                    b.HasOne("TodoListAPI.Models.Project", "IdProjectNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdProject")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Проекты");

                    b.HasOne("TodoListAPI.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Задачи");

                    b.Navigation("IdProjectNavigation");

                    b.Navigation("IdTaskNavigation");
                });

            modelBuilder.Entity("TodoListAPI.Models.TasksUser", b =>
                {
                    b.HasOne("TodoListAPI.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Задачи");

                    b.HasOne("TodoListAPI.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Пользователи");

                    b.Navigation("IdTaskNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("TodoListAPI.Models.UsersCommand", b =>
                {
                    b.HasOne("TodoListAPI.Models.Team", "IdTeamNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdTeam")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Команды");

                    b.HasOne("TodoListAPI.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Пользователи");

                    b.Navigation("IdTeamNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("TodoListAPI.Models.ApplicationUser", b =>
                {
                    b.Navigation("TasksUsers");

                    b.Navigation("UsersCommands");
                });

            modelBuilder.Entity("TodoListAPI.Models.Project", b =>
                {
                    b.Navigation("TasksProjects");
                });

            modelBuilder.Entity("TodoListAPI.Models.Status", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("TodoListAPI.Models.Task", b =>
                {
                    b.Navigation("TasksProjects");

                    b.Navigation("TasksUsers");
                });

            modelBuilder.Entity("TodoListAPI.Models.Team", b =>
                {
                    b.Navigation("UsersCommands");
                });
#pragma warning restore 612, 618
        }
    }
}
```

---

### 📄 `Migrations/20250929211742_InitialCreate.cs`

```csharp
﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    id_project = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_team = table.Column<int>(type: "int", nullable: true),
                    project_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    project_type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    descryption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    created_at = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    edited_by = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    edited_at = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    notes = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Проекты", x => x.id_project);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    idstatus = table.Column<int>(name: "id-status", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Название = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Статус", x => x.idstatus);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    id_task = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    task_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    priority = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deadline_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    complete_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    id_project = table.Column<int>(type: "int", nullable: true),
                    edited_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    edited_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    notes = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Задачи", x => x.id_task);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    id_team = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    team_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_access = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    crated_by = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    edited_at = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    edited_by = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Команды", x => x.id_team);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatronymicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUserStatus = table.Column<int>(type: "int", nullable: true),
                    IdUserStatusNavigationIdStatus = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Status_IdUserStatusNavigationIdStatus",
                        column: x => x.IdUserStatusNavigationIdStatus,
                        principalTable: "Status",
                        principalColumn: "id-status");
                });

            migrationBuilder.CreateTable(
                name: "Tasks - Projects",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idtask = table.Column<int>(name: "id-task", type: "int", nullable: false),
                    idproject = table.Column<int>(name: "id-project", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ЗадачаПроект", x => x.id);
                    table.ForeignKey(
                        name: "FK_Задачи - Проекты_Задачи",
                        column: x => x.idtask,
                        principalTable: "Tasks",
                        principalColumn: "id_task");
                    table.ForeignKey(
                        name: "FK_Задачи - Проекты_Проекты",
                        column: x => x.idproject,
                        principalTable: "Projects",
                        principalColumn: "id_project");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks - Users",
                columns: table => new
                {
                    id_assignees = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    id_task = table.Column<int>(type: "int", nullable: false),
                    id_user = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Задачи - Пользователи", x => x.id_assignees);
                    table.ForeignKey(
                        name: "FK_Задачи - Пользователи_Задачи",
                        column: x => x.id_task,
                        principalTable: "Tasks",
                        principalColumn: "id_task");
                    table.ForeignKey(
                        name: "FK_Задачи - Пользователи_Пользователи",
                        column: x => x.id_user,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users - Commands",
                columns: table => new
                {
                    id_connection = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_user = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    id_team = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Пользователи - Команды", x => x.id_connection);
                    table.ForeignKey(
                        name: "FK_Пользователи - Команды_Команды",
                        column: x => x.id_team,
                        principalTable: "Teams",
                        principalColumn: "id_team");
                    table.ForeignKey(
                        name: "FK_Пользователи - Команды_Пользователи",
                        column: x => x.id_user,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdUserStatusNavigationIdStatus",
                table: "AspNetUsers",
                column: "IdUserStatusNavigationIdStatus");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks - Projects_id-project",
                table: "Tasks - Projects",
                column: "id-project");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks - Projects_id-task",
                table: "Tasks - Projects",
                column: "id-task");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks - Users_id_task",
                table: "Tasks - Users",
                column: "id_task");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks - Users_id_user",
                table: "Tasks - Users",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Users - Commands_id_team",
                table: "Users - Commands",
                column: "id_team");

            migrationBuilder.CreateIndex(
                name: "IX_Users - Commands_id_user",
                table: "Users - Commands",
                column: "id_user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Tasks - Projects");

            migrationBuilder.DropTable(
                name: "Tasks - Users");

            migrationBuilder.DropTable(
                name: "Users - Commands");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
```

---

### 📄 `Migrations/TodoListDbContextModelSnapshot.cs`

```csharp
﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoListAPI.Models;

#nullable disable

namespace TodoListAPI.Migrations
{
    [DbContext(typeof(TodoListDbContext))]
    partial class TodoListDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TodoListAPI.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdUserStatus")
                        .HasColumnType("int");

                    b.Property<int?>("IdUserStatusNavigationIdStatus")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatronymicName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("RegistrationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecondName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("IdUserStatusNavigationIdStatus");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("TodoListAPI.Models.Project", b =>
                {
                    b.Property<int>("IdProject")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_project");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProject"));

                    b.Property<string>("CreatedAt")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("created_at")
                        .IsFixedLength();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("created_by")
                        .IsFixedLength();

                    b.Property<string>("Descryption")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("descryption");

                    b.Property<string>("EditedAt")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("edited_at")
                        .IsFixedLength();

                    b.Property<string>("EditedBy")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("edited_by")
                        .IsFixedLength();

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime")
                        .HasColumnName("end_date");

                    b.Property<int?>("IdTeam")
                        .HasColumnType("int")
                        .HasColumnName("id_team");

                    b.Property<string>("Notes")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("notes")
                        .IsFixedLength();

                    b.Property<string>("ProjectName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("project_name");

                    b.Property<string>("ProjectType")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("project_type");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime")
                        .HasColumnName("start_date");

                    b.HasKey("IdProject")
                        .HasName("PK_Проекты");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("TodoListAPI.Models.Status", b =>
                {
                    b.Property<int>("IdStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id-status");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdStatus"));

                    b.Property<string>("Название")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("IdStatus")
                        .HasName("PK_Статус");

                    b.ToTable("Status", (string)null);
                });

            modelBuilder.Entity("TodoListAPI.Models.Task", b =>
                {
                    b.Property<int>("IdTask")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_task");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTask"));

                    b.Property<DateTime?>("CompleteDate")
                        .HasColumnType("datetime")
                        .HasColumnName("complete_date");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int")
                        .HasColumnName("created_by");

                    b.Property<DateTime?>("DeadlineDate")
                        .HasColumnType("datetime")
                        .HasColumnName("deadline_date");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("description");

                    b.Property<DateTime?>("EditedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("edited_at");

                    b.Property<string>("EditedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("edited_by");

                    b.Property<int?>("IdProject")
                        .HasColumnType("int")
                        .HasColumnName("id_project");

                    b.Property<string>("Notes")
                        .HasMaxLength(50)
                        .HasColumnType("nchar(50)")
                        .HasColumnName("notes")
                        .IsFixedLength();

                    b.Property<string>("Priority")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("priority");

                    b.Property<string>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("status");

                    b.Property<string>("TaskName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("task_name");

                    b.HasKey("IdTask")
                        .HasName("PK_Задачи");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TodoListAPI.Models.TasksProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdProject")
                        .HasColumnType("int")
                        .HasColumnName("id-project");

                    b.Property<int>("IdTask")
                        .HasColumnType("int")
                        .HasColumnName("id-task");

                    b.HasKey("Id")
                        .HasName("PK_ЗадачаПроект");

                    b.HasIndex("IdProject");

                    b.HasIndex("IdTask");

                    b.ToTable("Tasks - Projects", (string)null);
                });

            modelBuilder.Entity("TodoListAPI.Models.TasksUser", b =>
                {
                    b.Property<string>("IdAssignees")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("id_assignees");

                    b.Property<int>("IdTask")
                        .HasColumnType("int")
                        .HasColumnName("id_task");

                    b.Property<string>("IdUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id_user");

                    b.HasKey("IdAssignees")
                        .HasName("PK_Задачи - Пользователи");

                    b.HasIndex("IdTask");

                    b.HasIndex("IdUser");

                    b.ToTable("Tasks - Users", (string)null);
                });

            modelBuilder.Entity("TodoListAPI.Models.Team", b =>
                {
                    b.Property<int>("IdTeam")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_team");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTeam"));

                    b.Property<string>("CratedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("crated_by");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<string>("EditedAt")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("edited_at");

                    b.Property<string>("EditedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("edited_by");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("notes");

                    b.Property<string>("TeamName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("team_name");

                    b.Property<string>("UserAccess")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("user_access");

                    b.HasKey("IdTeam")
                        .HasName("PK_Команды");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("TodoListAPI.Models.UsersCommand", b =>
                {
                    b.Property<int>("IdConnection")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_connection");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdConnection"));

                    b.Property<int>("IdTeam")
                        .HasColumnType("int")
                        .HasColumnName("id_team");

                    b.Property<string>("IdUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id_user");

                    b.HasKey("IdConnection")
                        .HasName("PK_Пользователи - Команды");

                    b.HasIndex("IdTeam");

                    b.HasIndex("IdUser");

                    b.ToTable("Users - Commands", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TodoListAPI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TodoListAPI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoListAPI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TodoListAPI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TodoListAPI.Models.ApplicationUser", b =>
                {
                    b.HasOne("TodoListAPI.Models.Status", "IdUserStatusNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdUserStatusNavigationIdStatus");

                    b.Navigation("IdUserStatusNavigation");
                });

            modelBuilder.Entity("TodoListAPI.Models.TasksProject", b =>
                {
                    b.HasOne("TodoListAPI.Models.Project", "IdProjectNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdProject")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Проекты");

                    b.HasOne("TodoListAPI.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Задачи");

                    b.Navigation("IdProjectNavigation");

                    b.Navigation("IdTaskNavigation");
                });

            modelBuilder.Entity("TodoListAPI.Models.TasksUser", b =>
                {
                    b.HasOne("TodoListAPI.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Задачи");

                    b.HasOne("TodoListAPI.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Пользователи");

                    b.Navigation("IdTaskNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("TodoListAPI.Models.UsersCommand", b =>
                {
                    b.HasOne("TodoListAPI.Models.Team", "IdTeamNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdTeam")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Команды");

                    b.HasOne("TodoListAPI.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Пользователи");

                    b.Navigation("IdTeamNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("TodoListAPI.Models.ApplicationUser", b =>
                {
                    b.Navigation("TasksUsers");

                    b.Navigation("UsersCommands");
                });

            modelBuilder.Entity("TodoListAPI.Models.Project", b =>
                {
                    b.Navigation("TasksProjects");
                });

            modelBuilder.Entity("TodoListAPI.Models.Status", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("TodoListAPI.Models.Task", b =>
                {
                    b.Navigation("TasksProjects");

                    b.Navigation("TasksUsers");
                });

            modelBuilder.Entity("TodoListAPI.Models.Team", b =>
                {
                    b.Navigation("UsersCommands");
                });
#pragma warning restore 612, 618
        }
    }
}
```

---

### 📄 `Models/ApplicationUser.cs`

```csharp
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic; // Добавьте этот using

namespace TodoListAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? PatronymicName { get; set; }
        public DateTime? RegistrationTime { get; set; }
        public string? Notes { get; set; }
        public int? IdUserStatus { get; set; }
        public virtual Status? IdUserStatusNavigation { get; set; }
        public virtual ICollection<TasksUser> TasksUsers { get; set; } = new List<TasksUser>();
        public virtual ICollection<UsersCommand> UsersCommands { get; set; } = new List<UsersCommand>();
    }
}
```

---

### 📄 `Models/Project.cs`

```csharp
﻿using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class Project
{
    public int IdProject { get; set; }

    public int? IdTeam { get; set; }

    public string? ProjectName { get; set; }

    public string? ProjectType { get; set; }

    public string? Descryption { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? CreatedBy { get; set; }

    public string? CreatedAt { get; set; }

    public string? EditedBy { get; set; }

    public string? EditedAt { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<TasksProject> TasksProjects { get; set; } = new List<TasksProject>();
}
```

---

### 📄 `Models/Status.cs`

```csharp
﻿using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

// Models/Status.cs
public partial class Status
{
    public int IdStatus { get; set; }
    public string? Название { get; set; }
    public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>(); // <--- ИЗМЕНИТЬ НА ApplicationUser
}
```

---

### 📄 `Models/Task.cs`

```csharp
﻿using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class Task
{
    public int IdTask { get; set; }

    public string? TaskName { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? Priority { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeadlineDate { get; set; }

    public DateTime? CompleteDate { get; set; }

    public int? IdProject { get; set; }

    public string? EditedBy { get; set; }

    public DateTime? EditedAt { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<TasksProject> TasksProjects { get; set; } = new List<TasksProject>();

    public virtual ICollection<TasksUser> TasksUsers { get; set; } = new List<TasksUser>();
}
```

---

### 📄 `Models/TasksProject.cs`

```csharp
﻿using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class TasksProject
{
    public int Id { get; set; }

    public int IdTask { get; set; }

    public int IdProject { get; set; }

    public virtual Project IdProjectNavigation { get; set; } = null!;

    public virtual Task IdTaskNavigation { get; set; } = null!;
}
```

---

### 📄 `Models/TasksUser.cs`

```csharp
﻿using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class TasksUser
{
    public string IdAssignees { get; set; } = null!;
    public int IdTask { get; set; }
    public string IdUser { get; set; } // <--- ИЗМЕНИТЬ НА STRING
    public virtual Task IdTaskNavigation { get; set; } = null!;
    public virtual ApplicationUser IdUserNavigation { get; set; } = null!; // <--- ИЗМЕНИТЬ НА ApplicationUser
}
```

---

### 📄 `Models/Team.cs`

```csharp
﻿using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class Team
{
    public int IdTeam { get; set; }

    public string? TeamName { get; set; }

    public string? Description { get; set; }

    public string? UserAccess { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CratedBy { get; set; }

    public string? EditedAt { get; set; }

    public string? EditedBy { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<UsersCommand> UsersCommands { get; set; } = new List<UsersCommand>();
}
```

---

### 📄 `Models/TodoListDbContext.cs`

```csharp
﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace TodoListAPI.Models;

public partial class TodoListDbContext : IdentityDbContext<ApplicationUser>
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TasksProject> TasksProjects { get; set; }

    public virtual DbSet<TasksUser> TasksUsers { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<UsersCommand> UsersCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.IdProject).HasName("PK_Проекты");

            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("created_by");
            entity.Property(e => e.Descryption).HasColumnName("descryption");
            entity.Property(e => e.EditedAt)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("edited_at");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("edited_by");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.Notes)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("notes");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(255)
                .HasColumnName("project_name");
            entity.Property(e => e.ProjectType)
                .HasMaxLength(255)
                .HasColumnName("project_type");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK_Статус");

            entity.ToTable("Status");

            entity.Property(e => e.IdStatus).HasColumnName("id-status");
            entity.Property(e => e.Название).HasMaxLength(255);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.IdTask).HasName("PK_Задачи");

            entity.Property(e => e.IdTask).HasColumnName("id_task");
            entity.Property(e => e.CompleteDate)
                .HasColumnType("datetime")
                .HasColumnName("complete_date");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeadlineDate)
                .HasColumnType("datetime")
                .HasColumnName("deadline_date");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EditedAt)
                .HasColumnType("datetime")
                .HasColumnName("edited_at");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(50)
                .HasColumnName("edited_by");
            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.Notes)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("notes");
            entity.Property(e => e.Priority)
                .HasMaxLength(255)
                .HasColumnName("priority");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.TaskName)
                .HasMaxLength(255)
                .HasColumnName("task_name");
        });

        modelBuilder.Entity<TasksProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ЗадачаПроект");

            entity.ToTable("Tasks - Projects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProject).HasColumnName("id-project");
            entity.Property(e => e.IdTask).HasColumnName("id-task");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.TasksProjects)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Проекты_Проекты");

            entity.HasOne(d => d.IdTaskNavigation).WithMany(p => p.TasksProjects)
                .HasForeignKey(d => d.IdTask)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Проекты_Задачи");
        });

        modelBuilder.Entity<TasksUser>(entity =>
        {
            entity.HasKey(e => e.IdAssignees).HasName("PK_Задачи - Пользователи");

            entity.ToTable("Tasks - Users");

            entity.Property(e => e.IdAssignees)
                .HasMaxLength(255)
                .HasColumnName("id_assignees");
            entity.Property(e => e.IdTask).HasColumnName("id_task");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdTaskNavigation).WithMany(p => p.TasksUsers)
                .HasForeignKey(d => d.IdTask)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Пользователи_Задачи");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TasksUsers)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Пользователи_Пользователи");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.IdTeam).HasName("PK_Команды");

            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.CratedBy)
                .HasMaxLength(255)
                .HasColumnName("crated_by");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EditedAt)
                .HasMaxLength(255)
                .HasColumnName("edited_at");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(255)
                .HasColumnName("edited_by");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.TeamName)
                .HasMaxLength(255)
                .HasColumnName("team_name");
            entity.Property(e => e.UserAccess)
                .HasMaxLength(255)
                .HasColumnName("user_access");
        });

        modelBuilder.Entity<UsersCommand>(entity =>
        {
            entity.HasKey(e => e.IdConnection).HasName("PK_Пользователи - Команды");

            entity.ToTable("Users - Commands");

            entity.Property(e => e.IdConnection).HasColumnName("id_connection");
            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdTeamNavigation).WithMany(p => p.UsersCommands)
                .HasForeignKey(d => d.IdTeam)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Пользователи - Команды_Команды");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UsersCommands)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Пользователи - Команды_Пользователи");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
```

---

### 📄 `Models/UsersCommand.cs`

```csharp
﻿using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

// Models/UsersCommand.cs
public partial class UsersCommand
{
    public int IdConnection { get; set; }
    public string IdUser { get; set; } 
    public int IdTeam { get; set; }
    public virtual Team IdTeamNavigation { get; set; } = null!;
    public virtual ApplicationUser IdUserNavigation { get; set; } = null!;
}
```

---

### 📄 `Models/DTO/LoginModel.cs`

```csharp
namespace TodoListAPI.Models.DTO
{
    public class LoginModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
```

---

### 📄 `Models/DTO/RegiserModel.cs`

```csharp
namespace TodoListAPI.Models.DTO
{
    public class RegisterModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
```

---

### 📄 `Properties/launchSettings.json`

```json
﻿{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:48393",
      "sslPort": 44393
    }
  },
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5023",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7014;http://localhost:5023",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

---

### 📄 `Services/AuthService.cs`

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoListAPI.Models; // Важно, чтобы была ссылка на ApplicationUser

namespace TodoListAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        // Внедряем UserManager для работы с пользователями и IConfiguration для доступа к секретному ключу
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUserAsync(string email, string password)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<string> LoginUserAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                // Логин и пароль верные, генерируем JWT токен
                return GenerateJwtToken(user);
            }

            // Если что-то не так, возвращаем null
            return null;
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
```

---

### 📄 `Services/IAuthService.cs`

```csharp
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TodoListAPI.Services
{
    public interface IAuthService
    {
        // Метод для регистрации пользователя
        Task<IdentityResult> RegisterUserAsync(string email, string password);

        // Метод для входа пользователя (возвращает токен или другой признак успеха)
        Task<string> LoginUserAsync(string email, string password);
    }
}
```

---

### 📄 `utils/gen-controllers.sh`

```bash
cd TodoListAPI

models=("Project" "User" "Command" "Status" "Task")

for model in "${models[@]}"; do
    
    controllerName="${model}sController"
    
    dotnet tool run dotnet-aspnet-codegenerator controller -name $controllerName -api -m $model -dc TodoListDbContext -outDir Controllers
done
```

---

### 📄 `utils/migrate.sh`

```bash
dotnet ef dbcontext scaffold "Server=localhost\MSSQLSERVER01;Database=TodoListDB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --no-onconfiguring
```
