using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Interfaces;

namespace Backend.Models;

public partial class Project : IAuditable
{
    [Key]
    [Column("id_project")]
    public int IdProject { get; set; }

    [Column("id_team")]
    public int? IdTeam { get; set; }

    [Column("project_name")]
    [MaxLength(255)]
    public string? ProjectName { get; set; }

    [Column("project_type")]
    [MaxLength(255)]
    public string? ProjectType { get; set; }

    [Column("descryption")]
    public string? Descryption { get; set; }

    [Column("start_date")]
    public DateTime? StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_by")]
    [MaxLength(450)]
    public string? CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("edited_by")]
    [MaxLength(450)]
    public string? UpdatedBy { get; set; }

    [Column("edited_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<TasksProject> TasksProjects { get; set; } = new List<TasksProject>();
}