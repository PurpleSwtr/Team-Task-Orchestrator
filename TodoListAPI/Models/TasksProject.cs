using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class TasksProject
{
    public int Id { get; set; }

    public int IdЗадача { get; set; }

    public int IdПроект { get; set; }

    public virtual Task IdЗадачаNavigation { get; set; } = null!;

    public virtual Project IdПроектNavigation { get; set; } = null!;
}
