using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class TasksUser
{
    public string IdAssignees { get; set; } = null!;
    public int IdTask { get; set; }
    public string IdUser { get; set; }
    public virtual Task IdTaskNavigation { get; set; } = null!;
    public virtual ApplicationUser IdUserNavigation { get; set; } = null!;
}
