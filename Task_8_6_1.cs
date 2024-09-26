class Program
{
    static void Main()
    {
        Console.WriteLine("Введите путь до папки:");
        string path = Console.ReadLine();

        try
        {
            if (Directory.Exists(path))
            {
                CleanDirectory(path);
                Console.WriteLine("Очистка завершена.");
            }
            else
            {
                Console.WriteLine("Папка по указанному пути не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    static void CleanDirectory(string path)
    {
        TimeSpan timeSpan = TimeSpan.FromMinutes(30);
        DateTime currentTime = DateTime.Now;

        string[] files = Directory.GetFiles(path);
        string[] directories = Directory.GetDirectories(path);

        foreach (string file in files)
        {
            FileInfo fileInfo = new(file);
            if ((currentTime - fileInfo.LastAccessTime) > timeSpan)
            {
                File.Delete(file);
                Console.WriteLine($"Удален файл: {file}");
            }
        }

        foreach (string directory in directories)
        {
            DirectoryInfo directoryInfo = new(directory);
            if ((currentTime - directoryInfo.LastAccessTime) > timeSpan)
            {
                CleanDirectory(directory);
                Directory.Delete(directory, true);
                Console.WriteLine($"Удалена папка: {directory}");
            }
            else
            {
                CleanDirectory(directory);
            }
        }
    }
}
