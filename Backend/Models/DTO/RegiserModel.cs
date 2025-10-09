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