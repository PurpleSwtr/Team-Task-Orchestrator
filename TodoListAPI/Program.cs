using TodoListAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TodoListAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Data.SqlClient;

// НЕ ИСПОЛЬЗУЕМ using Task = System.Threading.Tasks.Task;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TodoListDbContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

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

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var dbContext = services.GetRequiredService<TodoListDbContext>();

    bool dbReady = false;
    int retries = 0;
    const int maxRetries = 10;

    while (!dbReady && retries < maxRetries)
    {
        try
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                dbReady = true;
                logger.LogInformation("Database is ready. Applying migrations...");
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
            }
        }
        catch (SqlException)
        {
            retries++;
            logger.LogWarning("Database is not ready yet. Waiting... (Attempt {Attempt})", retries);
            // Явно указываем полный путь к классу Task
            await System.Threading.Tasks.Task.Delay(5000);
        }
    }

    if (!dbReady)
    {
        logger.LogError("Database could not be reached after {MaxRetries} attempts. Application is shutting down.", maxRetries);
        return; 
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

// Возвращаем аутентификацию на место
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();