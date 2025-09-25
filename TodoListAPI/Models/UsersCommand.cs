using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class UsersCommand
{
    public int IdConnection { get; set; }

    public int IdUser { get; set; }

    public int IdTeam { get; set; }

    public virtual Team IdTeamNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
