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