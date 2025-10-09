public class UserWithRolesDto
{
    public string Id { get; set; }
    public string? ShortName { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    public IList<string> Roles { get; set; }
}