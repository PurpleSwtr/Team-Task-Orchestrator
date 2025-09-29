using TodoListAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TodoListAPI.Services; // <-- Добавляем using для сервисов
using Microsoft.AspNetCore.Authentication.JwtBearer; // <-- Добавляем using для JWT
using Microsoft.IdentityModel.Tokens; // <-- Добавляем using для токенов
using System.Text; // <-- Добавляем using для кодировки

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration; // <-- Получаем доступ к конфигурации

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TodoListDbContext>(options =>
    options.UseSqlServer(connectionString));

// Настраиваем Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>() // <-- Можно добавить IdentityRole, если нужны роли
    .AddEntityFrameworkStores<TodoListDbContext>()
    .AddDefaultTokenProviders();

// === НАСТРОЙКА JWT АУТЕНТИФИКАЦИИ ===
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
// ===================================

// Регистрируем наш сервис
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Включаем аутентификацию и авторизацию. Порядок важен!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();