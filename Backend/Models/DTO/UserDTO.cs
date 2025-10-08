namespace Backend.Models.DTO
{
    public class UserDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateTime? RegistrationTime { get; set; }
        public int? IdUserStatus { get; set; }
    }
}