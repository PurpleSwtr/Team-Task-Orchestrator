using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string? FirstName { get; set; }

    public string? SecondName { get; set; }

    public string? PatronymicName { get; set; }

    public int? IdUserStatus { get; set; }

    public string? Email { get; set; }

    public DateTime? RegistrationTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? CreatedAt { get; set; }

    public string? EditedBy { get; set; }

    public string? EditedAt { get; set; }

    public string? Notes { get; set; }

    public virtual Status? IdUserStatusNavigation { get; set; }

    public virtual ICollection<TasksUser> TasksUsers { get; set; } = new List<TasksUser>();

    public virtual ICollection<UsersCommand> UsersCommands { get; set; } = new List<UsersCommand>();
}
