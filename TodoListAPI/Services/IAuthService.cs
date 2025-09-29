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