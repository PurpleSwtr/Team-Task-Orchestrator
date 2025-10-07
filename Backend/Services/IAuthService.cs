using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(string email, string password);

        Task<string> LoginUserAsync(string email, string password);
    }
}