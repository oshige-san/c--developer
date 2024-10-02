
class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Введите путь к директории: ");
            string directoryPath = Console.ReadLine()!;

            long size = GetDirectorySize(directoryPath);
            Console.WriteLine($"Размер папки '{directoryPath}' составляет {size} байт.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
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
                FileInfo fileInfo = new (file);
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
