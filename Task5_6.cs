using System;

class Program
{
    static void Main()
    {
        var userData = GetUserData();
        DisplayUserData(userData);
    }

    static (string FirstName, string LastName, int Age, bool HasPet, string[] PetNames, string[] FavoriteColors) GetUserData()
    {
        string firstName = GetInput("Введите ваше имя: ");
        string lastName = GetInput("Введите вашу фамилию: ");
        int age = GetValidIntInput("Введите ваш возраст: ");
        bool hasPet = GetYesNoInput("У вас есть питомец? (да/нет): ");
        string[]? petNames = null;

        if (hasPet)
        {
            int petCount = GetValidIntInput("Введите количество питомцев: ");
            petNames = GetPetNames(petCount);
        }

        int colorCount = GetValidIntInput("Введите количество любимых цветов: ");
        string[] favoriteColors = GetFavoriteColors(colorCount);

        return (firstName, lastName, age, hasPet, petNames, favoriteColors);
    }

    static string GetInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    static int GetValidIntInput(string prompt)
    {
        int result;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out result) && result > 0)
            {
                return result;
            }
            Console.WriteLine("Некорректный ввод. Пожалуйста, введите число больше 0.");
        }
    }

    static bool GetYesNoInput(string prompt)
    {
        while (true)
        {
            string input = GetInput(prompt).ToLower();
            if (input == "да")
            {
                return true;
            }
            else if (input == "нет")
            {
                return false;
            }
            Console.WriteLine("Некорректный ввод. Пожалуйста, введите 'да' или 'нет'.");
        }
    }

    static string[] GetPetNames(int count)
    {
        string[] names = new string[count];
        for (int i = 0; i < count; i++)
        {
            names[i] = GetInput($"Введите кличку питомца {i + 1}: ");
        }
        return names;
    }

    static string[] GetFavoriteColors(int count)
    {
        string[] colors = new string[count];
        for (int i = 0; i < count; i++)
        {
            colors[i] = GetInput($"Введите любимый цвет {i + 1}: ");
        }
        return colors;
    }

    static void DisplayUserData((string FirstName, string LastName, int Age, bool HasPet, string[] PetNames, string[] FavoriteColors) userData)
    {
        Console.WriteLine("\nДанные пользователя:");
        Console.WriteLine($"Имя: {userData.FirstName}");
        Console.WriteLine($"Фамилия: {userData.LastName}");
        Console.WriteLine($"Возраст: {userData.Age}");
        Console.WriteLine($"Наличие питомца: {(userData.HasPet ? "Да" : "Нет")}");

        if (userData.HasPet)
        {
            Console.WriteLine("Клички питомцев:");
            foreach (var name in userData.PetNames)
            {
                Console.WriteLine(name);
            }
        }

        Console.WriteLine("Любимые цвета:");
        foreach (var color in userData.FavoriteColors)
        {
            Console.WriteLine(color);
        }
    }
}
