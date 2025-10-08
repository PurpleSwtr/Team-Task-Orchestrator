namespace Backend.Models.DTO
{
    public class RegisterModel
    {
        public required string FirstName { get; set; }
        public required string SecondName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}