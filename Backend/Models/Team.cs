using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Interfaces;

namespace Backend.Models;

public partial class Team : IAuditable
{
    [Key]
    [Column("id_team")]
    public int IdTeam { get; set; }

    [Column("team_name")]
    [MaxLength(255)]
    public string? TeamName { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("user_access")]
    [MaxLength(255)]
    public string? UserAccess { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    [MaxLength(450)]
    public string? CreatedBy { get; set; }

    [Column("edited_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("edited_by")]
    [MaxLength(450)]
    public string? UpdatedBy { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<UsersCommand> UsersCommands { get; set; } = new List<UsersCommand>();
}