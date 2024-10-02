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
                long sizeBefore = GetDirectorySize(path);
                Console.WriteLine($"Размер папки до очистки: {sizeBefore} байт.");

                int filesDeleted, spaceFreed;
                CleanDirectory(path, out filesDeleted, out spaceFreed);

                long sizeAfter = GetDirectorySize(path);
                Console.WriteLine($"Размер папки после очистки: {sizeAfter} байт.");

                Console.WriteLine($"Удалено файлов: {filesDeleted}");
                Console.WriteLine($"Освобождено места: {spaceFreed} байт.");
            }
            else
            {
                Console.WriteLine("Папка по указанному пути не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void CleanDirectory(string path, out int filesDeleted, out int spaceFreed)
    {
        TimeSpan timeSpan = TimeSpan.FromMinutes(30);
        DateTime currentTime = DateTime.Now;
        filesDeleted = 0;
        spaceFreed = 0;

        string[] files = Directory.GetFiles(path);
        string[] directories = Directory.GetDirectories(path);

        foreach (string file in files)
        {
            FileInfo fileInfo = new(file);
            if ((currentTime - fileInfo.LastAccessTime) > timeSpan)
            {
                spaceFreed += (int)fileInfo.Length;
                File.Delete(file);
                filesDeleted++;
                Console.WriteLine($"Удален файл: {file}");
            }
        }

        foreach (string directory in directories)
        {
            DirectoryInfo directoryInfo = new(directory);
            if ((currentTime - directoryInfo.LastAccessTime) > timeSpan)
            {
                int subFilesDeleted, subSpaceFreed;
                CleanDirectory(directory, out subFilesDeleted, out subSpaceFreed);
                filesDeleted += subFilesDeleted;
                spaceFreed += subSpaceFreed;
                Directory.Delete(directory, true);
                Console.WriteLine($"Удалена папка: {directory}");
            }
            else
            {
                int subFilesDeleted, subSpaceFreed;
                CleanDirectory(directory, out subFilesDeleted, out subSpaceFreed);
                filesDeleted += subFilesDeleted;
                spaceFreed += subSpaceFreed;
            }
        }
    }

    static long GetDirectorySize(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException("Указанный путь не существует или не является директорией.");
        }

        long totalSize = 0;

        try
        {
            foreach (string file in Directory.GetFiles(directoryPath))
            {
                FileInfo fileInfo = new(file);
                totalSize += fileInfo.Length;
            }

            foreach (string dir in Directory.GetDirectories(directoryPath))
            {
                totalSize += GetDirectorySize(dir);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при вычислении размера папки: {ex.Message}");
        }

        return totalSize;
    }
}
