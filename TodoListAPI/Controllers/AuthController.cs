using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoListAPI.Services; // <-- Используем наш сервис
using TodoListAPI.Models.DTO; // <-- Не забудь добавить этот using!
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
        public async Task<IActionResult> Register(string email, string password)
        {
            var result = await _authService.RegisterUserAsync(email, password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User created successfully!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Теперь мы берем email и password из модели
            var token = await _authService.LoginUserAsync(model.Email, model.Password);

            if (token != null)
            {
                return Ok(new { Token = token });
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }
    }
}