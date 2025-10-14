
---

### 📄 `Backend.csproj`

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
    <PackageReference Include="NLog.Web.AspNetCore" Version="6.0.5" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
    
  </ItemGroup>
  <ItemGroup>
    <Content Include="Generators\Files\**\*.*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
  <ItemGroup>
    <Content Update="nlog.config">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
</Project>
```

---

### 📄 `Backend.sln`

```text
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.5.2.0
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Backend", "Backend.csproj", "{4747A7DF-3B89-89FA-E0FA-75003B50B2C4}"
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

### 📄 `Dockerfile`

```dockerfile
# syntax=docker/dockerfile:1

# Comments are provided throughout this file to help you get started.
# If you need more help, visit the Dockerfile reference guide at
# https://docs.docker.com/go/dockerfile-reference/

# Want to help us make this template better? Share your feedback here: https://forms.gle/ybq9Krt8jtBL3iCk7

################################################################################

# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /src

COPY *.csproj .
RUN dotnet restore

COPY . .

# This is the architecture you’re building for, which is passed in by the builder.
# Placing it here allows the previous steps to be cached across architectures.
ARG TARGETARCH

# Build the application.
# Leverage a cache mount to /root/.nuget/packages so that subsequent builds don't have to re-download packages.
# If TARGETARCH is "amd64", replace it with "x64" - "x64" is .NET's canonical name for this and "amd64" doesn't
#   work in .NET 6.0.
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app .

# Switch to a non-privileged user (defined in the base image) that the app will run under.
# See https://docs.docker.com/go/dockerfile-user-best-practices/
# and https://github.com/dotnet/dotnet-docker/discussions/4764
USER $APP_UID

ENTRYPOINT ["dotnet", "Backend.dll"]
```

---

### 📄 `Program.cs`

```csharp
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Backend.Generators; 
using Task = System.Threading.Tasks.Task;
using NLog;
using NLog.Web;

// Очищаем стандартные провайдеры логирования

var builder = WebApplication.CreateBuilder(args);

// // ТЕСТОВЫЙ БЛОК

// // --- НАЧАЛО: Обновленный вызов генератора ---
// var emailGenerator = new DataGeneratorEmail();
// string filePath = Path.Combine(AppContext.BaseDirectory, "Generators", "Files", "Emails","users.csv");
// emailGenerator.GetRandomUsername(filePath); // <-- Теперь просто один вызов
// return;
// // --- КОНЕЦ ---

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
                  .AllowCredentials(); // localstorage и кукисы
        });
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TodoListDbContext>();
        context.Database.Migrate();
        await DbInitializer.InitializeAsync(services);
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
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

### 📄 `nlog.config`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogLevel="Info"
      internalLogFile="logs/internal-nlog.log">

    <variable name="logLayout" value="${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />

    <targets>
        <target xsi:type="File" name="fileTarget" fileName="logs/app-${shortdate}.log"
                layout="${logLayout}"
                archiveEvery="Day"
                archiveFileName="logs/archives/app.{#}.log"
                archiveNumbering="Date"
                archiveDateFormat="yyyy-MM-dd"
                maxArchiveFiles="7" />

        <target xsi:type="Console" name="consoleTarget" layout="${logLayout}" />
    </targets>

    <rules>
        <logger name="*" minlevel="Debug" writeTo="fileTarget" />
        <logger name="*" minlevel="Info" writeTo="consoleTarget" />
    </rules>
</nlog>
```

---

### 📄 `Авторизация.md`

```markdown
Авторизация. И на сервере и на клиенте.

Разные права у пользователей.

Всё в отдельной базе. 

Генератор: админ
Поставить задачу: модератор
Просмотреть задачи: пользователи
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
using Backend.Services;
using Backend.Models.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace Backend.Controllers
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterUserAsync(model);

            if (result.Succeeded)
            {
                return Ok(new { Message = $"{result}" });
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
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(120)
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

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
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
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
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
namespace Backend.Controllers
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
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Task = Backend.Models.Task;


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
```

---

### 📄 `Controllers/UsersController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TodoListDbContext _context;
        public UsersController(UserManager<ApplicationUser> userManager, TodoListDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithRolesDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(user => new UserWithRolesDto
                {
                    Id = user.Id,
                    ShortName = user.ShortName,
                    Email = user.Email,
                    Gender = user.Gender,
                    Roles = (from userRole in _context.UserRoles
                            join role in _context.Roles on userRole.RoleId equals role.Id
                            where userRole.UserId == user.Id
                            select role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithRolesDto>> GetUser(string id) 
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userWithRoles = new UserWithRolesDto
            {
                Id = user.Id,
                ShortName = user.ShortName,
                Email = user.Email,
                Gender = user.Gender,
                RegistrationTime = user.RegistrationTime,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                Roles = roles
            };

            return Ok(userWithRoles);
        }

        // DELETE: api/Users/DeleteAll
        [HttpDelete("DeleteAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            try
            {
                var users = await _userManager.Users
                .Where(x => x.Email != "admin@admin.ru")
                .ToListAsync();
                foreach (var user in users)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return BadRequest($"Failed to delete user {user.UserName}: {errors}");
                    }
                }
                return Ok($"Successfully deleted {users.Count} users");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("ChangeRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserWithRolesDto>> ChangeUserRole([FromBody] UserRoleDto newRoles)
        {
            var user = await _userManager.FindByIdAsync(newRoles.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to remove user roles");
            }

            result = await _userManager.AddToRolesAsync(user, newRoles.Roles);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to add user roles");
            }

            var updatedRoles = await _userManager.GetRolesAsync(user);

            var userWithRoles = new UserWithRolesDto
            {
                Id = user.Id,
                ShortName = user.ShortName,
                Email = user.Email,
                Gender = user.Gender,
                RegistrationTime = user.RegistrationTime,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                Roles = updatedRoles
            };

            return Ok(userWithRoles);
        }
        // добавить логику создания (POST), обновления (PUT) и удаления (DELETE)

    }
}
```

---

### 📄 `Data/DbInitializer.cs`

```csharp
using Microsoft.AspNetCore.Identity;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "User", "Moderator", "Teamlead" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
```

---

### 📄 `Generators/EmailGenerator.cs`

```csharp
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO; // <-- Изменили using'и
using System.Text;

namespace Backend.Generators
{
    public class DataGeneratorEmail
    {
        private static readonly Random _random = new Random();
        public string GetRandomDomain()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "Generators", "Files", "Emails", "domains.md");
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Warning: File not found: {filePath}");
                return string.Empty;
            }

            try
            {
                string[] fileData = File.ReadAllLines(filePath);
                
                if (fileData.Length == 0)
                {
                    Console.WriteLine($"Warning: File is empty: {filePath}");
                    return string.Empty;
                }
                
                Random random = new Random();
                int randomIndex = random.Next(0, fileData.Length);
                return fileData[randomIndex];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return string.Empty;
            }
        }
        public string GetRandomUsername(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    long length = fileStream.Length;
                    if (length == 0)
                    {
                        Console.WriteLine("Файл с email'ами пуст.");
                        return string.Empty;
                    }

                    long position = (long)(_random.NextDouble() * length);

                    fileStream.Seek(position, SeekOrigin.Begin);

                    using (var streamReader = new StreamReader(fileStream))
                    {
 
                        streamReader.ReadLine();

                        string line = streamReader.ReadLine();


                        if (string.IsNullOrEmpty(line))
                        {
                            fileStream.Seek(0, SeekOrigin.Begin);
                            line = streamReader.ReadLine();
                        }

                        if (string.IsNullOrEmpty(line)) return "Не удалось прочитать строку";


                        string domain = GetRandomDomain();

                        using (var stringReader = new StringReader(line))
                        using (var parser = new TextFieldParser(stringReader))
                        {
                            parser.SetDelimiters(",");
                            parser.HasFieldsEnclosedInQuotes = true;
                            string[] fields = parser.ReadFields();

                            if (fields != null && fields.Length > 0)
                            {
                                string email = fields[0];
                                return $"{email}@{domain}".ToLower();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }

            return "Ошибка";
        }
        
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
```

---

### 📄 `Generators/ProjectsGenerator.cs`

```csharp

```

---

### 📄 `Generators/TaskGenerator.cs`

```csharp
using Backend.Models;

using TaskEntity = Backend.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace Backend.Generators
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

        public async Task Generate(TodoListDbContext context, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var newTask = new TaskEntity
                {
                    TaskName = GetTask(),
                    Description = "Сгенерировано автоматически",
                    CreatedAt = DateTime.UtcNow,
                };
                context.Tasks.Add(newTask);
            }
            await context.SaveChangesAsync();
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
using Backend.Models;

using TaskEntity = Backend.Models.Task;
using Task = System.Threading.Tasks.Task;
using Backend.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Backend.Generators
{
    public class DataGeneratorUser
    {
        public class FullNameList
        {
            public string[]? FirstNames { get; set; }
            public string[]? MiddleNames { get; set; }
            public string[]? LastNames { get; set; }
        }

        public class FullName
        {
            public string? FirstName { get; set; }
            public string? MiddleName { get; set; }
            public string? LastName { get; set; }
            public string? Gender { get; set; }

        }

        private Dictionary<string, FullNameList>? _maleNames;
        private Dictionary<string, FullNameList>? _femaleNames;
        private static readonly Random _random = new Random();

        public void ReadData()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string[] genders = ["Female", "Male"];
            string[] nameTypes = ["first", "second", "third"];

            _maleNames = [];
            _femaleNames = [];

            foreach (string gender in genders)
            {
                foreach (string nameType in nameTypes)
                {
                    string filePath = Path.Combine(baseDirectory, "Generators", "Files", "Users", gender, $"{nameType}.md");
                    
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"Warning: File not found: {filePath}");
                        continue;
                    }

                    string[] fileData = File.ReadAllLines(filePath);
                    
                    var nameList = new FullNameList();
                    
                    switch (nameType)
                    {
                        case "first":
                            nameList.FirstNames = fileData;
                            break;
                        case "second":
                            nameList.MiddleNames = fileData;
                            break;
                        case "third":
                            nameList.LastNames = fileData;
                            break;
                    }

                    if (gender == "Male")
                    {
                        _maleNames[nameType] = nameList;
                    }
                    else
                    {
                        _femaleNames[nameType] = nameList;
                    }
                }
            }
        }

        private string GetRandomElement(string[]? array)
        {
            if (array == null || array.Length == 0)
                return string.Empty;
                
            return array[_random.Next(array.Length)];
        }

        public FullName GenerateRandomName(string gender)
        {
            var names = gender == "Male" ? _maleNames : _femaleNames;

            if (names == null) throw new InvalidOperationException("Data not loaded. Call ReadData() first.");

            var fullName= new FullName();

            fullName.FirstName = GetRandomElement(names["first"].FirstNames);
            fullName.MiddleName = GetRandomElement(names["second"].MiddleNames);
            fullName.LastName = GetRandomElement(names["third"].LastNames);
            
            return fullName;
        }

        public FullName GetUser(string gender)
        {
            if (_maleNames == null || _femaleNames == null)
            {
                ReadData();
            }
            return GenerateRandomName(gender);
        }

        public async Task Generate(
            TodoListDbContext context, 
            int count, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            var roles = await roleManager.Roles
                .Where(r => r.Name != "Admin")
                .Select(r => r.Name)
                .ToListAsync();

            if (roles.Count == 0)
            {
                return; 
            }

            for (int i = 0; i < count; i++)
            {
                var fullName = new FullName();
                Random random = new();
                string gender = random.Next(2) == 0 ? "Female" : "Male";
                
                fullName = GetUser(gender);

                var emailGenerator = new DataGeneratorEmail();
                string filePath = Path.Combine(AppContext.BaseDirectory, "Generators", "Files", "Emails","users.csv");
                
                string email = emailGenerator.GetRandomUsername(filePath);

                string short_first = fullName.FirstName.Substring(0,1); 
                string short_last = fullName.LastName.Substring(0,1);  

                var newUser = new Models.ApplicationUser
                {
                    FirstName = fullName.FirstName,
                    SecondName = fullName.MiddleName,
                    LastName = fullName.LastName,
                    RegistrationTime = DateTime.UtcNow.AddDays(-Random.Shared.Next(0, 365)).AddHours(-Random.Shared.Next(0, 24)),
                    Gender = gender,
                    Email = email,
                    UserName = email, 
                    ShortName = $"{fullName.MiddleName} {short_first}.{short_last}."
                };

                var result = await userManager.CreateAsync(newUser, "GeneratedUser123!");

                if (result.Succeeded)
                {
                    string randomRole = roles[random.Next(roles.Count)];
                    await userManager.AddToRoleAsync(newUser, randomRole);
                }
                else
                {
                    Console.WriteLine($"Ошибка при создании пользователя {newUser.Email}:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }
                }
            }
        }

    }
}
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
using Backend.Models;

#nullable disable

namespace Backend.Migrations
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

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
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

            modelBuilder.Entity("Backend.Models.Project", b =>
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

            modelBuilder.Entity("Backend.Models.Status", b =>
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

            modelBuilder.Entity("Backend.Models.Task", b =>
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

            modelBuilder.Entity("Backend.Models.TasksProject", b =>
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

            modelBuilder.Entity("Backend.Models.TasksUser", b =>
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

            modelBuilder.Entity("Backend.Models.Team", b =>
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

            modelBuilder.Entity("Backend.Models.UsersCommand", b =>
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
                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Backend.Models.ApplicationUser", null)
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

                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
                {
                    b.HasOne("Backend.Models.Status", "IdUserStatusNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdUserStatusNavigationIdStatus");

                    b.Navigation("IdUserStatusNavigation");
                });

            modelBuilder.Entity("Backend.Models.TasksProject", b =>
                {
                    b.HasOne("Backend.Models.Project", "IdProjectNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdProject")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Проекты");

                    b.HasOne("Backend.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Задачи");

                    b.Navigation("IdProjectNavigation");

                    b.Navigation("IdTaskNavigation");
                });

            modelBuilder.Entity("Backend.Models.TasksUser", b =>
                {
                    b.HasOne("Backend.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Задачи");

                    b.HasOne("Backend.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Пользователи");

                    b.Navigation("IdTaskNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Backend.Models.UsersCommand", b =>
                {
                    b.HasOne("Backend.Models.Team", "IdTeamNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdTeam")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Команды");

                    b.HasOne("Backend.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Пользователи");

                    b.Navigation("IdTeamNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
                {
                    b.Navigation("TasksUsers");

                    b.Navigation("UsersCommands");
                });

            modelBuilder.Entity("Backend.Models.Project", b =>
                {
                    b.Navigation("TasksProjects");
                });

            modelBuilder.Entity("Backend.Models.Status", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Backend.Models.Task", b =>
                {
                    b.Navigation("TasksProjects");

                    b.Navigation("TasksUsers");
                });

            modelBuilder.Entity("Backend.Models.Team", b =>
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

namespace Backend.Migrations
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

### 📄 `Migrations/20251008100058_AddShortNameToUser.Designer.cs`

```csharp
﻿// <auto-generated />
using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(TodoListDbContext))]
    [Migration("20251008100058_AddShortNameToUser")]
    partial class AddShortNameToUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
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

                    b.Property<string>("ShortName")
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

            modelBuilder.Entity("Backend.Models.Project", b =>
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

            modelBuilder.Entity("Backend.Models.Status", b =>
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

            modelBuilder.Entity("Backend.Models.Task", b =>
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

            modelBuilder.Entity("Backend.Models.TasksProject", b =>
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

            modelBuilder.Entity("Backend.Models.TasksUser", b =>
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

            modelBuilder.Entity("Backend.Models.Team", b =>
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

            modelBuilder.Entity("Backend.Models.UsersCommand", b =>
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

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
                {
                    b.HasOne("Backend.Models.Status", "IdUserStatusNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdUserStatusNavigationIdStatus");

                    b.Navigation("IdUserStatusNavigation");
                });

            modelBuilder.Entity("Backend.Models.TasksProject", b =>
                {
                    b.HasOne("Backend.Models.Project", "IdProjectNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdProject")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Проекты");

                    b.HasOne("Backend.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Задачи");

                    b.Navigation("IdProjectNavigation");

                    b.Navigation("IdTaskNavigation");
                });

            modelBuilder.Entity("Backend.Models.TasksUser", b =>
                {
                    b.HasOne("Backend.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Задачи");

                    b.HasOne("Backend.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Пользователи");

                    b.Navigation("IdTaskNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Backend.Models.UsersCommand", b =>
                {
                    b.HasOne("Backend.Models.Team", "IdTeamNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdTeam")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Команды");

                    b.HasOne("Backend.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Пользователи");

                    b.Navigation("IdTeamNavigation");

                    b.Navigation("IdUserNavigation");
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
                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Backend.Models.ApplicationUser", null)
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

                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
                {
                    b.Navigation("TasksUsers");

                    b.Navigation("UsersCommands");
                });

            modelBuilder.Entity("Backend.Models.Project", b =>
                {
                    b.Navigation("TasksProjects");
                });

            modelBuilder.Entity("Backend.Models.Status", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Backend.Models.Task", b =>
                {
                    b.Navigation("TasksProjects");

                    b.Navigation("TasksUsers");
                });

            modelBuilder.Entity("Backend.Models.Team", b =>
                {
                    b.Navigation("UsersCommands");
                });
#pragma warning restore 612, 618
        }
    }
}
```

---

### 📄 `Migrations/20251008100058_AddShortNameToUser.cs`

```csharp
﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddShortNameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "AspNetUsers");
        }
    }
}
```

---

### 📄 `Migrations/20251008191012_AddGenderToUser.Designer.cs`

```csharp
﻿// <auto-generated />
using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(TodoListDbContext))]
    [Migration("20251008191012_AddGenderToUser")]
    partial class AddGenderToUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
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

                    b.Property<string>("Gender")
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

                    b.Property<string>("ShortName")
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

            modelBuilder.Entity("Backend.Models.Project", b =>
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

            modelBuilder.Entity("Backend.Models.Status", b =>
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

            modelBuilder.Entity("Backend.Models.Task", b =>
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

            modelBuilder.Entity("Backend.Models.TasksProject", b =>
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

            modelBuilder.Entity("Backend.Models.TasksUser", b =>
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

            modelBuilder.Entity("Backend.Models.Team", b =>
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

            modelBuilder.Entity("Backend.Models.UsersCommand", b =>
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

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
                {
                    b.HasOne("Backend.Models.Status", "IdUserStatusNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdUserStatusNavigationIdStatus");

                    b.Navigation("IdUserStatusNavigation");
                });

            modelBuilder.Entity("Backend.Models.TasksProject", b =>
                {
                    b.HasOne("Backend.Models.Project", "IdProjectNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdProject")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Проекты");

                    b.HasOne("Backend.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Задачи");

                    b.Navigation("IdProjectNavigation");

                    b.Navigation("IdTaskNavigation");
                });

            modelBuilder.Entity("Backend.Models.TasksUser", b =>
                {
                    b.HasOne("Backend.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Задачи");

                    b.HasOne("Backend.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Пользователи");

                    b.Navigation("IdTaskNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Backend.Models.UsersCommand", b =>
                {
                    b.HasOne("Backend.Models.Team", "IdTeamNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdTeam")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Команды");

                    b.HasOne("Backend.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Пользователи");

                    b.Navigation("IdTeamNavigation");

                    b.Navigation("IdUserNavigation");
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
                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Backend.Models.ApplicationUser", null)
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

                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
                {
                    b.Navigation("TasksUsers");

                    b.Navigation("UsersCommands");
                });

            modelBuilder.Entity("Backend.Models.Project", b =>
                {
                    b.Navigation("TasksProjects");
                });

            modelBuilder.Entity("Backend.Models.Status", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Backend.Models.Task", b =>
                {
                    b.Navigation("TasksProjects");

                    b.Navigation("TasksUsers");
                });

            modelBuilder.Entity("Backend.Models.Team", b =>
                {
                    b.Navigation("UsersCommands");
                });
#pragma warning restore 612, 618
        }
    }
}
```

---

### 📄 `Migrations/20251008191012_AddGenderToUser.cs`

```csharp
﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");
        }
    }
}
```

---

### 📄 `Migrations/20251008204415_AddLastNameRenameToUser.Designer.cs`

```csharp
﻿// <auto-generated />
using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(TodoListDbContext))]
    [Migration("20251008204415_AddLastNameRenameToUser")]
    partial class AddLastNameRenameToUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
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

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdUserStatus")
                        .HasColumnType("int");

                    b.Property<int?>("IdUserStatusNavigationIdStatus")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("ShortName")
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

            modelBuilder.Entity("Backend.Models.Project", b =>
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

            modelBuilder.Entity("Backend.Models.Status", b =>
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

            modelBuilder.Entity("Backend.Models.Task", b =>
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

            modelBuilder.Entity("Backend.Models.TasksProject", b =>
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

            modelBuilder.Entity("Backend.Models.TasksUser", b =>
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

            modelBuilder.Entity("Backend.Models.Team", b =>
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

            modelBuilder.Entity("Backend.Models.UsersCommand", b =>
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

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
                {
                    b.HasOne("Backend.Models.Status", "IdUserStatusNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdUserStatusNavigationIdStatus");

                    b.Navigation("IdUserStatusNavigation");
                });

            modelBuilder.Entity("Backend.Models.TasksProject", b =>
                {
                    b.HasOne("Backend.Models.Project", "IdProjectNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdProject")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Проекты");

                    b.HasOne("Backend.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Задачи");

                    b.Navigation("IdProjectNavigation");

                    b.Navigation("IdTaskNavigation");
                });

            modelBuilder.Entity("Backend.Models.TasksUser", b =>
                {
                    b.HasOne("Backend.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Задачи");

                    b.HasOne("Backend.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Пользователи");

                    b.Navigation("IdTaskNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Backend.Models.UsersCommand", b =>
                {
                    b.HasOne("Backend.Models.Team", "IdTeamNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdTeam")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Команды");

                    b.HasOne("Backend.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Пользователи");

                    b.Navigation("IdTeamNavigation");

                    b.Navigation("IdUserNavigation");
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
                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Backend.Models.ApplicationUser", null)
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

                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
                {
                    b.Navigation("TasksUsers");

                    b.Navigation("UsersCommands");
                });

            modelBuilder.Entity("Backend.Models.Project", b =>
                {
                    b.Navigation("TasksProjects");
                });

            modelBuilder.Entity("Backend.Models.Status", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Backend.Models.Task", b =>
                {
                    b.Navigation("TasksProjects");

                    b.Navigation("TasksUsers");
                });

            modelBuilder.Entity("Backend.Models.Team", b =>
                {
                    b.Navigation("UsersCommands");
                });
#pragma warning restore 612, 618
        }
    }
}
```

---

### 📄 `Migrations/20251008204415_AddLastNameRenameToUser.cs`

```csharp
﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddLastNameRenameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatronymicName",
                table: "AspNetUsers",
                newName: "LastName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "PatronymicName");
        }
    }
}
```

---

### 📄 `Migrations/TodoListDbContextModelSnapshot.cs`

```csharp
﻿// <auto-generated />
using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
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

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
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

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdUserStatus")
                        .HasColumnType("int");

                    b.Property<int?>("IdUserStatusNavigationIdStatus")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("ShortName")
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

            modelBuilder.Entity("Backend.Models.Project", b =>
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

            modelBuilder.Entity("Backend.Models.Status", b =>
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

            modelBuilder.Entity("Backend.Models.Task", b =>
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

            modelBuilder.Entity("Backend.Models.TasksProject", b =>
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

            modelBuilder.Entity("Backend.Models.TasksUser", b =>
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

            modelBuilder.Entity("Backend.Models.Team", b =>
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

            modelBuilder.Entity("Backend.Models.UsersCommand", b =>
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

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
                {
                    b.HasOne("Backend.Models.Status", "IdUserStatusNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdUserStatusNavigationIdStatus");

                    b.Navigation("IdUserStatusNavigation");
                });

            modelBuilder.Entity("Backend.Models.TasksProject", b =>
                {
                    b.HasOne("Backend.Models.Project", "IdProjectNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdProject")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Проекты");

                    b.HasOne("Backend.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksProjects")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Проекты_Задачи");

                    b.Navigation("IdProjectNavigation");

                    b.Navigation("IdTaskNavigation");
                });

            modelBuilder.Entity("Backend.Models.TasksUser", b =>
                {
                    b.HasOne("Backend.Models.Task", "IdTaskNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdTask")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Задачи");

                    b.HasOne("Backend.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("TasksUsers")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Задачи - Пользователи_Пользователи");

                    b.Navigation("IdTaskNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Backend.Models.UsersCommand", b =>
                {
                    b.HasOne("Backend.Models.Team", "IdTeamNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdTeam")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Команды");

                    b.HasOne("Backend.Models.ApplicationUser", "IdUserNavigation")
                        .WithMany("UsersCommands")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Пользователи - Команды_Пользователи");

                    b.Navigation("IdTeamNavigation");

                    b.Navigation("IdUserNavigation");
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
                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Backend.Models.ApplicationUser", null)
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

                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Models.ApplicationUser", b =>
                {
                    b.Navigation("TasksUsers");

                    b.Navigation("UsersCommands");
                });

            modelBuilder.Entity("Backend.Models.Project", b =>
                {
                    b.Navigation("TasksProjects");
                });

            modelBuilder.Entity("Backend.Models.Status", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Backend.Models.Task", b =>
                {
                    b.Navigation("TasksProjects");

                    b.Navigation("TasksUsers");
                });

            modelBuilder.Entity("Backend.Models.Team", b =>
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

namespace Backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public string? ShortName { get; set; } 
        public string? Gender { get; set; } 
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

namespace Backend.Models;

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

namespace Backend.Models;

// Models/Status.cs
public partial class Status
{
    public int IdStatus { get; set; }
    public string? Название { get; set; }
    public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}
```

---

### 📄 `Models/Task.cs`

```csharp
﻿using System;
using System.Collections.Generic;

namespace Backend.Models;

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

namespace Backend.Models;

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

namespace Backend.Models;

public partial class TasksUser
{
    public string IdAssignees { get; set; } = null!;
    public int IdTask { get; set; }
    public string IdUser { get; set; }
    public virtual Task IdTaskNavigation { get; set; } = null!;
    public virtual ApplicationUser IdUserNavigation { get; set; } = null!;
}
```

---

### 📄 `Models/Team.cs`

```csharp
﻿using System;
using System.Collections.Generic;

namespace Backend.Models;

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
namespace Backend.Models;

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

namespace Backend.Models;

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
namespace Backend.Models.DTO
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
using System.ComponentModel.DataAnnotations; // <-- Добавьте этот using

namespace Backend.Models.DTO
{
    public class RegisterModel
    {
        public required string FirstName { get; set; }
        public required string SecondName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Необходимо указать пол.")]
        public string Gender { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
```

---

### 📄 `Models/DTO/UserDTO.cs`

```csharp
namespace Backend.Models.DTO
{
    public class UserDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateTime? RegistrationTime { get; set; }
        public int? IdUserStatus { get; set; }
    }
}
```

---

### 📄 `Models/DTO/UserRole.cs`

```csharp
public class UserRole
{
    public string Id { get; set; }
    public IList<string> Roles { get; set; }
}
```

---

### 📄 `Models/DTO/UserWithRolesDto.cs`

```csharp
public class UserWithRolesDto
{
    public string Id { get; set; }
    public string? ShortName { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    public DateTime? RegistrationTime { get; set; }
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? LastName { get; set; }
    public IList<string> Roles { get; set; }
}

public class UserRoleDto
{
    public string UserId { get; set; }
    public List<string> Roles { get; set; }
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
using Backend.Models;
using Backend.Models.DTO;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                LastName = model.LastName,
                Gender = model.Gender,
                RegistrationTime = DateTime.UtcNow,
                ShortName = $"{model.SecondName} {model.FirstName[0]}.{(string.IsNullOrEmpty(model.LastName) ? "" : model.LastName[0] + ".")}"
            };
            var creationResult = await _userManager.CreateAsync(user, model.Password);
            Console.WriteLine(model.Password);
            if (!creationResult.Succeeded)
            {
                return creationResult;
            }
            IdentityResult addToRoleResult;
            if (model.Email == "admin@admin.ru" && model.Password == "Admin123!")
            {
                addToRoleResult = await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
            }

            return addToRoleResult; 
        }

        public async Task<string> LoginUserAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                return await GenerateJwtToken(user);
            }
            return null;
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(120),
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
using Backend.Models.DTO;

namespace Backend.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterModel model);

        Task<string> LoginUserAsync(string email, string password);
    }
}
```

---

### 📄 `utils/asprename.md`

```markdown
Отличный вопрос! Переименование таблиц, созданных ASP.NET Core Identity (таких как `AspNetUsers`), — это стандартная задача для кастомизации. Просто так переименовать таблицу в базе данных нельзя, потому что Entity Framework и Identity "ожидают" найти таблицы с определёнными именами.

Правильный способ — указать Entity Framework Core, какие имена вы хотите использовать. Это делается в вашем `DbContext`.

Вот пошаговая инструкция, как безопасно переименовать `AspNetUsers` в `Users` и другие связанные таблицы.

### Почему это требует особого подхода?

ASP.NET Core Identity по умолчанию создаёт целый набор взаимосвязанных таблиц:
*   `AspNetUsers` - Пользователи
*   `AspNetRoles` - Роли
*   `AspNetUserRoles` - Связь пользователей и ролей
*   `AspNetUserClaims` - "Утверждения" или данные о пользователе
*   `AspNetUserLogins` - Для входа через внешние сервисы (Google, Facebook)
*   `AspNetUserTokens` - Токены (например, для сброса пароля)
*   `AspNetRoleClaims` - "Утверждения" для ролей

Все эти таблицы связаны между собой через внешние ключи (foreign keys). Поэтому просто переименовать `AspNetUsers` недостаточно — нужно переименовать всю группу таблиц, чтобы сохранить консистентность.

---

### Инструкция по переименованию таблиц Identity

#### Шаг 1: Настройка DbContext

Откройте ваш файл контекста базы данных: `Backend/Models/TodoListDbContext.cs`. Вам нужно переопределить метод `OnModelCreating` и явно указать новые имена для каждой таблицы Identity.

Добавьте следующий код в класс `TodoListDbContext`:

```csharp
// Backend/Models/TodoListDbContext.cs

// ... using statements
using Microsoft.AspNetCore.Identity; // Убедитесь, что этот using есть

public partial class TodoListDbContext : IdentityDbContext<ApplicationUser>
{
    // ... ваш конструктор и DbSet'ы ...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ВАЖНО: Сначала вызываем базовую реализацию.
        // Это необходимо для того, чтобы Identity настроил свою схему.
        base.OnModelCreating(modelBuilder);

        // А теперь переопределяем имена таблиц
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable(name: "Users"); // Переименовываем AspNetUsers в Users
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Roles"); // AspNetRoles -> Roles
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles"); // AspNetUserRoles -> UserRoles
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims"); // AspNetUserClaims -> UserClaims
        });

        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins"); // AspNetUserLogins -> UserLogins
        });

        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims"); // AspNetRoleClaims -> RoleClaims
        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens"); // AspNetUserTokens -> UserTokens
        });
        
        // Здесь продолжается ваша существующая логика для других таблиц
        modelBuilder.Entity<Project>(entity =>
        {
            // ...
        });

        // ... и так далее для всех ваших моделей
    }

    // ...
}
```

#### Шаг 2: Создание новой миграции

Теперь, когда вы указали EF Core новые имена таблиц, нужно создать миграцию, которая применит эти изменения к базе данных.

1.  Откройте терминал в папке `Backend`.
2.  Выполните команду для создания миграции. Дайте ей понятное имя.

    ```bash
    dotnet ef migrations add CustomizeIdentityTableNames
    ```

3.  **Проверьте сгенерированный файл миграции.** Откройте новый файл в папке `Backend/Migrations`. Его метод `Up` должен содержать команды `RenameTable`, которые переименовывают таблицы, сохраняя все данные. Он будет выглядеть примерно так:

    ```csharp
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameTable(
            name: "AspNetUserTokens",
            newName: "UserTokens");

        migrationBuilder.RenameTable(
            name: "AspNetUsers",
            newName: "Users");
        
        // ... и так далее для всех остальных таблиц Identity
    }
    ```
    Это подтверждает, что EF Core правильно понял ваше намерение и не будет удалять и создавать таблицы заново.

#### Шаг 3: Применение миграции

Последний шаг — применить миграцию к вашей базе данных.

1.  Убедитесь, что Docker-контейнер с БД запущен.
2.  В терминале (в папке `Backend`) выполните:

    ```bash
    dotnet ef database update
    ```

Готово! После выполнения команды все таблицы Identity будут переименованы в вашей базе данных без потери данных.

### Очень важные моменты:

*   **Сделайте бэкап!** Перед выполнением таких серьёзных изменений в структуре рабочей или важной базы данных всегда делайте резервную копию.
*   **Когда лучше это делать?** Идеальный момент для переименования таблиц — самое начало проекта, до того, как в базе накопилось много данных. Если вы делаете это на существующем проекте, делайте это осторожно и тщательно протестируйте.
*   **Raw SQL запросы.** Если где-то в вашем коде есть "сырые" SQL-запросы (например, через `_context.Database.ExecuteSqlRawAsync()`), которые обращаются к старым именам таблиц (`AspNetUsers`), их нужно будет найти и обновить вручную.
```

---

### 📄 `utils/changemodel.md`

```markdown
Конечно! Вот инструкция, как безболезненно работать с миграциями в вашем проекте, включая добавление новых полей и переименование существующих.

### Обзор процесса работы с миграциями в Entity Framework Core

В вашем проекте используется Entity Framework Core, который применяет подход "Code-First". Это означает, что структура вашей базы данных определяется C# классами в папке `Backend/Models`. Любые изменения в этих моделях (добавление, удаление или изменение полей) должны сопровождаться созданием и применением "миграции" — специального файла, который описывает, как обновить схему базы данных, чтобы она соответствовала вашим моделям.

---

### Инструкция: Как добавить новое поле в модель

Давайте в качестве примера добавим поле `ShortName` (сокращённое имя) для пользователя, как у вас указано в `TODO.md`.

#### Шаг 1: Изменение модели

Откройте файл модели, который вы хотите изменить. В нашем случае это `Backend/Models/ApplicationUser.cs`. Добавьте в класс новое свойство:

```csharp
// Backend/Models/ApplicationUser.cs

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? PatronymicName { get; set; }
        
        // 👇 Новое добавленное поле
        public string? ShortName { get; set; } 

        public DateTime? RegistrationTime { get; set; }
        public string? Notes { get; set; }
        public int? IdUserStatus { get; set; }
        public virtual Status? IdUserStatusNavigation { get; set; }
        public virtual ICollection<TasksUser> TasksUsers { get; set; } = new List<TasksUser>();
        public virtual ICollection<UsersCommand> UsersCommands { get; set; } = new List<UsersCommand>();
    }
}
```

#### Шаг 2: Создание миграции

Теперь нужно создать файл миграции, который добавит соответствующую колонку в таблицу `AspNetUsers`.

1.  **Откройте терминал** в корневой папке вашего проекта.
2.  Перейдите в директорию бэкенда:
    ```bash
    cd Backend
    ```
3.  Выполните команду для создания миграции. Дайте ей осмысленное имя, например, `AddShortNameToUser`.

    ```bash
    dotnet ef migrations add AddShortNameToUser
    ```

После выполнения этой команды в папке `Backend/Migrations` появится новый файл с именем вроде `20251008xxxxxx_AddShortNameToUser.cs`. Его содержимое будет выглядеть примерно так:

```csharp
// ...
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<string>(
        name: "ShortName",
        table: "AspNetUsers",
        type: "nvarchar(max)",
        nullable: true);
}
// ...
```
Этот код говорит Entity Framework добавить колонку `ShortName` в таблицу `AspNetUsers`.

#### Шаг 3: Применение миграции к базе данных

Последний шаг — применить изменения к базе данных.

1.  Убедитесь, что ваш Docker-контейнер с базой данных запущен. Вы можете запустить его командой из корневой директории:
    ```bash
    docker-compose up -d db
    ```2.  В терминале, находясь в папке `Backend`, выполните команду:
    ```bash
    dotnet ef database update
    ```

Готово! Ваша база данных обновлена, и в таблице `AspNetUsers` теперь есть колонка `ShortName`.

---

### Инструкция: Как переименовать существующее поле

Переименование — более деликатный процесс, так как по умолчанию EF может удалить старую колонку (вместе со всеми данными) и создать новую. Чтобы избежать потери данных, нужно немного отредактировать файл миграции.

Давайте для примера переименуем поле `PatronymicName` в `LastName` в той же модели `ApplicationUser`.

#### Шаг 1: Изменение модели

Переименуйте свойство в файле `Backend/Models/ApplicationUser.cs`.

```csharp
// Backend/Models/ApplicationUser.cs
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    
    // 👇 Поле переименовано с PatronymicName на LastName
    public string? LastName { get; set; } 

    public string? ShortName { get; set; } 
    // ... остальной код
}
```

#### Шаг 2: Создание миграции

Как и в прошлый раз, создайте миграцию из папки `Backend`.

```bash
dotnet ef migrations add RenamePatronymicToLastNameOnUser
```

#### Шаг 3: **(Важно!)** Редактирование файла миграции

Откройте сгенерированный файл миграции (`..._RenamePatronymicToLastNameOnUser.cs`). Вы увидите, что EF сгенерировал код для удаления старой колонки и добавления новой:

```csharp
// Неправильный вариант, который приведёт к потере данных!
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropColumn(
        name: "PatronymicName",
        table: "AspNetUsers");

    migrationBuilder.AddColumn<string>(
        name: "LastName",
        table: "AspNetUsers",
        type: "nvarchar(max)",
        nullable: true);
}
```

Это приведёт к потере всех данных в этой колонке. **Замените** содержимое метода `Up` на специальную команду `RenameColumn`:

```csharp
// Правильный вариант для переименования
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.RenameColumn(
        name: "PatronymicName",
        table: "AspNetUsers",
        newName: "LastName");
}
```
Таким же образом нужно исправить метод `Down`, который отвечает за откат миграции:
```csharp
// Правильный откат
protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.RenameColumn(
        name: "LastName",
        table: "AspNetUsers",
        newName: "PatronymicName");
}
```
Этот подход гарантирует, что колонка будет просто переименована в базе данных без потери информации.

#### Шаг 4: Применение миграции

Теперь, когда миграция исправлена, примените её к базе данных:

```bash
dotnet ef database update
```

#### Шаг 5: Обновление кода

Не забудьте найти все места в вашем коде (например, в контроллерах, сервисах, DTO), где использовалось старое имя `PatronymicName`, и заменить его на `LastName`.

### Общие рекомендации

*   **Одна миграция — одно логическое изменение.** Старайтесь не смешивать в одной миграции добавление полей, их переименование и удаление. Это упростит отладку, если что-то пойдет не так.
*   **Всегда проверяйте сгенерированные миграции.** Особенно при переименовании или изменении типа данных. Автоматически сгенерированный код не всегда идеален.
*   **Работа в команде.** Перед тем как применять миграции, убедитесь, что все члены команды получили последние изменения из системы контроля версий (Git), чтобы избежать конфликтов.
```

---

### 📄 `utils/clean-run.sh`

```bash
# Флаг -v удаляет анонимные и именованные тома, привязанные к контейнерам
docker-compose down -v

# Удаляет все неиспользуемые контейнеры, сети, образы (включая "висячие")
docker system prune -a -f

docker-compose build --no-cache

docker-compose up -d

docker-compose ps

docker-compose logs -f server
```

---

### 📄 `utils/gen-controllers.sh`

```bash
cd Backend

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

---

### 📄 `utils/roles.md`

```markdown
1.  **Гость (Guest)** — Просмотр отдельных проектов/задач без возможности что-либо редактировать. Полезно для приглашения клиента или сотрудника из другого отдела.

2.  **Пользователь (User) / Исполнитель (Contributor)** — Базовая роль. Может:
    *   Просматривать назначенные ему проекты и задачи.
    *   Менять статус своих задач.
    *   Оставлять комментарии/заметки.

3.  **Модератор (Moderator) / Редактор (Editor)** — Часто нужна на уровне проекта, а не всей системы.
    *   Может создавать и редактировать задачи в рамках своего проекта.
    *   Может назначать исполнителей (Users) на задачи.
    *   Может управлять тегами, статусами задач (в рамках проекта).

4.  **Тимлид (Team Lead) / Менеджер проекта (Project Manager)** — Расширенные права в рамках команды/проекта.
    *   Всё, что может Модератор.
    *   Создавать и архивировать проекты в своей команде.
    *   Управлять составом команды (добавлять/удалять участников с ролями до Модератора).
    *   Просматривать аналитику по своему проекту (сроки, нагрузка).

5.  **Администратор (Administrator)** — Полный контроль над всей системой.
    *   Управление пользователями и их глобальными ролями.
    *   Создание и управление всеми командами и проектами.
    *   Доступ к системным настройкам и полной аналитике.

6.  **Владелец системы (Owner) / Суперадмин (Super Administrator)** — Техническая роль. Отличается от Админа тем, что не может быть удалена или лишена прав. Обычно это вы или главный техник.

---

### Рекомендация по структуре на основе ваших моделей

Глядя на ваши модели, особенно на `UsersCommand` (связь пользователей и команд) и `Project` (который привязан к команде), я вижу логичную структуру:

*   **Глобальные роли (уровень системы):** `Гость`, `Пользователь`, `Администратор`, `Владелец`.
*   **Локальные роли (уровень команды/проекта):** `Исполнитель`, `Модератор`, `Тимлид`.

**Как это связать:**
Пользователь с глобальной ролью `Пользователь` может входить в несколько команд, и в каждой из них ему может быть выдана своя локальная роль. Это реализуется не через систему ролей Identity, а через вашу собственную таблицу, например, `UserTeamRoles`:

| UserId | TeamId | ProjectId | RoleInTeam     |
| :----- | :----- | :-------- | :------------- |
| user1  | teamA  | NULL      | TeamLead       |
| user1  | teamB  | NULL      | Contributor    |
| user2  | teamA  | NULL      | Moderator      |
| user1  | NULL   | projectX  | ProjectManager |

Такой подход максимально гибок, но и сложнее в реализации. Для начала можно обойтись системой глобальных ролей от Identity, а к локальным ролям вернуться, когда потребуется.

### Итог

1.  **Поддержка нескольких ролей — это правильный путь.** Начинайте с нее сразу.
2.  **Расширьте список ролей**, как минимум, до: `Guest`, `User`, `Moderator`, `TeamLead`, `Administrator`.
3.  **Подумайте о двухуровневой системе** (глобальные и локальные роли) для будущей масштабируемости, но начните с простой реализации на глобальных ролях ASP.NET Identity.

Это даст вам мощную и гибкую систему разграничения прав, которая будет расти вместе с вашим приложением.
```
