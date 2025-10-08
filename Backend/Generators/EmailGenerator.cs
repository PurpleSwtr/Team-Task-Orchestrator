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
                return string.Empty;
            }

            try
            {
                string[] fileData = File.ReadAllLines(filePath);
                
                if (fileData.Length == 0)
                {
                    Console.WriteLine($"Warning: File is empty: {filePath}");
                    return string.Empty;
                }
                
                Random random = new Random();
                int randomIndex = random.Next(0, fileData.Length);
                return fileData[randomIndex];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return string.Empty;
            }
        }
        public string GetRandomUsername(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    long length = fileStream.Length;
                    if (length == 0)
                    {
                        Console.WriteLine("Файл с email'ами пуст.");
                        return string.Empty;
                    }

                    long position = (long)(_random.NextDouble() * length);

                    fileStream.Seek(position, SeekOrigin.Begin);

                    using (var streamReader = new StreamReader(fileStream))
                    {
 
                        streamReader.ReadLine();

                        string line = streamReader.ReadLine();


                        if (string.IsNullOrEmpty(line))
                        {
                            fileStream.Seek(0, SeekOrigin.Begin);
                            line = streamReader.ReadLine();
                        }

                        if (string.IsNullOrEmpty(line)) return "Не удалось прочитать строку";


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
                                return $"{email}@{domain}".ToLower();
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