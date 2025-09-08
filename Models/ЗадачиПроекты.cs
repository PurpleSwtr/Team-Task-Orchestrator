using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class ЗадачиПроекты
{
    public int Id { get; set; }

    public int IdЗадача { get; set; }

    public int IdПроект { get; set; }

    public virtual Задачи IdЗадачаNavigation { get; set; } = null!;

    public virtual Проекты IdПроектNavigation { get; set; } = null!;
}
