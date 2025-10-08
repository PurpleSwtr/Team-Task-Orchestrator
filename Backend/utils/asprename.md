Отличный вопрос! Переименование таблиц, созданных ASP.NET Core Identity (таких как `AspNetUsers`), — это стандартная задача для кастомизации. Просто так переименовать таблицу в базе данных нельзя, потому что Entity Framework и Identity "ожидают" найти таблицы с определёнными именами.

Правильный способ — указать Entity Framework Core, какие имена вы хотите использовать. Это делается в вашем `DbContext`.

Вот пошаговая инструкция, как безопасно переименовать `AspNetUsers` в `Users` и другие связанные таблицы.

### Почему это требует особого подхода?

ASP.NET Core Identity по умолчанию создаёт целый набор взаимосвязанных таблиц:
*   `AspNetUsers` - Пользователи
*   `AspNetRoles` - Роли
*   `AspNetUserRoles` - Связь пользователей и ролей
*   `AspNetUserClaims` - "Утверждения" или данные о пользователе
*   `AspNetUserLogins` - Для входа через внешние сервисы (Google, Facebook)
*   `AspNetUserTokens` - Токены (например, для сброса пароля)
*   `AspNetRoleClaims` - "Утверждения" для ролей

Все эти таблицы связаны между собой через внешние ключи (foreign keys). Поэтому просто переименовать `AspNetUsers` недостаточно — нужно переименовать всю группу таблиц, чтобы сохранить консистентность.

---

### Инструкция по переименованию таблиц Identity

#### Шаг 1: Настройка DbContext

Откройте ваш файл контекста базы данных: `Backend/Models/TodoListDbContext.cs`. Вам нужно переопределить метод `OnModelCreating` и явно указать новые имена для каждой таблицы Identity.

Добавьте следующий код в класс `TodoListDbContext`:

```csharp
// Backend/Models/TodoListDbContext.cs

// ... using statements
using Microsoft.AspNetCore.Identity; // Убедитесь, что этот using есть

public partial class TodoListDbContext : IdentityDbContext<ApplicationUser>
{
    // ... ваш конструктор и DbSet'ы ...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ВАЖНО: Сначала вызываем базовую реализацию.
        // Это необходимо для того, чтобы Identity настроил свою схему.
        base.OnModelCreating(modelBuilder);

        // А теперь переопределяем имена таблиц
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable(name: "Users"); // Переименовываем AspNetUsers в Users
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Roles"); // AspNetRoles -> Roles
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles"); // AspNetUserRoles -> UserRoles
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims"); // AspNetUserClaims -> UserClaims
        });

        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins"); // AspNetUserLogins -> UserLogins
        });

        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims"); // AspNetRoleClaims -> RoleClaims
        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens"); // AspNetUserTokens -> UserTokens
        });
        
        // Здесь продолжается ваша существующая логика для других таблиц
        modelBuilder.Entity<Project>(entity =>
        {
            // ...
        });

        // ... и так далее для всех ваших моделей
    }

    // ...
}
```

#### Шаг 2: Создание новой миграции

Теперь, когда вы указали EF Core новые имена таблиц, нужно создать миграцию, которая применит эти изменения к базе данных.

1.  Откройте терминал в папке `Backend`.
2.  Выполните команду для создания миграции. Дайте ей понятное имя.

    ```bash
    dotnet ef migrations add CustomizeIdentityTableNames
    ```

3.  **Проверьте сгенерированный файл миграции.** Откройте новый файл в папке `Backend/Migrations`. Его метод `Up` должен содержать команды `RenameTable`, которые переименовывают таблицы, сохраняя все данные. Он будет выглядеть примерно так:

    ```csharp
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameTable(
            name: "AspNetUserTokens",
            newName: "UserTokens");

        migrationBuilder.RenameTable(
            name: "AspNetUsers",
            newName: "Users");
        
        // ... и так далее для всех остальных таблиц Identity
    }
    ```
    Это подтверждает, что EF Core правильно понял ваше намерение и не будет удалять и создавать таблицы заново.

#### Шаг 3: Применение миграции

Последний шаг — применить миграцию к вашей базе данных.

1.  Убедитесь, что Docker-контейнер с БД запущен.
2.  В терминале (в папке `Backend`) выполните:

    ```bash
    dotnet ef database update
    ```

Готово! После выполнения команды все таблицы Identity будут переименованы в вашей базе данных без потери данных.

### Очень важные моменты:

*   **Сделайте бэкап!** Перед выполнением таких серьёзных изменений в структуре рабочей или важной базы данных всегда делайте резервную копию.
*   **Когда лучше это делать?** Идеальный момент для переименования таблиц — самое начало проекта, до того, как в базе накопилось много данных. Если вы делаете это на существующем проекте, делайте это осторожно и тщательно протестируйте.
*   **Raw SQL запросы.** Если где-то в вашем коде есть "сырые" SQL-запросы (например, через `_context.Database.ExecuteSqlRawAsync()`), которые обращаются к старым именам таблиц (`AspNetUsers`), их нужно будет найти и обновить вручную.