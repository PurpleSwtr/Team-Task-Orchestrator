using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class Status
{
    public int IdStatus { get; set; }

    public string? Название { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
