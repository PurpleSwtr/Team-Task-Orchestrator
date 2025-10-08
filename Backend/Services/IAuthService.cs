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