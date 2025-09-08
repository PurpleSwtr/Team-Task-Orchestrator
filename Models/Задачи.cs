using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class Задачи
{
    public int IdTask { get; set; }

    public string? TaskName { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? Priority { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeadlineDate { get; set; }

    public DateTime? CompleteDate { get; set; }

    public int? IdProject { get; set; }

    public string? EditedBy { get; set; }

    public DateTime? EditedAt { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<ЗадачиПользователи> ЗадачиПользователиs { get; set; } = new List<ЗадачиПользователи>();

    public virtual ICollection<ЗадачиПроекты> ЗадачиПроектыs { get; set; } = new List<ЗадачиПроекты>();
}
