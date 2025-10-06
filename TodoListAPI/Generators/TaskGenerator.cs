using TodoListAPI.Models;

using TaskEntity = TodoListAPI.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace TodoListAPI.Generators
{
    public class DataGeneratorTask
    {
        private string[]? _fileData;

        private static readonly Random _random = new Random();

        public void ReadData()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "Generators", "Files", "Tasks.md");

            _fileData = File.ReadAllLines(filePath);

        }
        public string GetTask()
        {
            if (_fileData == null)
            {
                ReadData();
            }

            int taskIndex = _random.Next(_fileData!.Length);
            return _fileData[taskIndex];
        }

        public async Task Generate(TodoListDbContext context, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var newTask = new TaskEntity
                {
                    TaskName = GetTask(),
                    Description = "Сгенерировано автоматически",
                    CreatedAt = DateTime.UtcNow,
                };
                context.Tasks.Add(newTask);
            }
            await context.SaveChangesAsync();
        }

    }
}