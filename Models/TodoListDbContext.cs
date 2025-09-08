using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TodoListAPI.Models;

public partial class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Задачи> Задачиs { get; set; }

    public virtual DbSet<ЗадачиПользователи> ЗадачиПользователиs { get; set; }

    public virtual DbSet<ЗадачиПроекты> ЗадачиПроектыs { get; set; }

    public virtual DbSet<Команды> Командыs { get; set; }

    public virtual DbSet<Пользователи> Пользователиs { get; set; }

    public virtual DbSet<ПользователиКоманды> ПользователиКомандыs { get; set; }

    public virtual DbSet<Проекты> Проектыs { get; set; }

    public virtual DbSet<Статус> Статусs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Задачи>(entity =>
        {
            entity.HasKey(e => e.IdTask);

            entity.ToTable("Задачи");

            entity.Property(e => e.IdTask).HasColumnName("id_task");
            entity.Property(e => e.CompleteDate)
                .HasColumnType("datetime")
                .HasColumnName("complete_date");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeadlineDate)
                .HasColumnType("datetime")
                .HasColumnName("deadline_date");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EditedAt)
                .HasColumnType("datetime")
                .HasColumnName("edited_at");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(50)
                .HasColumnName("edited_by");
            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.Notes)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("notes");
            entity.Property(e => e.Priority)
                .HasMaxLength(255)
                .HasColumnName("priority");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.TaskName)
                .HasMaxLength(255)
                .HasColumnName("task_name");
        });

        modelBuilder.Entity<ЗадачиПользователи>(entity =>
        {
            entity.HasKey(e => e.IdAssignees);

            entity.ToTable("Задачи - Пользователи");

            entity.Property(e => e.IdAssignees)
                .HasMaxLength(255)
                .HasColumnName("id_assignees");
            entity.Property(e => e.IdTask).HasColumnName("id_task");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdTaskNavigation).WithMany(p => p.ЗадачиПользователиs)
                .HasForeignKey(d => d.IdTask)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Пользователи_Задачи");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ЗадачиПользователиs)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Пользователи_Пользователи");
        });

        modelBuilder.Entity<ЗадачиПроекты>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ЗадачаПроект");

            entity.ToTable("Задачи - Проекты");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdЗадача).HasColumnName("ID_Задача");
            entity.Property(e => e.IdПроект).HasColumnName("ID_Проект");

            entity.HasOne(d => d.IdЗадачаNavigation).WithMany(p => p.ЗадачиПроектыs)
                .HasForeignKey(d => d.IdЗадача)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Проекты_Задачи");

            entity.HasOne(d => d.IdПроектNavigation).WithMany(p => p.ЗадачиПроектыs)
                .HasForeignKey(d => d.IdПроект)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Проекты_Проекты");
        });

        modelBuilder.Entity<Команды>(entity =>
        {
            entity.HasKey(e => e.IdTeam);

            entity.ToTable("Команды");

            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.CratedBy)
                .HasMaxLength(255)
                .HasColumnName("crated_by");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EditedAt)
                .HasMaxLength(255)
                .HasColumnName("edited_at");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(255)
                .HasColumnName("edited_by");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.TeamName)
                .HasMaxLength(255)
                .HasColumnName("team_name");
            entity.Property(e => e.UserAccess)
                .HasMaxLength(255)
                .HasColumnName("user_access");
        });

        modelBuilder.Entity<Пользователи>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.ToTable("Пользователи");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("created_by");
            entity.Property(e => e.EditedAt)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("edited_at");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("edited_by");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.IdUserStatus).HasColumnName("id_user_status");
            entity.Property(e => e.Notes)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("notes");
            entity.Property(e => e.PatronymicName)
                .HasMaxLength(255)
                .HasColumnName("patronymic_name");
            entity.Property(e => e.RegistrationTime)
                .HasColumnType("datetime")
                .HasColumnName("registration_time");
            entity.Property(e => e.SecondName)
                .HasMaxLength(255)
                .HasColumnName("second_name");

            entity.HasOne(d => d.IdUserStatusNavigation).WithMany(p => p.Пользователиs)
                .HasForeignKey(d => d.IdUserStatus)
                .HasConstraintName("FK_Пользователи_Статус");
        });

        modelBuilder.Entity<ПользователиКоманды>(entity =>
        {
            entity.HasKey(e => e.IdConnection);

            entity.ToTable("Пользователи - Команды");

            entity.Property(e => e.IdConnection).HasColumnName("id_connection");
            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdTeamNavigation).WithMany(p => p.ПользователиКомандыs)
                .HasForeignKey(d => d.IdTeam)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Пользователи - Команды_Команды");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ПользователиКомандыs)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Пользователи - Команды_Пользователи");
        });

        modelBuilder.Entity<Проекты>(entity =>
        {
            entity.HasKey(e => e.IdProject);

            entity.ToTable("Проекты");

            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("created_by");
            entity.Property(e => e.Descryption).HasColumnName("descryption");
            entity.Property(e => e.EditedAt)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("edited_at");
            entity.Property(e => e.EditedBy)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("edited_by");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.Notes)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("notes");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(255)
                .HasColumnName("project_name");
            entity.Property(e => e.ProjectType)
                .HasMaxLength(255)
                .HasColumnName("project_type");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
        });

        modelBuilder.Entity<Статус>(entity =>
        {
            entity.HasKey(e => e.IdStatus);

            entity.ToTable("Статус");

            entity.Property(e => e.IdStatus).HasColumnName("id-status");
            entity.Property(e => e.Название).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
