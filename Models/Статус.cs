using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class Статус
{
    public int IdStatus { get; set; }

    public string? Название { get; set; }

    public virtual ICollection<Пользователи> Пользователиs { get; set; } = new List<Пользователи>();
}
