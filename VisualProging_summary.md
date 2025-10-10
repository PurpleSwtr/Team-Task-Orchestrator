
---

### 📄 `.dockerignore`

```gitignore
# Include any files or directories that you don't want to be copied to your
# container here (e.g., local build artifacts, temporary files, etc.).
#
# For more help, visit the .dockerignore file reference guide at
# https://docs.docker.com/go/build-context-dockerignore/

**/.DS_Store
**/.classpath
**/.dockerignore
**/.env
**/.git
**/.gitignore
**/.project
**/.settings
**/.toolstarget
**/.vs
**/.vscode
**/*.*proj.user
**/*.dbmdl
**/*.jfm
**/bin
**/charts
**/docker-compose*
**/compose.y*ml
**/Dockerfile*
**/node_modules
**/npm-debug.log
**/obj
**/secrets.dev.yaml
**/values.dev.yaml
LICENSE
README.md
```

---

### 📄 `.env.example`

```ini
DB_SA_PASSWORD = password
JWT_KEY = key
```

---

### 📄 `.gitignore`

```gitignore
# ===================================================================
# Fichiers secrets et de configuration locale
# NE JAMAIS METTRE DE MOTS DE PASSE OU DE CLÉS D'API DANS GIT !
# ===================================================================
*.env.local
.env
# ===================================================================
# Dépendances et artefacts de construction
# Les dépendances doivent être installées, pas stockées dans Git.
# Les artefacts sont générés à partir du code source.
# ===================================================================

Backend/bin/
Backend/obj/
Backend/Generators/Files
Backend/appsettings.Development.json

Frontend/node_modules/
Frontend/dist/
Frontend/dist-ssr/
Frontend/coverage/
Frontend/.DS_Store
Frontend/npm-debug.log*
Frontend/yarn-debug.log*
Frontend/yarn-error.log*
Frontend/pnpm-debug.log*
Frontend/lerna-debug.log*
Frontend/*.tsbuildinfo

# ===================================================================
# Fichiers générés par l'IDE et l'éditeur
# Ces fichiers sont spécifiques à l'environnement de développement local de chaque utilisateur.
# ===================================================================

# Visual Studio / Rider
.vs/
*.user
*.suo
*.sln.docstates

# Visual Studio Code
.vscode/*
# Ne pas ignorer les paramètres et les extensions recommandés pour le projet
!.vscode/settings.json
!.vscode/tasks.json
!.vscode/launch.json
!.vscode/extensions.json
*.code-workspace

# JetBrains (Rider, WebStorm, etc.)
.idea/

# ===================================================================
# Fichiers du système d'exploitation
# Fichiers inutiles générés par macOS, Windows et Linux.
# ===================================================================
.DS_Store
Thumbs.db
ehthumbs.db
Desktop.ini
$RECYCLE.BIN/

# ===================================================================
# Fichiers de log
# ===================================================================
*.log
logs/
```

---

### 📄 `README.md`

```markdown
# ToDoList Курсовая работа

Когда-нибудь тут нужно написать крутое ридми...

В описании можно указать чёто типо почему я выбрал именно эти технологии, что конкретно применялось. Ещё вот эти красивые гитхабовские иконки и всякое такое.

Потом обязательно заскриншотим или вставим гифки (если можно) Корочееее, дел много...
```

---

### 📄 `TODO.md`

```markdown
ИТАК ПЛАН, КАК ВСЁ ДОЛЖНО РАБОТАТЬ!!!

1. Учимся создавать докер контейнер
2. Сначала запускаем базу
3. Потом сервер бекенда
4. Пытаемся подружить бек и фронт (настраиваем middleware)
5. Не забываем про миграции

Главная страничка с кнопкой под регистрацию / вход в хедере

Доступ к менюшкам на основе роли

Генератор: админ
Поставить задачу: модератор
Просмотреть задачи: пользователи

Совмещённая страничка logreg

Скрывающийся sidebar

Основная встречающая страничка о проекте и кнопками регистрация вход (отдельный layout)


Регистрация новые поля
Обновить ДТО регистрации

Адекватный перенос на странички

Динамическая страничка с ошибкой

Панелька с пользователями

Добавить лидов и прочие статусы

Админ панелька

Сделать поиск

Добавить в бдшку поле с сокращённым именем

Объекты использующие ref переписать на reactive

Подключиться к базе данных в докере через mssql
```

---

### 📄 `admin-acc.txt`

```text
admin@admin.ru
Admin123!
```

---

### 📄 `compose.yaml`

```yaml
services:
  server:
    build:
      context: ./Backend
      target: final
    ports:
      - "8080:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Server=db;Database=TodoListDB;User Id=sa;Password=${DB_SA_PASSWORD};TrustServerCertificate=True;
      - Jwt__Key=${JWT_KEY}
      - ASPNETCORE_ENVIRONMENT=Development
    depends_on:
      db:
        condition: service_healthy

  db:
    build:
      context: ./Database
      dockerfile: Dockerfile
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=${DB_SA_PASSWORD}
    ports:
      - "1433:1433"
    volumes:
      - db-data:/var/opt/mssql
    healthcheck:
      test: ["CMD-SHELL", "/opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U sa -P '${DB_SA_PASSWORD}' -Q 'SELECT 1' || exit 1"]
      interval: 10s
      timeout: 5s
      retries: 10
      start_period: 10s

volumes:
  db-data:
```

---

### 📄 `test-account.txt`

```text
ivanov.sergey@mail.ru | Иванов | Сергей | Владимирович | RedStarIvanov92!
petrova.anna@yandex.ru | Петрова | Анна | Сергеевна | AnnPetrova_FLOWER
sidorov.alex@inbox.ru | Сидоров | Алексей | Дмитриевич | SidAlex1985
kuznetsova.maria@gmail.com | Кузнецова | Мария | Олеговна | MashaKuz_2023
fedorov.dmitriy@list.ru | Фёдоров | Дмитрий | Игоревич | FedDmitry007
smirnova.ekaterina@rambler.ru | Смирнова | Екатерина | Викторовна | KatySmirn_!Sun
popov.mikhail@hotmail.com | Попов | Михаил | Александрович | MegaMisha_pop
volkova.olga@bk.ru | Волкова | Ольга | Павловна | OliaVolk_Spring
lebedeva.irina@yahoo.com | Лебедева | Ирина | Анатольевна | IrinaLebed88!
kozlov.vladimir@sibmail.com | Козлов | Владимир | Николаевич | VovaKozlov_ZZ
novikova.tatiana@proton.me | Новикова | Татьяна | Борисовна | TanyaNovik_*
morozov.andrey@internet.ru | Морозов | Андрей | Валерьевич | FrostyAndy_91
pavlova.elena@outlook.com | Павлова | Елена | Станиславовна | LenaPavlova_Moon
sokolov.artem@mail.com | Соколов | Артём | Геннадьевич | SokolArt_Turbo
orlova.natalia@yandex.com | Орлова | Наталья | Романовна | NataliOrl_Star
vorobiev.igor@gmail.com | Воробьёв | Игорь | Петрович | IgVorobey_2024
guseva.svetlana@list.com | Гусева | Светлана | Фёдоровна | SvetGus_Alba
tarasov.pavel@inbox.com | Тарасов | Павел | Олегович | PavelTar_#1
belyakova.daria@rambler.com | Белякова | Дарья | Кирилловна | DashaBely_White
medvedev.konstantin@bk.com | Медведев | Константин | Юрьевич | MedvedKostya_777
```

---

### 📄 `Backend/Backend.csproj`

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

### 📄 `Backend/Backend.sln`

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

### 📄 `Backend/Dockerfile`

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

### 📄 `Backend/Program.cs`

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

### 📄 `Backend/appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TodoListDB;User Id=sa;Password=SuperSecretPassword12345;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

### 📄 `Backend/appsettings.json`

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

### 📄 `Backend/Авторизация.md`

```markdown
Авторизация. И на сервере и на клиенте.

Разные права у пользователей.

Всё в отдельной базе.
```

---

### 📄 `Backend/.config/dotnet-tools.json`

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

### 📄 `Backend/Controllers/AuthController.cs`

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

### 📄 `Backend/Controllers/ProjectsController.cs`

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

### 📄 `Backend/Controllers/StatussController.cs`

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

### 📄 `Backend/Controllers/TasksController.cs`

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

### 📄 `Backend/Controllers/UsersController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
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
       
        // добавить логику создания (POST), обновления (PUT) и удаления (DELETE)

    }
}
```

---

### 📄 `Backend/Data/DbInitializer.cs`

```csharp
using Microsoft.AspNetCore.Identity;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "User", "Moderator" };
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

### 📄 `Backend/Generators/EmailGenerator.cs`

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

### 📄 `Backend/Generators/GeneratorController.cs`

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

### 📄 `Backend/Generators/ProjectsGenerator.cs`

```csharp

```

---

### 📄 `Backend/Generators/TaskGenerator.cs`

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

### 📄 `Backend/Generators/TeamsGenerator.cs`

```csharp

```

---

### 📄 `Backend/Generators/UserGenerator.cs`

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
                    RegistrationTime = DateTime.UtcNow,
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
            }
        }

    }
}
```

---

### 📄 `Backend/Migrations/20250929211742_InitialCreate.Designer.cs`

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

### 📄 `Backend/Migrations/20250929211742_InitialCreate.cs`

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

### 📄 `Backend/Migrations/20251008100058_AddShortNameToUser.Designer.cs`

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

### 📄 `Backend/Migrations/20251008100058_AddShortNameToUser.cs`

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

### 📄 `Backend/Migrations/20251008191012_AddGenderToUser.Designer.cs`

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

### 📄 `Backend/Migrations/20251008191012_AddGenderToUser.cs`

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

### 📄 `Backend/Migrations/20251008204415_AddLastNameRenameToUser.Designer.cs`

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

### 📄 `Backend/Migrations/20251008204415_AddLastNameRenameToUser.cs`

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

### 📄 `Backend/Migrations/TodoListDbContextModelSnapshot.cs`

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

### 📄 `Backend/Models/ApplicationUser.cs`

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

### 📄 `Backend/Models/Project.cs`

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

### 📄 `Backend/Models/Status.cs`

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

### 📄 `Backend/Models/Task.cs`

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

### 📄 `Backend/Models/TasksProject.cs`

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

### 📄 `Backend/Models/TasksUser.cs`

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

### 📄 `Backend/Models/Team.cs`

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

### 📄 `Backend/Models/TodoListDbContext.cs`

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

### 📄 `Backend/Models/UsersCommand.cs`

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

### 📄 `Backend/Models/DTO/LoginModel.cs`

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

### 📄 `Backend/Models/DTO/RegiserModel.cs`

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

### 📄 `Backend/Models/DTO/UserDTO.cs`

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

### 📄 `Backend/Models/DTO/UserWithRolesDto.cs`

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
```

---

### 📄 `Backend/Properties/launchSettings.json`

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

### 📄 `Backend/Services/AuthService.cs`

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Models.DTO;

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

            if (!creationResult.Succeeded)
            {
                return creationResult;
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, "Admin");

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

### 📄 `Backend/Services/IAuthService.cs`

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

### 📄 `Backend/utils/asprename.md`

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

### 📄 `Backend/utils/changemodel.md`

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

### 📄 `Backend/utils/clean-run.sh`

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

### 📄 `Backend/utils/gen-controllers.sh`

```bash
cd Backend

models=("Project" "User" "Command" "Status" "Task")

for model in "${models[@]}"; do
    
    controllerName="${model}sController"
    
    dotnet tool run dotnet-aspnet-codegenerator controller -name $controllerName -api -m $model -dc TodoListDbContext -outDir Controllers
done
```

---

### 📄 `Backend/utils/migrate.sh`

```bash
dotnet ef dbcontext scaffold "Server=localhost\MSSQLSERVER01;Database=TodoListDB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --no-onconfiguring
```

---

### 📄 `Backend/utils/roles.md`

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

---

### 📄 `Database/Dockerfile`

```dockerfile
FROM mcr.microsoft.com/mssql/server:2022-latest

USER root


RUN apt-get update && apt-get install -y curl gnupg apt-transport-https


RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list > /etc/apt/sources.list.d/mssql-release.list


RUN apt-get update \
    && ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev \
    && rm -rf /var/lib/apt/lists/*


USER mssql
```

---

### 📄 `Frontend/.prettierrc.json`

```json
{
  "$schema": "https://json.schemastore.org/prettierrc",
  "semi": false,
  "singleQuote": true,
  "printWidth": 100
}
```

---

### 📄 `Frontend/README.md`

```markdown
# Frontend

This template should help get you started developing with Vue 3 in Vite.

## Recommended IDE Setup

[VSCode](https://code.visualstudio.com/) + [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

## Customize configuration

See [Vite Configuration Reference](https://vite.dev/config/).

## Project Setup

```sh
npm install
```

### Compile and Hot-Reload for Development

```sh
npm run dev
```

### Type-Check, Compile and Minify for Production

```sh
npm run build
```

### Lint with [ESLint](https://eslint.org/)

```sh
npm run lint
```
```

---

### 📄 `Frontend/env.d.ts`

```typescript
/// <reference types="vite/client" />

// 👇 Вот эту часть нужно добавить
declare module '*.vue' {
  import type { DefineComponent } from 'vue'
  const component: DefineComponent<{}, {}, any>
  export default component
}
```

---

### 📄 `Frontend/eslint.config.ts`

```typescript
import { globalIgnores } from 'eslint/config'
import { defineConfigWithVueTs, vueTsConfigs } from '@vue/eslint-config-typescript'
import pluginVue from 'eslint-plugin-vue'
import skipFormatting from '@vue/eslint-config-prettier/skip-formatting'

// To allow more languages other than `ts` in `.vue` files, uncomment the following lines:
// import { configureVueProject } from '@vue/eslint-config-typescript'
// configureVueProject({ scriptLangs: ['ts', 'tsx'] })
// More info at https://github.com/vuejs/eslint-config-typescript/#advanced-setup

export default defineConfigWithVueTs(
  {
    name: 'app/files-to-lint',
    files: ['**/*.{ts,mts,tsx,vue}'],
  },

  globalIgnores(['**/dist/**', '**/dist-ssr/**', '**/coverage/**']),

  pluginVue.configs['flat/essential'],
  vueTsConfigs.recommended,
  skipFormatting,
)
```

---

### 📄 `Frontend/index.html`

```html
<!DOCTYPE html>
<html lang="">
  <head>
    <meta charset="UTF-8">
    <link rel="icon" href="/favicon.ico">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>ToDoApp</title>
  </head>
  <body>
    <div id="app"></div>
    <script type="module" src="/src/main.ts"></script>
  </body>
</html>
```

---

### 📄 `Frontend/package.json`

```json
{
  "name": "frontend",
  "version": "0.0.0",
  "private": true,
  "type": "module",
  "engines": {
    "node": "^20.19.0 || >=22.12.0"
  },
  "scripts": {
    "dev": "vite",
    "build": "run-p type-check \"build-only {@}\" --",
    "preview": "vite preview",
    "build-only": "vite build",
    "type-check": "vue-tsc --build",
    "lint": "eslint . --fix",
    "format": "prettier --write src/"
  },
  "dependencies": {
    "axios": "^1.12.2",
    "pinia": "^3.0.3",
    "svg-loader": "^0.0.2",
    "vue": "^3.5.18",
    "vue-router": "^4.5.1"
  },
  "devDependencies": {
    "@tailwindcss/vite": "^4.1.13",
    "@tsconfig/node22": "^22.0.2",
    "@types/node": "^22.16.5",
    "@vitejs/plugin-vue": "^6.0.1",
    "@vue/eslint-config-prettier": "^10.2.0",
    "@vue/eslint-config-typescript": "^14.6.0",
    "@vue/tsconfig": "^0.7.0",
    "eslint": "^9.31.0",
    "eslint-plugin-vue": "~10.3.0",
    "jiti": "^2.4.2",
    "npm-run-all2": "^8.0.4",
    "prettier": "3.6.2",
    "tailwindcss": "^4.1.13",
    "typescript": "~5.8.0",
    "vite": "^7.0.6",
    "vite-plugin-vue-devtools": "^8.0.0",
    "vite-svg-loader": "^5.1.0",
    "vue-tsc": "^3.0.4"
  }
}
```

---

### 📄 `Frontend/tsconfig.app.json`

```json
{
  "extends": "@vue/tsconfig/tsconfig.dom.json",
  "include": ["env.d.ts", "src/**/*", "src/**/*.vue"],
  "exclude": ["src/**/__tests__/*"],
  "compilerOptions": {
    "tsBuildInfoFile": "./node_modules/.tmp/tsconfig.app.tsbuildinfo",

    "paths": {
      "@/*": ["./src/*"]
    }
  }
}
```

---

### 📄 `Frontend/tsconfig.json`

```json
{
  "files": [],
  "references": [
    {
      "path": "./tsconfig.node.json"
    },
    {
      "path": "./tsconfig.app.json"
    }
  ],
  "compilerOptions": {
    "allowJs": true
  }
}
```

---

### 📄 `Frontend/tsconfig.node.json`

```json
{
  "extends": "@tsconfig/node22/tsconfig.json",
  "include": [
    "vite.config.*",
    "vitest.config.*",
    "cypress.config.*",
    "nightwatch.conf.*",
    "playwright.config.*",
    "eslint.config.*"
  ],
  "compilerOptions": {
    "noEmit": true,
    "tsBuildInfoFile": "./node_modules/.tmp/tsconfig.node.tsbuildinfo",

    "module": "ESNext",
    "moduleResolution": "Bundler",
    "types": ["node"]
  }
}
```

---

### 📄 `Frontend/vite.config.ts`

```typescript
import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'
import tailwindcss from '@tailwindcss/vite'
import svgLoader from 'vite-svg-loader'
// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(), 
    vueDevTools(), 
    tailwindcss(),
    svgLoader(),
    
    ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
})
```

---

### 📄 `Frontend/src/App.vue`

```vue
<script setup lang="ts">
import { RouterView, useRoute, useRouter, type RouteLocationNormalizedLoadedGeneric } from 'vue-router'
import { ref, onMounted, provide, readonly } from 'vue' 
import apiClient from './api'; 
import LoadingCircle from './components/ui/LoadingCircle.vue';
import E_403 from './layouts/E_403.vue';
import E_404 from './layouts/E_404.vue';

const isLoggedIn = ref(false)
const isAuthLoading = ref(true) 

const accessList = ['/about', '/login_register', '/']

const checkAuthStatus = async () => {
  isAuthLoading.value = true;
  try {
    await apiClient.get('/Auth/me'); 
    isLoggedIn.value = true;
  } catch (error) {
    isLoggedIn.value = false; 
  } finally {
    isAuthLoading.value = false;
  }
}

onMounted(() => {
  checkAuthStatus()
})

const setLoggedIn = () => {
  isLoggedIn.value = true;
}

const setLoggedOut = () => {
  isLoggedIn.value = false;
}

provide('auth', {
  isLoggedIn: readonly(isLoggedIn), 
  setLoggedIn,
  setLoggedOut,
  checkAuthStatus 
})

const existsCheck = () => {
  const path = useRoute()
  const router = useRouter()
  const routes = router.getRoutes()
  const routeExists = routes.some(route => 
    route.path === path.path)
  return !routeExists
};

const accessCheck = () => {
  const router = useRoute()

  if (isLoggedIn.value)
  {
    return false
  }
  else if (accessList.includes(router.path)) {
    return false
  }
  else
  {
    return true
  }
};

</script>

<template>
  <div v-if="isAuthLoading" class="loading-screen">
    <LoadingCircle/>
  </div>
  <body class="app">
    <E_404 v-if="existsCheck()"></E_404>
    <E_403 v-else-if="accessCheck()"></E_403>
    <RouterView v-else/>
  </body>
</template>

<style scoped>
.app {
  background-color: #e8e8e8;
}

.loading-screen {
  background-color: #e8e8e8;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  font-size: 4.5rem;
  color: #333;
}
</style>
```

---

### 📄 `Frontend/src/main.ts`

```typescript
import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')
```

---

### 📄 `Frontend/src/api/index.ts`

```typescript
import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:8080/api',
  withCredentials: true
});

apiClient.interceptors.request.use(request => {
  const { method, url, data } = request;
  // console.log(`${method?.toUpperCase()} ${url}`, data ? { data } : '');
  return request;
});

apiClient.interceptors.response.use(
  (response) => {
    const { status, config: { method, url }, data } = response;
    console.log(`${status} ${method?.toUpperCase()} ${url}`, { data });
    return response;
  },
  (error) => {
    if (error.response) {
      const { status, config: { method, url }, data } = error.response;
      console.error(`${status} ${method?.toUpperCase()} ${url}`, { data });
    } else {
      console.error('Ошибка:', error.message);
    }
    return Promise.reject(error);
  }
);

export default apiClient;
```

---

### 📄 `Frontend/src/assets/base.css`

```css

```

---

### 📄 `Frontend/src/assets/main.css`

```css
/* 
@import url('https://fonts.googleapis.com/css2?family=Montserrat:wght@400;500;600;700&display=swap');

@theme {
  fontFamily: {
    sans: ['Montserrat', ...theme('fontFamily.sans')],
  }
} */

@import './base.css';
@import 'tailwindcss';
```

---

### 📄 `Frontend/src/components/CardTask.vue`

```vue
<template>
  <div class="p-4 pl-6 ml-5 mt-10 bg-gray-100 text-black relative overflow-hidden duration-500 rounded-2xl w-70 max-w-full hover:-translate-y-2 shadow-gray-300 hover:shadow-xl shadow-md hover:cursor-pointer hover:scale-103">
    <!-- <TaskIcon class="w-5 h-5 inline mr-1 mb-1"></TaskIcon> -->
    <AppIcon icon_name="task" 
    class="absolute right-5 bottom-7 scale-500 text-gray-200 z-0"></AppIcon>
    <h1 class="text-lg font-semibold truncate inline text-gray-700 relative z-10">{{ props.tittle }}</h1>
    <p class="font-normal line-clamp-3 text-gray-700 relative z-10">{{ props.task }}</p>
    <div class="flex items-center gap-2 mt-2 relative z-10">
      <input type="checkbox" />
      <span class="text-sm text-gray-700">Выполнено</span>
    </div>
  </div>
</template>

<script setup>
import AppIcon from './ui/AppIcon.vue';

const props = defineProps({
  tittle: String,
  task: String,
})
</script>

<style></style>
```

---

### 📄 `Frontend/src/components/features/LoginForm.vue`

```vue
<template>
    <div class="flex flex-col items-center w-full max-w-md bg-gray-100 pt-30 rounded-3xl shadow-2xl shadow-gray-300">
      <h1 class="text-3xl font-bold mb-6 text-gray-700">Вход в аккаунт</h1>
      <div class="flex flex-col gap-4 w-full">
        <input 
          type="email" 
          placeholder="Email" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="email"
        >
        <input 
          type="password" 
          placeholder="Пароль" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="password"
        >
        <Transition name="fade" mode="out-in">
          <p v-if="errorMessage" class="text-red-500 self-center" key="error-message">
            {{ errorMessage }}
          </p>
        </Transition>
        <AppButton @click="tryLogin" :statusLoading="buttonLoading" message="Войти" class="self-center mb-10"/>
        <div class="text-center">
            <p class="inline">У вас нет аккаунта? </p>
            <button 
                @click="$emit('switchToRegister')"
                class="inline text-blue-500 cursor-pointer underline-offset-2 hover:underline hover:text-blue-700"
            >
                Зарегистрироваться
            </button>
        </div>
        <div class="pb-15"></div>
      </div>
    </div>
</template>

<script setup lang="ts">
import { ref, inject } from 'vue';
import { useRouter } from 'vue-router';
import AppButton from '@/components/ui/AppButton.vue';
import apiClient from '@/api';

const emit = defineEmits(['switchToRegister']);
const router = useRouter();
const auth = inject('auth') as { setLoggedIn: () => void }; // Получаем доступ к функции из provide

const buttonLoading = ref(false);
const email = ref('');
const password = ref('');
const errorMessage = ref('');

const tryLogin = async () => {
    buttonLoading.value = true;
    errorMessage.value = '';
    try {
      await apiClient.post('/Auth/login', {
        email: email.value,
        password: password.value
      });

      if (auth) {
        auth.setLoggedIn();
      }
      
        await router.push('/');

    } catch (error) {
      errorMessage.value = 'Неверный email или пароль.';
      console.error('Ошибка при входе:', error);
    } finally {
      buttonLoading.value = false;
    }
  };
</script>

<style scoped>
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}
</style>
```

---

### 📄 `Frontend/src/components/features/ModalForm.vue`

```vue
<script setup lang="ts">
import type { UserData, UserDataForAdmin } from '@/types/tables';
import AppButton from '@/components/ui/AppButton.vue'; 
import AppIcon from '../ui/AppIcon.vue';
import AppWarnButton from '../ui/AppWarnButton.vue';
import { ref, watch } from 'vue';

const props = defineProps<{
  userData: UserDataForAdmin | null 
  isOpen: boolean
}>();

const emit = defineEmits(['close']);

const onClose = () => {
  emit('close'); 
};

const onChange = () => {
  try
  {
    if(Object.keys(selectedRoles.value).length !== 0) {
      console.log(selectedRoles.value)
    }
    else {
      throw new Error
    }
  }
  catch
  {
    console.log("Роль не выбрана!")
  }
};

const userRoles = ref([
    { key: 'User', label: 'Пользователь' },
    { key: 'Teamlead', label: 'Тимлид' },
    { key: 'Moderator', label: 'Модератор' },
    { key: 'Admin', label: 'Админ' },
]);

const selectedRoles = ref<string[]>([]);


watch(() => props.userData, (newUserData) => {
  if (newUserData && newUserData.roles) {

    selectedRoles.value = [...newUserData.roles].flat();
  } else {

    selectedRoles.value = [];
  }
});

</script>

<template>
  <Transition name="fade">
    <div v-if="isOpen" class="fixed inset-0 backdrop-saturate-100 backdrop-opacity-100 backdrop-blur-sm backdrop-brightness-75 flex items-center justify-center" @click.self="onClose">
      <div class="bg-gray-50 rounded-2xl shadow-2xl max-w-2xl w-full p-16 mx-4 relative overflow-hidden">
        <template v-if="userData">
                <div class="flex justify-center -mt-5">
                  <div class="flex items-center justify-center bg-gray-300 w-32 h-32 rounded-full border-0 relative overflow-hidden">
                    <AppIcon icon_name="miniuser" class="absolute scale-600 text-white mt-10"></AppIcon>
                  </div>
                </div>
                <div class="text-center mt-2">
                  <h1 class="text-3xl font-bold text-gray-700 mb-2 inline">{{ userData.firstName + ' '}} </h1>
                  <h1 class="text-3xl font-bold text-gray-700 mb-2 inline">{{ userData.secondName + ' '}} </h1>
                  <h1 class="text-3xl font-bold text-gray-700 mb-2 inline">{{ userData.lastName}}</h1>
                </div>
                <p class="text-gray-600 font-bold mt-4">Email:</p>
                <p class="text-gray-600 mb-4">{{userData.email }}</p>
                <p class="text-gray-600 font-bold mt-4">Пол:</p>
                <p class="text-gray-600 mb-4">{{userData.gender === 'Male' ? 'Мужчина' : userData.gender === 'Female' ? 'Женщина' : ''}}</p>
                <p class="text-gray-600 font-bold mt-4 mb-1">Права доступа:</p>
                <div id="roles" v-for="value in userRoles">
                  <input 
                    type="checkbox" 
                    class="mr-2" 
                    :value="value.key"
                    :id="value.key"
                    v-model="selectedRoles"
                  ></input>
                  <p class="text-gray-600 mb-4  inline">{{value.label}}</p>
                </div>
                <p class="text-gray-600 font-bold mt-4">Дата регистрации:</p>
                <p class="text-gray-600 mb-4">{{userData.registrationTime.slice(0,10).replace(/-/g, ".") }}</p>
              </template>
            <template v-else>
                 <p class="text-gray-600">Загрузка данных...</p>
            </template>
            
            <div class="flex justify-start mt-10">
              <AppButton message="Закрыть" @click="onClose" />
            </div>
            <AppWarnButton message="Применить изменения" class="mt-3" @click="onChange" />
            <AppIcon icon_name="miniuser" 
              class="absolute right-25 bottom-32 scale-1800 text-gray-200">
            </AppIcon>
        </div>
    </div>
  </Transition>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
```

---

### 📄 `Frontend/src/components/features/RegisterForm.vue`

```vue
<template>
    <div class="flex flex-col items-center w-full max-w-md bg-gray-100 pt-30 rounded-3xl shadow-2xl shadow-gray-300">
      <h1 class="text-3xl font-bold mb-6 text-gray-700">Регистрация</h1>
      <div class="flex flex-col gap-4 w-full">
        <AppInput 
        i_type="email" 
        i_placeholder="Email" 
        v-model="email"/>
        <AppInput 
        i_type="text" 
        i_placeholder="Фамилия" 
        v-model="secondName"/>
        <AppInput 
        i_type="text" 
        i_placeholder="Имя" 
        v-model="firstName"/>
        <AppInput 
        i_type="text" 
        i_placeholder="Отчество" 
        v-model="lastName"/>
        <AppInput 
        i_type="password" 
        i_placeholder="Пароль" 
        v-model="password"/>
        <AppInput 
        i_type="password" 
        i_placeholder="Подтверждение пароля" 
        v-model="password_check"/>
        <div class="flex justify-center space-x-8 py-4">
          <label class="flex items-center space-x-3 cursor-pointer group">
            <input type="radio" id="one" value="Male" v-model="gender" class="hidden" />
            <span class="w-6 h-6 border-2 border-gray-300 rounded-full flex items-center justify-center group-hover:border-blue-500 transition-all duration-200">
              <span class="w-3 h-3 rounded-full bg-transparent group-has-[:checked]:bg-blue-500 transition-all duration-200"></span>
            </span>
            <span class="text-gray-700 group-has-[:checked]:text-blue-600 font-medium transition-colors duration-200">Мужчина</span>
          </label>
          
          <label class="flex items-center space-x-3 cursor-pointer group">
            <input type="radio" id="two" value="Female" v-model="gender" class="hidden" />
            <span class="w-6 h-6 border-2 border-gray-300 rounded-full flex items-center justify-center group-hover:border-pink-500 transition-all duration-200">
              <span class="w-3 h-3 rounded-full bg-transparent group-has-[:checked]:bg-pink-500 transition-all duration-200"></span>
            </span>
            <span class="text-gray-700 group-has-[:checked]:text-pink-600 font-medium transition-colors duration-200">Женщина</span>
          </label>
        </div>
        
        <Transition name="fade" mode="out-in">
          <p v-if="errorMessage" class="text-red-500 self-center px-5 text-pretty" key="error-message">
            {{ errorMessage }}
          </p>
        </Transition>
        <AppButton @click="tryRegister" :statusLoading="buttonLoading" message="Зарегистрироваться"
        class="self-center mb-10"/>
        <div class="text-center">
            <p class="inline">У вас уже есть аккаунт? </p>
            <button 
                @click="$emit('switchToLogin')"
                class="inline text-blue-500 cursor-pointer underline-offset-2 hover:underline hover:text-blue-700"
            >
                Войти
            </button>
        </div>
        <div class="pb-15"></div>
      </div>
    </div>
</template>

<script setup lang="ts">
import { ref, inject, reactive } from 'vue';
import { useRouter } from 'vue-router';
import AppButton from '@/components/ui/AppButton.vue';
import apiClient from '@/api';
import AppInput from '../ui/AppInput.vue';

const emit = defineEmits(['switchToLogin']);
const router = useRouter();
const auth = inject('auth') as { 
  setLoggedIn: () => void;
  checkAuthStatus: () => Promise<void>; // Убедись, что checkAuthStatus прокинут через provide
};
const buttonLoading = ref(false);

const firstName = ref('');
const secondName = ref('');
const lastName = ref('');

const email = ref('');

const password = ref('');
const password_check = ref('');

const gender = ref('');

const errorMessage = ref('')

const tryRegister = async () => {
    errorMessage.value = '';
    console.log(gender.value)
    if (password.value == password_check.value) {
      try {
          buttonLoading.value = true; 
          await apiClient.post('/Auth/register', {
            firstName: firstName.value,
            secondName: secondName.value,
            lastName: lastName.value,
            gender: gender.value,
            email: email.value,
            password: password.value
          });
          await apiClient.post('/Auth/login', {
            email: email.value,
            password: password.value
          });
          
          if (auth) {
            auth.setLoggedIn();
            await auth.checkAuthStatus();
            await router.push('/');
          }
          
          await router.push('/'); 
      } catch (error: any) {

        if (error.response && error.response.data && Array.isArray(error.response.data)) {
          errorMessage.value = error.response.data.map((e: any) => e.description).join(' ');
        }
        else{
          errorMessage.value = "Не все поля заполненны, либо не соответсвуют требованиям."
        }
        console.error('Ошибка при регистрации:', error);
      } finally {
        buttonLoading.value = false; 
      }
    }
    else
    {
      errorMessage.value = 'Пароли несовпадают!';
    }
  };
</script>


<style scoped>
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}
</style>
```

---

### 📄 `Frontend/src/components/features/SideBar.vue`

```vue
<template>
  <aside class="w-64 h-screen bg-gray-800 text-cyan-50 flex flex-col p-5">
    <div class="text-2xl font-bold mb-10 text-yellow-500">
      Курсач24
    </div>
    
    <nav class="flex flex-col flex-1">
      <ul class="flex-1">
        <li class="mb-4">
          <MenuButton 
            message="Главная" 
            icon="home" 
            route_path="/"
          />
        </li>
        <li v-if="!auth?.isLoggedIn.value" class="mb-4">
          <MenuButton 
            message="Войти" 
            icon="login" 
            route_path="/login_register"
          />
        </li>
        
        <template v-if="auth?.isLoggedIn.value">
            <li v-for="item in protectedItems" :key="item.route_path" class="mb-4">
              <MenuButton 
                :message="item.message" 
                :icon="item.icon" 
                :route_path="item.route_path"
              />
            </li>
        </template>
        
        <li v-if="!auth?.isLoggedIn.value" class="mb-4">
            <MenuButton 
            message="О проекте" 
            route_path="/about" 
            icon="about"/>
        </li>
      </ul>

      <div v-if="auth?.isLoggedIn.value">
        <div class="border-t border-gray-500 my-4"></div>
        <ul>
          <li>
            <MenuButton 
              message="Настройки" 
              route_path="/settings" 
              icon="settings"
              class="mb-4"
            />
            <MenuButton 
              message="Выйти" 
              route_path="#" 
              icon="logout"
              @click.prevent="handleLogout"
            />
          </li>
        </ul>
      </div>
    </nav>
  </aside>
</template>

<script setup lang="ts">
  import { ref, inject, type Ref } from 'vue';
  import { useRouter } from 'vue-router';
  import MenuButton from '../ui/MenuButton.vue';
  import type { MenuItem } from '@/types';
  import apiClient from '@/api';

  const router = useRouter();
  const auth = inject('auth') as { 
    isLoggedIn: Ref<boolean>; 
    setLoggedOut: () => void;
  };
  
  const protectedItems = ref<MenuItem[]>([
    {message: "Мои задачи", route_path: "/tasks", icon: "tasks"},
    {message: "Пользователи", route_path: "/users", icon: "users"},
    {message: "Команды", route_path: "/teams", icon: "teams"},
    {message: "Проекты", route_path: "/projects", icon: "projects"},
    {message: "Генератор", route_path: "/generator", icon: "generator"},
    {message: "Статистика", route_path: "/statictics", icon: "statictics"},
    {message: "Админ", route_path: "/admin_panel", icon: "audit"},

  ]);

  const handleLogout = async () => {
    try {
      await apiClient.post('/Auth/logout');
    } catch (error) {
      console.error('Ошибка при выходе на сервере:', error);
    } finally {
      if (auth) {
        auth.setLoggedOut();
      }
      await router.push('/login_register');
    }
  };
</script>

<style>
.router-link-exact-active {
  background-color: #4A5568;
  font-weight: bold;
}
</style>
```

---

### 📄 `Frontend/src/components/features/Datasets/AdminPanel.vue`

```vue
<template>
    <TableForm 
        class="flex-1"
        :items="users"
        :columns="userColumns"
    >

        <template #row="{ item }">
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 rounded-l-xl border-y-2 border-l-2 border-gray-200">
                <AppIcon icon_name="miniuser" class="mx-auto text-gray-700 hover:cursor-pointer hover:-translate-y-0.5 duration-500 hover:scale-110" @click="showUserDetails(item.id)"/>
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.shortName || '—' }}
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.gender || '—' }}
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.roles || '—' }}
            </td>

            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200 last:border-r-2 last:rounded-r-xl">
                {{ item.email || '—' }}
            </td>
        </template>
    </TableForm>
</template>

<script setup lang="ts">
import TableForm from '@/components/features/Table/TableForm.vue';
import { ref, onMounted, defineExpose } from 'vue';
import type { UserDataForAdmin } from '@/types/tables';
import apiClient from '@/api';
import AppIcon from '@/components/ui/AppIcon.vue'; 
const emit = defineEmits(['showUserDetails']);

const showUserDetails = (userId: string) => {

    emit('showUserDetails', userId);
}
const userColumns = ref([
    { key: 'actions', label: '' },
    { key: 'shortName', label: 'ФИО' },
    { key: 'gender', label: 'Пол' },
    { key: 'roles', label: 'Роль' },
    

    { key: 'email', label: 'Email' },
]);

const users = ref<UserDataForAdmin[]>([]);
const isLoading = ref(false);

const normalizeGender = (gender: string): string => 
  gender === 'Male' ? 'М' : gender === 'Female' ? 'Ж' : '';

const fetchUsers = async () => {
    isLoading.value = true;
    try {
        const response = await apiClient.get('/Users');
        const usersFromApi = response.data;
        if (Array.isArray(usersFromApi)) {
            users.value = usersFromApi.map((user: any) => ({
                id: user.id,
                shortName: user.shortName,
                roles: user.roles[0],
                gender: normalizeGender(user.gender),
                email: user.email,
                registrationTime: user.registrationTime,

            }));
        } else {
            console.error("Ожидался массив, но получен другой тип данных:", usersFromApi);
        }
    } catch (error) {
        console.error("Ошибка при загрузке пользователей:", error);
    } finally {
        isLoading.value = false;
    }
};


onMounted(() => {
    fetchUsers();
});

defineExpose({
  fetchUsers
});
</script>
```

---

### 📄 `Frontend/src/components/features/Datasets/UsersDataset.vue`

```vue
<template>
    <TableForm 
        class="flex-1"
        :items="users"
        :columns="userColumns"
    >
        <template #row="{ item }">
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 rounded-l-xl border-y-2 border-l-2 border-gray-200">
                <AppIcon icon_name="miniuser" class="mx-auto text-gray-700 hover:cursor-pointer hover:-translate-y-0.5 duration-500 hover:scale-110" @click="showUserDetails(item.id)"/>
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.shortName || '—' }}
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.gender || '—' }}
            </td>
            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200">
                {{ item.roles || '—' }}
            </td>

            <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 text-gray-700 font-semibold truncate border-y-2 border-gray-200 last:border-r-2 last:rounded-r-xl">
                {{ item.email || '—' }}
            </td>
        </template>
    </TableForm>
</template>

<script setup lang="ts">
import TableForm from '@/components/features/Table/TableForm.vue';
import { ref, onMounted, defineExpose } from 'vue';
import type { UserData } from '@/types/tables';
import apiClient from '@/api';
import AppIcon from '@/components/ui/AppIcon.vue';
import { useApiAsyncGet } from '@/composables';

const userColumns = ref([
    { key: 'actions', label: '' },
    { key: 'shortName', label: 'ФИО' },
    { key: 'gender', label: 'Пол' },
    { key: 'roles', label: 'Роль' },
    { key: 'email', label: 'Email' },
]);

const users = ref<UserData[]>([]);
const isLoading = ref(false);

const normalizeGender = (gender: string): string => 
  gender === 'Male' ? 'М' : gender === 'Female' ? 'Ж' : '';

const fetchUsers = async () => {
    isLoading.value = true;
    try {
        const response = await apiClient.get('/Users');
        const usersFromApi = response.data;
        if (Array.isArray(usersFromApi)) {
            users.value = usersFromApi.map((user: any) => ({
                id: user.id,
                shortName: user.shortName,
                gender: normalizeGender(user.gender),
                roles: user.roles[0],
                email: user.email,
                registrationTime: user.registrationTime,
            }));
        } else {
            console.error("Ожидался массив, но получен другой тип данных:", usersFromApi);
        }
    } catch (error) {
        console.error("Ошибка при загрузке пользователей:", error);
    } finally {
        isLoading.value = false;
    }
};

const showUserDetails = (userId: string) => {
    console.log(`Пользователь ${userId}`)
    const response = useApiAsyncGet(`/Users/${userId}`)
    console.log(response)  
}

onMounted(() => {
    fetchUsers();
});

defineExpose({
  fetchUsers
});
</script>
```

---

### 📄 `Frontend/src/components/features/Table/TableForm.vue`

```vue
<template>
    <div class="bg-white border-2 border-gray-300 rounded-3xl shadow-xl p-5 overflow-auto">
        <table class="w-full border-separate border-spacing-y-3">
            <thead>
                <tr>
                    <th v-for="column in columns" :key="column.key" class="text-left font-bold text-gray-500 p-3">
                        {{ column.label }}
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in items" :key="item.id" class="group transition-colors">
                    <slot name="row" :item="item"></slot>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    items: Array<any>, 
    columns: { key: string, label: string }[] 
}>();
</script>
```

---

### 📄 `Frontend/src/components/ui/AppButton.vue`

```vue
<template>
    <button :disabled="props.statusLoading" 
    class="cursor-pointer 
    bg-green-600 
    hover:bg-green-700 
    transition-colors 
    rounded-xl 
    px-5 py-2 
    text-white 
    font-semibold 
    fade 
    disabled:bg-amber-500
    disabled:cursor-default 
    shadow-md">
    <span v-if="!props.statusLoading">{{props.message}}</span>
    <span v-else class="inline-flex items-center gap-x-2">
        Загрузка
        <LoadingCircle class="text-base " /> 
    </span>
    </button>

</template>

<script setup lang="ts">
    import LoadingCircle from './LoadingCircle.vue';

    const props = defineProps<{
        message: string
        statusLoading?: boolean
    }>()

</script>

<style scoped>

</style>
```

---

### 📄 `Frontend/src/components/ui/AppIcon.vue`

```vue
<script setup lang="ts">
import { computed } from 'vue';

const icons = import.meta.glob('@/assets/icons/*.svg', { eager: true });

const props = defineProps({
    icon_name: String,
})

const iconComponent = computed(() => {
    const path = `/src/assets/icons/${props.icon_name}.svg`;
    return icons[path];
});
</script>

<template>
    <component :is="iconComponent"/>
</template>
```

---

### 📄 `Frontend/src/components/ui/AppInput.vue`

```vue
<template>
    <input 
          :type="props.i_type" 
          :placeholder="props.i_placeholder" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="model"
        >
</template>

<script setup lang="ts">
    const model = defineModel<string>()
    const props = defineProps<{
        i_type: string
        i_placeholder?: string
    }>()
</script>
```

---

### 📄 `Frontend/src/components/ui/AppSearch.vue`

```vue
<script setup lang="ts">
import AppIcon from './AppIcon.vue';

</script>

<template>
    <div class="ml-15 mb-5 h-12 w-70 bg-white rounded-3xl border-2 border-gray-300">
        <input type="search" class="w-full h-full py-3 px-8 text-md text-gray-400 outline-0" placeholder="Поиск">
    </div>
</template>
```

---

### 📄 `Frontend/src/components/ui/AppWarnButton.vue`

```vue
<template>
    <button :disabled="props.statusLoading" 
    class="cursor-pointer 
    bg-red-500 
    hover:bg-red-700 
    transition-colors 
    rounded-xl 
    px-5 py-2 
    text-white 
    font-semibold 
    fade 
    disabled:bg-amber-500
    disabled:cursor-default 
    shadow-md">
    <span v-if="!props.statusLoading">{{props.message}}</span>
    <span v-else class="inline-flex items-center gap-x-2">
        Загрузка
        <LoadingCircle class="text-base " /> 
    </span>
    </button>

</template>

<script setup lang="ts">
    import LoadingCircle from './LoadingCircle.vue';

    const props = defineProps<{
        message: string
        statusLoading?: boolean
    }>()

</script>

<style scoped>

</style>
```

---

### 📄 `Frontend/src/components/ui/LoadingCircle.vue`

```vue
<template>
    <div class="loader"></div>
</template>

<style scoped>
.loader {

  width: 1em;
  aspect-ratio: 1;
  border-radius: 50%;
  

  border: 0.16em solid currentColor;

  animation:
    l20-1 0.8s infinite linear alternate,
    l20-2 1.6s infinite linear;
}

@keyframes l20-1 {
   0%    {clip-path: polygon(50% 50%,0       0,  50%   0%,  50%    0%, 50%    0%, 50%    0%, 50%    0% )}
   12.5% {clip-path: polygon(50% 50%,0       0,  50%   0%,  100%   0%, 100%   0%, 100%   0%, 100%   0% )}
   25%   {clip-path: polygon(50% 50%,0       0,  50%   0%,  100%   0%, 100% 100%, 100% 100%, 100% 100% )}
   50%   {clip-path: polygon(50% 50%,0       0,  50%   0%,  100%   0%, 100% 100%, 50%  100%, 0%   100% )}
   62.5% {clip-path: polygon(50% 50%,100%    0, 100%   0%,  100%   0%, 100% 100%, 50%  100%, 0%   100% )}
   75%   {clip-path: polygon(50% 50%,100% 100%, 100% 100%,  100% 100%, 100% 100%, 50%  100%, 0%   100% )}
   100%  {clip-path: polygon(50% 50%,50%  100%,  50% 100%,   50% 100%,  50% 100%, 50%  100%, 0%   100% )}
}
@keyframes l20-2 { 
  0%    {transform:scaleY(1)  rotate(0deg)}
  49.99%{transform:scaleY(1)  rotate(135deg)}
  50%   {transform:scaleY(-1) rotate(0deg)}
  100%  {transform:scaleY(-1) rotate(-135deg)}
}
</style>
```

---

### 📄 `Frontend/src/components/ui/MenuButton.vue`

```vue
<template>
    <RouterLink 
        :to="props.route_path" 
        class="flex items-center p-2 rounded-lg hover:bg-gray-900 transition-colors hover:text-yellow-600 :text-yellow-500"
    >
        <!-- <component :is="iconMap[props.icon]" class="w-6 h-6" /> -->
        <AppIcon :icon_name="svgMap[props.icon]" class="w-6 h-6" />
        <span class="ml-3">{{props.message}}</span>
    </RouterLink>    
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router';
import AppIcon from './AppIcon.vue';

// import { ref, type Component } from 'vue'; 
// import HomeIcon from '@/components/icons/HomeIcon.vue';

// import LoginIcon from '@/components/icons/LoginIcon.vue';
// import LogoutIcon from '@/components/icons/LogoutIcon.vue';
// import SettingsIcon from '../icons/SettingsIcon.vue';

// import AboutIcon from '@/components/icons/AboutIcon.vue';
// import GeneratorIcon from '../icons/GeneratorIcon.vue';
// import StaticticsIcon from '../icons/StaticticsIcon.vue';

// import MenuTaskIcon from '../icons/MenuTaskIcon.vue';
// import ProjectsIcon from '../icons/ProjectsIcon.vue';
// import TeamsIcon from '../icons/TeamsIcon.vue';
// import UsersIcon from '../icons/UsersIcon.vue';

// const AppColor = ref("#fbbf24")

// const iconMap: { [key: string]: Component } = {
//   home: HomeIcon,
//   login: LoginIcon,
//   about: AboutIcon,
//   generator: GeneratorIcon,
//   settings: SettingsIcon,
//   logout: LogoutIcon,
//   projects: ProjectsIcon,
//   teams: TeamsIcon,
//   users: UsersIcon,
//   statistics: StaticticsIcon,
//   tasks: MenuTaskIcon
// };

const svgMap: {[key: string]: string}= {
  home: 'home',
  login: 'login',
  about: 'about',
  tasks: 'menutask',
  users: 'users',
  teams: 'teams',
  projects: 'projects',
  generator: 'generator',
  statictics: 'statictics',
  settings: 'settings',
  logout: 'logout',
  audit: 'audit'
};

const props = defineProps<{
    icon: string
    message: string
    route_path: string
    }>()
</script>

<style>
.router-link-exact-active {
  background-color: #4A5568;
  font-weight: bold;
}
.router-link-exact-active span {
  color: #f8c146; 
}

.router-link-exact-active svg {
  color: #f8c146; 
}
</style>
```

---

### 📄 `Frontend/src/composables/index.ts`

```typescript
export { useStrip } from './useStrip'
export { useApiSync } from './useApi'
export { useApiAsyncGet } from './useApi'
export { useApiAsyncDelete } from './useApi'
```

---

### 📄 `Frontend/src/composables/useApi.ts`

```typescript
import apiClient from "@/api";
import type { AxiosResponse } from "axios";

export function useApiSync(endpoint: string){
    
}

export function useApiAsyncDelete(endpoint: string){
    const apiAsync = async (point: string) => {
    const response = await apiClient.delete(point);
        if (response) {
            return response
        }
    }
    return apiAsync(endpoint)
}

export function useApiAsyncGet(endpoint: string){
    const apiAsync = async (point: string) => {
    const response = await apiClient.get(point);
        if (response) {
            return response
        }
    }
    return apiAsync(endpoint)
}
```

---

### 📄 `Frontend/src/composables/useStrip.ts`

```typescript
export function useStrip(text: string, toStrip: string,){
    return text.replace(toStrip, '')
}
```

---

### 📄 `Frontend/src/layouts/E_403.vue`

```vue
<script setup lang="ts">

import AppButton from '@/components/ui/AppButton.vue';
import AppIcon from '@/components/ui/AppIcon.vue';
import router from '@/router';
import {ref} from 'vue'

const message = ref('Вернуться')
const isLoading = ref(false)
const Getback = async () => {
    await router.push('/');
};

</script>

<template>
    <div class="h-screen flex items-center justify-center">
        <div class="w-170 h-120 bg-white border-2 border-gray-300 rounded-3xl shadow-xl grid grid-rows-[auto_auto_1fr_auto] p-10">
            <div class="flex items-center gap-4">
                <h1 class="text-8xl font-medium text-gray-600">403</h1>
                <AppIcon icon_name="error" class="w-26 h-26 ml-75 mt-2 text-orange-400"/>
            </div>
            
            <h5 class="mt-4 text-3xl font-semibold tracking-wide text-gray-500">
                Вы не авторизованы!
            </h5>
            
            <div>
                <p class="mt-10 text-2xl text-gray-500 text-justify">Ресурс, к которому вы хотите получить доступ, защищён, и у вас нет необходимых разрешений для доступа.</p>
            </div>
            
            <div class="flex justify-end mr-10 mb-3">
                <AppButton 
                    :message='message' 
                    :statusLoading="isLoading" 
                    @click="Getback"
                ></AppButton>
            </div>
        </div>
    </div>
</template>
```

---

### 📄 `Frontend/src/layouts/E_404.vue`

```vue
<script setup lang="ts">

import AppButton from '@/components/ui/AppButton.vue';
import AppIcon from '@/components/ui/AppIcon.vue';
import router from '@/router';
import {ref} from 'vue'

const message = ref('Вернуться')
const isLoading = ref(false)
const Getback = async () => {
    await router.push('/');
};

</script>

<template>
    <div class="h-screen flex items-center justify-center">
        <div class="w-170 h-120 bg-white border-2 border-gray-300 rounded-3xl shadow-xl grid grid-rows-[auto_auto_1fr_auto] p-10">
            <div class="flex items-center gap-4">
                <h1 class="text-8xl font-medium text-gray-600">404</h1>
                <AppIcon icon_name="error" class="w-26 h-26 ml-75 mt-2 text-orange-400"/>
            </div>
            
            <h5 class="mt-4 text-3xl font-semibold tracking-wide text-gray-500">
                Страничка не найдена!
            </h5>
            
            <div>
                <p class="mt-10 text-2xl text-gray-500 text-justify">Ресурс, к которому вы хотите получить доступ недоступен.</p>
            </div>
            
            <div class="flex justify-end mr-10 mb-3">
                <AppButton 
                    :message='message' 
                    :statusLoading="isLoading" 
                    @click="Getback"
                ></AppButton>
            </div>
        </div>
    </div>
</template>
```

---

### 📄 `Frontend/src/layouts/MainLayout.vue`

```vue
<template>
  <div class="flex h-screen">
    
    <SideBar /> 
    <main class="flex-1 p-8 overflow-y-auto">
      <RouterView />
    </main>

</div>
</template>

<script setup lang="ts">
import SideBar from '@/components/features/SideBar.vue' 
import { ref } from 'vue';
</script>
```

---

### 📄 `Frontend/src/router/index.ts`

```typescript
import { createRouter, createWebHistory } from 'vue-router'
import MainLayout from '../layouts/MainLayout.vue'
import HomePage from '../views/HomePage.vue'
import AboutPage from '../views/AboutPage.vue'
import SettingsPage from '@/views/SettingsPage.vue'

// import LoginPage from '@/views/auth/LoginPage.vue'
// import RegisterPage from '@/views/auth/RegisterPage.vue'

import GeneratorPage from '../views/GeneratorPage.vue'
import StatisticsPage from '@/views/StatisticsPage.vue'

import TasksPage from '@/views/TasksPage.vue'
import TeamsPage from '@/views/TeamsPage.vue'
import ProjectsPage from '@/views/ProjectsPage.vue'
import UsersPage from '@/views/UsersPage.vue'
import LoginRegisterPage from '@/views/auth/LoginRegisterPage.vue'
import AdminPage from '@/views/AdminPage.vue'

export const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: MainLayout, 
      children: [ 
        {
          path: '', 
          name: 'home',
          component: HomePage,
        },
        {
          path: 'about',
          name: 'about',  
          component: AboutPage,
        },
        {
          path: 'login_register', 
          name: 'login_register',  
          component: LoginRegisterPage,
        },

        {
          path: 'generator', 
          name: 'generator',  
          component: GeneratorPage,
        },
        {
          path: 'settings', 
          name: 'settings',  
          component: SettingsPage,
        },
        {
          path: 'projects', 
          name: 'projects',  
          component: ProjectsPage,
        },
        {
          path: 'teams', 
          name: 'teams',  
          component: TeamsPage,
        },
        {
          path: 'users', 
          name: 'users',  
          component: UsersPage,
        },
        {
          path: 'statictics', 
          name: 'statictics',   
          component: StatisticsPage,
        },
        {
          path: 'tasks', 
          name: 'tasks',  
          component: TasksPage,
        },
        {
          path: 'logout', 
          name: 'logout',  
          component: MainLayout,
        },
        {
          path: 'admin_panel', 
          name: 'admin_panel',  
          component: AdminPage,
        },
      ],
    },
  ],
})

export default router
```

---

### 📄 `Frontend/src/types/index.ts`

```typescript
export interface MenuItem {
  message: string;
  icon: string;
  route_path: string; 
}

export interface Task {
  id: number;
  tittle: string;
  text_task: string;
}
```

---

### 📄 `Frontend/src/types/tables.ts`

```typescript
import type { Component } from "vue";

export interface UserData {
  id: string;
  gender: string;
  firstName?: string;
  secondName?: string;
  lastName?: string | null; 
  shortName: string;
  roles: Array<[]>;
  email: string | null; 
  registrationTime: string;
}

export interface UserDataForAdmin {
  id: string;
  gender: string;
  firstName?: string;
  secondName?: string;
  lastName?: string | null; 
  shortName: string;
  roles: Array<[]>;
  email: string | null; 
  registrationTime: string;

}
```

---

### 📄 `Frontend/src/views/AboutPage.vue`

```vue
<script setup lang="ts">
  import AppIcon from '@/components/ui/AppIcon.vue';
</script>

<template>
  <div>
    <h1 class="text-3xl font-bold">О проекте</h1>
    <p class="mt-4">
      Это мой курсач.
    </p>
  </div>
</template>
```

---

### 📄 `Frontend/src/views/AdminPage.vue`

```vue
<template>
    <AppWarnButton message="Удалить всех пользователей" @click="delAllUsers" :statusLoading="isLoading"></AppWarnButton>
    <AdminPanel @show-user-details="handleShowUserDetails" class="mt-7"/>
    <ModalForm 
        :is-open="isModalOpen" 
        :user-data="selectedUser" 
        @close="isModalOpen = false"
    />
</template>

<script setup lang="ts">
import apiClient from '@/api';
import AdminPanel from '@/components/features/Datasets/AdminPanel.vue';
import ModalForm from '@/components/features/ModalForm.vue';
import AppWarnButton from '@/components/ui/AppWarnButton.vue';
import { useApiAsyncDelete } from '@/composables/useApi';
import { ref } from 'vue';

const isLoading = ref(false)

const delAllUsers = async () => {
    isLoading.value = true
    useApiAsyncDelete('/Users/DeleteAll')
    isLoading.value = false
}

const isModalOpen = ref(false);
const selectedUser = ref(null); 

const handleShowUserDetails = async (userId: string) => {
    try {
        const response = await apiClient.get(`/Users/${userId}`);
        if(response) {
            selectedUser.value = response.data;
            isModalOpen.value = true;
        }
    } catch (error) {
        console.error("Не удалось получить данные пользователя:", error);
    }
};
</script>
```

---

### 📄 `Frontend/src/views/GeneratorPage.vue`

```vue
<template>
    <!-- <span>Таблица: </span> -->
    <select v-model="selected" class="w-50 border rounded-lg mr-4 py-1 bg-gray-100 border-gray-400">
        <option disabled value="">Выберите таблицу</option>
        <option v-for="table in tables
        ">{{table}}</option>
    </select>

    <input v-model="cnt_generations" type="number" class="w-50 border rounded-lg mr-4 py-1 bg-gray-100 border-gray-400 outline-0" placeholder="Количество генераций">


    <AppButton :message='message' :statusLoading="isLoading" @click="GenStart"></AppButton>
    <span class="pl-4 text-xl text-gray-800">{{status}}</span>
    <div class="flex justify-center pt-7">
        <UsersDataset ref="usersDatasetRef"/>
    </div>

</template>

<script setup lang="ts">
import apiClient from '@/api';
import AppButton from '@/components/ui/AppButton.vue';
import {onMounted, ref} from 'vue'
import UsersDataset from '@/components/features/Datasets/UsersDataset.vue';
const tables = ref<string[]>([])
const usersDatasetRef = ref<InstanceType<typeof UsersDataset> | null>(null);
let status = ref('')
let selected = ref('')
let cnt_generations = ref('')
let isLoading = ref(false)
let message = ref('Сгенерировать')

const getTables = async () => {
    try {
    tables.value = (await apiClient.get('/Generator')).data
    }   
    catch {

    }
};

onMounted(getTables);

const GenStart = async () => {
    console.log("Начало генерации...");
    isLoading.value = true;
    status.value = '';
    try {
        await apiClient.post('/Generator', {
            generatorTable: selected.value,
            countGenerations: cnt_generations.value,
        });
        status.value = 'Готово!';

        if (usersDatasetRef.value) {
            await usersDatasetRef.value.fetchUsers();
        }

    } catch {
        status.value = 'Генерация не завершена!';
    } finally {
        isLoading.value = false;
    }
};

</script>

<style scoped>

</style>
```

---

### 📄 `Frontend/src/views/HomePage.vue`

```vue
<template>
  <AppButton message='Создать задачу' :statusLoading="buttonLoading" @click="addTask"></AppButton>

  <TransitionGroup name="fade" tag="div" class="grid grid-cols-3">
    <CardTask
      v-for="cur_task in tasks"
      :key="cur_task.id"
      :tittle="cur_task.tittle"
      :task="cur_task.text_task"
    />
  </TransitionGroup>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import CardTask from '@/components/CardTask.vue'
import AppButton from '@/components/ui/AppButton.vue'

import type { Task } from '@/types'

const tasks = ref<Task[]>([])
const buttonLoading = ref(false)

function addTask(event: Event) {
  console.log("Задача добавлена!")
  tasks.value.push({
    id: Date.now(), 
    tittle: `Заголовок ${tasks.value.length + 1}`,
    text_task: `текст ${tasks.value.length + 1}`
  })
  // buttonLoading.value = !buttonLoading.value
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
```

---

### 📄 `Frontend/src/views/ProjectsPage.vue`

```vue
<template>

</template>

<script setup lang="ts">

</script>

<style scoped>
    
</style>
```

---

### 📄 `Frontend/src/views/SettingsPage.vue`

```vue
<template></template>
```

---

### 📄 `Frontend/src/views/StatisticsPage.vue`

```vue
<template></template>
```

---

### 📄 `Frontend/src/views/TasksPage.vue`

```vue
<template>
    
</template>
```

---

### 📄 `Frontend/src/views/TeamsPage.vue`

```vue
<template>

</template>

<script setup lang="ts">

</script>

<style scoped>
    
</style>
```

---

### 📄 `Frontend/src/views/UsersPage.vue`

```vue
<template>
    <AppSearch></AppSearch>
    <div class="flex justify-center">
        <UsersDataset/>
    </div>
</template>

<script setup lang="ts">
import AppSearch from '@/components/ui/AppSearch.vue';
import UsersDataset from '@/components/features/Datasets/UsersDataset.vue';
</script>
```

---

### 📄 `Frontend/src/views/auth/LoginRegisterPage.vue`

```vue
// views/auth/LoginRegisterPage.vue

<template>
  <div class="min-h-screen flex items-center justify-center pb-50">
    <Transition name="fade" mode="out-in">
      <component 
        :is="activeComponent" 
        @switch-to-register="activeComponentName = 'RegisterForm'"
        @switch-to-login="activeComponentName = 'LoginForm'"
      />
    </Transition>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, shallowRef } from 'vue';
import LoginForm from '@/components/features/LoginForm.vue';
import RegisterForm from '@/components/features/RegisterForm.vue';

const components = {
  LoginForm,
  RegisterForm
};

const activeComponentName = ref<'LoginForm' | 'RegisterForm'>('LoginForm');

const activeComponent = computed(() => components[activeComponentName.value]);
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
```
