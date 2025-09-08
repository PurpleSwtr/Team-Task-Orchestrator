using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class Пользователи
{
    public int IdUser { get; set; }

    public string? FirstName { get; set; }

    public string? SecondName { get; set; }

    public string? PatronymicName { get; set; }

    public int? IdUserStatus { get; set; }

    public string? Email { get; set; }

    public DateTime? RegistrationTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? CreatedAt { get; set; }

    public string? EditedBy { get; set; }

    public string? EditedAt { get; set; }

    public string? Notes { get; set; }

    public virtual Статус? IdUserStatusNavigation { get; set; }

    public virtual ICollection<ЗадачиПользователи> ЗадачиПользователиs { get; set; } = new List<ЗадачиПользователи>();

    public virtual ICollection<ПользователиКоманды> ПользователиКомандыs { get; set; } = new List<ПользователиКоманды>();
}
