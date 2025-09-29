using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

// Models/Status.cs
public partial class Status
{
    public int IdStatus { get; set; }
    public string? Название { get; set; }
    public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>(); // <--- ИЗМЕНИТЬ НА ApplicationUser
}
