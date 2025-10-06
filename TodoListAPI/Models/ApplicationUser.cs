using Microsoft.AspNetCore.Identity;
using System.Collections.Generic; // Добавьте этот using

namespace TodoListAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? PatronymicName { get; set; }
        public DateTime? RegistrationTime { get; set; }
        public string? Notes { get; set; }
        public int? IdUserStatus { get; set; }
        public virtual Status? IdUserStatusNavigation { get; set; }
        public virtual ICollection<TasksUser> TasksUsers { get; set; } = new List<TasksUser>();
        public virtual ICollection<UsersCommand> UsersCommands { get; set; } = new List<UsersCommand>();
    }
}