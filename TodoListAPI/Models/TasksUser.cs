using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class TasksUser
{
    public string IdAssignees { get; set; } = null!;

    public int IdTask { get; set; }

    public int IdUser { get; set; }

    public virtual Task IdTaskNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
