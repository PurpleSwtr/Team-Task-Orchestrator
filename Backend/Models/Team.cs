using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Team
{
    public int IdTeam { get; set; }

    public string? TeamName { get; set; }

    public string? Description { get; set; }

    public string? UserAccess { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CratedBy { get; set; }

    public string? EditedAt { get; set; }

    public string? EditedBy { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<UsersCommand> UsersCommands { get; set; } = new List<UsersCommand>();
}
