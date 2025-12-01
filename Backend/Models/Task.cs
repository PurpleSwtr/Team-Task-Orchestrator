using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Interfaces;

namespace Backend.Models;

public partial class Task : IAuditable
{
    [Key]
    [Column("id_task")]
    public int IdTask { get; set; }

    [Column("task_name")]
    [MaxLength(255)]
    public string? TaskName { get; set; }

    [Column("description")]
    [MaxLength(255)]
    public string? Description { get; set; }

    [Column("status")]
    [MaxLength(255)]
    public string? Status { get; set; }

    [Column("priority")]
    [MaxLength(255)]
    public string? Priority { get; set; }

    [Column("deadline_date")]
    public DateTime? DeadlineDate { get; set; }

    [Column("complete_date")]
    public DateTime? CompleteDate { get; set; }

    [Column("id_project")]
    public int? IdProject { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_by")]
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
    public virtual ICollection<TasksUser> TasksUsers { get; set; } = new List<TasksUser>();
}