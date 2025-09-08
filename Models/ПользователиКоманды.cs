using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class ПользователиКоманды
{
    public int IdConnection { get; set; }

    public int IdUser { get; set; }

    public int IdTeam { get; set; }

    public virtual Команды IdTeamNavigation { get; set; } = null!;

    public virtual Пользователи IdUserNavigation { get; set; } = null!;
}
