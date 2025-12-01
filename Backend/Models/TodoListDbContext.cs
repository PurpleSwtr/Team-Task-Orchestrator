using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Backend.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Backend.Models;

public partial class TodoListDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TodoListDbContext(DbContextOptions<TodoListDbContext> options, IHttpContextAccessor httpContextAccessor = null)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Status> Statuses { get; set; }
    public virtual DbSet<Task> Tasks { get; set; }
    public virtual DbSet<TasksProject> TasksProjects { get; set; }
    public virtual DbSet<TasksUser> TasksUsers { get; set; }
    public virtual DbSet<Team> Teams { get; set; }
    public virtual DbSet<UsersCommand> UsersCommands { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = currentUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = currentUserId;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = currentUserId;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<Task>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<Team>().HasQueryFilter(e => e.DeletedAt == null);

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK_Статус");
            entity.ToTable("Status");
            entity.Property(e => e.IdStatus).HasColumnName("id-status");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.IdTask).HasName("PK_Задачи");

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

        modelBuilder.Entity<TasksProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ЗадачаПроект");

            entity.ToTable("Tasks - Projects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProject).HasColumnName("id-project");
            entity.Property(e => e.IdTask).HasColumnName("id-task");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.TasksProjects)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Проекты_Проекты");

            entity.HasOne(d => d.IdTaskNavigation).WithMany(p => p.TasksProjects)
                .HasForeignKey(d => d.IdTask)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Проекты_Задачи");
        });

        modelBuilder.Entity<TasksUser>(entity =>
        {
            entity.HasKey(e => e.IdAssignees).HasName("PK_Задачи - Пользователи");

            entity.ToTable("Tasks - Users");

            entity.Property(e => e.IdAssignees)
                .HasMaxLength(255)
                .HasColumnName("id_assignees");
            entity.Property(e => e.IdTask).HasColumnName("id_task");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdTaskNavigation).WithMany(p => p.TasksUsers)
                .HasForeignKey(d => d.IdTask)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Пользователи_Задачи");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TasksUsers)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Задачи - Пользователи_Пользователи");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.IdTeam).HasName("PK_Команды");

            entity.Property(e => e.IdTeam).HasColumnName("id_team");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.TeamName)
                .HasMaxLength(255)
                .HasColumnName("team_name");
            entity.Property(e => e.UserAccess)
                .HasMaxLength(255)
                .HasColumnName("user_access");
        });

        modelBuilder.Entity<UsersCommand>(entity =>
        {
            entity.HasKey(e => e.IdConnection).HasName("PK_Пользователи - Команды");

            entity.ToTable("Users - Commands");

            entity.Property(e => e.IdConnection).HasColumnName("id_connection");
            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdTeamNavigation).WithMany(p => p.UsersCommands)
                .HasForeignKey(d => d.IdTeam)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Пользователи - Команды_Команды");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UsersCommands)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Пользователи - Команды_Пользователи");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
