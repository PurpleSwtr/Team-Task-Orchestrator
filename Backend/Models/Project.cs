using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Project
{
    public int IdProject { get; set; }

    public int? IdTeam { get; set; }

    public string? ProjectName { get; set; }

    public string? ProjectType { get; set; }

    public string? Descryption { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? CreatedBy { get; set; }

    public string? CreatedAt { get; set; }

    public string? EditedBy { get; set; }

    public string? EditedAt { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<TasksProject> TasksProjects { get; set; } = new List<TasksProject>();
}
