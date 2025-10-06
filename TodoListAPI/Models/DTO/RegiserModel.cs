namespace TodoListAPI.Models.DTO
{
    public class RegisterModel
    {
        // Тут потом будет и имя, и всякая прочая шляпа
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}