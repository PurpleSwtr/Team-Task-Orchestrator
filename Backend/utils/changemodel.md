Конечно! Вот инструкция, как безболезненно работать с миграциями в вашем проекте, включая добавление новых полей и переименование существующих.

### Обзор процесса работы с миграциями в Entity Framework Core

В вашем проекте используется Entity Framework Core, который применяет подход "Code-First". Это означает, что структура вашей базы данных определяется C# классами в папке `Backend/Models`. Любые изменения в этих моделях (добавление, удаление или изменение полей) должны сопровождаться созданием и применением "миграции" — специального файла, который описывает, как обновить схему базы данных, чтобы она соответствовала вашим моделям.

---

### Инструкция: Как добавить новое поле в модель

Давайте в качестве примера добавим поле `ShortName` (сокращённое имя) для пользователя, как у вас указано в `TODO.md`.

#### Шаг 1: Изменение модели

Откройте файл модели, который вы хотите изменить. В нашем случае это `Backend/Models/ApplicationUser.cs`. Добавьте в класс новое свойство:

```csharp
// Backend/Models/ApplicationUser.cs

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? PatronymicName { get; set; }
        
        // 👇 Новое добавленное поле
        public string? ShortName { get; set; } 

        public DateTime? RegistrationTime { get; set; }
        public string? Notes { get; set; }
        public int? IdUserStatus { get; set; }
        public virtual Status? IdUserStatusNavigation { get; set; }
        public virtual ICollection<TasksUser> TasksUsers { get; set; } = new List<TasksUser>();
        public virtual ICollection<UsersCommand> UsersCommands { get; set; } = new List<UsersCommand>();
    }
}
```

#### Шаг 2: Создание миграции

Теперь нужно создать файл миграции, который добавит соответствующую колонку в таблицу `AspNetUsers`.

1.  **Откройте терминал** в корневой папке вашего проекта.
2.  Перейдите в директорию бэкенда:
    ```bash
    cd Backend
    ```
3.  Выполните команду для создания миграции. Дайте ей осмысленное имя, например, `AddShortNameToUser`.

    ```bash
    dotnet ef migrations add AddShortNameToUser
    ```

После выполнения этой команды в папке `Backend/Migrations` появится новый файл с именем вроде `20251008xxxxxx_AddShortNameToUser.cs`. Его содержимое будет выглядеть примерно так:

```csharp
// ...
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<string>(
        name: "ShortName",
        table: "AspNetUsers",
        type: "nvarchar(max)",
        nullable: true);
}
// ...
```
Этот код говорит Entity Framework добавить колонку `ShortName` в таблицу `AspNetUsers`.

#### Шаг 3: Применение миграции к базе данных

Последний шаг — применить изменения к базе данных.

1.  Убедитесь, что ваш Docker-контейнер с базой данных запущен. Вы можете запустить его командой из корневой директории:
    ```bash
    docker-compose up -d db
    ```2.  В терминале, находясь в папке `Backend`, выполните команду:
    ```bash
    dotnet ef database update
    ```

Готово! Ваша база данных обновлена, и в таблице `AspNetUsers` теперь есть колонка `ShortName`.

---

### Инструкция: Как переименовать существующее поле

Переименование — более деликатный процесс, так как по умолчанию EF может удалить старую колонку (вместе со всеми данными) и создать новую. Чтобы избежать потери данных, нужно немного отредактировать файл миграции.

Давайте для примера переименуем поле `PatronymicName` в `LastName` в той же модели `ApplicationUser`.

#### Шаг 1: Изменение модели

Переименуйте свойство в файле `Backend/Models/ApplicationUser.cs`.

```csharp
// Backend/Models/ApplicationUser.cs
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    
    // 👇 Поле переименовано с PatronymicName на LastName
    public string? LastName { get; set; } 

    public string? ShortName { get; set; } 
    // ... остальной код
}
```

#### Шаг 2: Создание миграции

Как и в прошлый раз, создайте миграцию из папки `Backend`.

```bash
dotnet ef migrations add RenamePatronymicToLastNameOnUser
```

#### Шаг 3: **(Важно!)** Редактирование файла миграции

Откройте сгенерированный файл миграции (`..._RenamePatronymicToLastNameOnUser.cs`). Вы увидите, что EF сгенерировал код для удаления старой колонки и добавления новой:

```csharp
// Неправильный вариант, который приведёт к потере данных!
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropColumn(
        name: "PatronymicName",
        table: "AspNetUsers");

    migrationBuilder.AddColumn<string>(
        name: "LastName",
        table: "AspNetUsers",
        type: "nvarchar(max)",
        nullable: true);
}
```

Это приведёт к потере всех данных в этой колонке. **Замените** содержимое метода `Up` на специальную команду `RenameColumn`:

```csharp
// Правильный вариант для переименования
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.RenameColumn(
        name: "PatronymicName",
        table: "AspNetUsers",
        newName: "LastName");
}
```
Таким же образом нужно исправить метод `Down`, который отвечает за откат миграции:
```csharp
// Правильный откат
protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.RenameColumn(
        name: "LastName",
        table: "AspNetUsers",
        newName: "PatronymicName");
}
```
Этот подход гарантирует, что колонка будет просто переименована в базе данных без потери информации.

#### Шаг 4: Применение миграции

Теперь, когда миграция исправлена, примените её к базе данных:

```bash
dotnet ef database update
```

#### Шаг 5: Обновление кода

Не забудьте найти все места в вашем коде (например, в контроллерах, сервисах, DTO), где использовалось старое имя `PatronymicName`, и заменить его на `LastName`.

### Общие рекомендации

*   **Одна миграция — одно логическое изменение.** Старайтесь не смешивать в одной миграции добавление полей, их переименование и удаление. Это упростит отладку, если что-то пойдет не так.
*   **Всегда проверяйте сгенерированные миграции.** Особенно при переименовании или изменении типа данных. Автоматически сгенерированный код не всегда идеален.
*   **Работа в команде.** Перед тем как применять миграции, убедитесь, что все члены команды получили последние изменения из системы контроля версий (Git), чтобы избежать конфликтов.