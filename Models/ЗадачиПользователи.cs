using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class ЗадачиПользователи
{
    public string IdAssignees { get; set; } = null!;

    public int IdTask { get; set; }

    public int IdUser { get; set; }

    public virtual Задачи IdTaskNavigation { get; set; } = null!;

    public virtual Пользователи IdUserNavigation { get; set; } = null!;
}
