public class UserWithRolesDto
{
    public string Id { get; set; }
    public string? ShortName { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    public DateTime? RegistrationTime { get; set; }
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? LastName { get; set; }
    public IList<string> Roles { get; set; }
}