using Microsoft.VisualBasic.FileIO;
using System;
using System.IO; // <-- Изменили using'и
using System.Text;

namespace Backend.Generators
{
    public class DataGeneratorEmail
    {
        private static readonly Random _random = new Random();
        public string GetRandomDomain()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "Generators", "Files", "Emails", "domains.md");
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Warning: File not found: {filePath}");
                return string.Empty; // или вернуть значение по умолчанию
            }

            try
            {
                string[] fileData = File.ReadAllLines(filePath);
                
                // Проверяем, что файл не пустой
                if (fileData.Length == 0)
                {
                    Console.WriteLine($"Warning: File is empty: {filePath}");
                    return string.Empty;
                }
                
                // Выбираем случайную строку
                Random random = new Random();
                int randomIndex = random.Next(0, fileData.Length);
                return fileData[randomIndex];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return string.Empty; // или выбросить исключение
            }
        }
        public string GetRandomUsername(string filePath)
        {
            try
            {
                // Открываем файл для чтения
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    long length = fileStream.Length;
                    if (length == 0)
                    {
                        Console.WriteLine("Файл с email'ами пуст.");
                        return string.Empty;
                    }

                    // 1. Выбираем случайную позицию в файле
                    long position = (long)(_random.NextDouble() * length);

                    // 2. Перемещаемся в эту позицию
                    fileStream.Seek(position, SeekOrigin.Begin);

                    // Используем StreamReader для удобного чтения строк
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        // 3. Первая строка, которую мы читаем, может быть неполной. Пропускаем ее.
                        // Если мы попали в самый конец файла, этот вызов вернет null.
                        streamReader.ReadLine();

                        // 4. Читаем следующую, уже гарантированно полную строку
                        string line = streamReader.ReadLine();

                        // Если мы оказались в самом конце файла и вторая строка пустая,
                        // просто вернемся в начало и прочитаем самую первую строку файла.
                        if (string.IsNullOrEmpty(line))
                        {
                            fileStream.Seek(0, SeekOrigin.Begin);
                            line = streamReader.ReadLine();
                        }

                        // Если после всех попыток строка все еще пуста, выходим
                        if (string.IsNullOrEmpty(line)) return "Не удалось прочитать строку";

                        // Используем TextFieldParser для корректной обработки CSV (например, кавычек)
                        // на одной единственной строке. Это быстро.
                        string domain = GetRandomDomain();

                        using (var stringReader = new StringReader(line))
                        using (var parser = new TextFieldParser(stringReader))
                        {
                            parser.SetDelimiters(",");
                            parser.HasFieldsEnclosedInQuotes = true;
                            string[] fields = parser.ReadFields();

                            if (fields != null && fields.Length > 0)
                            {
                                string email = fields[0];
                                Console.WriteLine($"{email}@{domain}");
                                return $"{email}@{domain}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }

            return "Ошибка";
        }
        
    }
}