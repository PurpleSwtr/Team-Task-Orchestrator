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