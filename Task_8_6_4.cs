class Program
{
    static void Main(string[] args)
    {
        string binaryFilePath = "students.bin";

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string studentsDirectoryPath = Path.Combine(desktopPath, "Students");

        Directory.CreateDirectory(studentsDirectoryPath);

        Dictionary<string, List<Student>> studentsByGroup = new Dictionary<string, List<Student>>();

        using (BinaryReader reader = new BinaryReader(File.Open(binaryFilePath, FileMode.Open)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                string name = reader.ReadString();
                string group = reader.ReadString();
                long dateOfBirthBinary = reader.ReadInt64();
                DateTime dateOfBirth = DateTime.FromBinary(dateOfBirthBinary);
                decimal averageScore = reader.ReadDecimal();

                Student student = new Student
                {
                    Name = name,
                    Group = group,
                    DateOfBirth = dateOfBirth,
                    AverageScore = averageScore
                };

                if (!studentsByGroup.ContainsKey(group))
                {
                    studentsByGroup[group] = new List<Student>();
                }
                studentsByGroup[group].Add(student);
            }
        }

        foreach (var group in studentsByGroup)
        {
            string groupFilePath = Path.Combine(studentsDirectoryPath, $"{group.Key}.txt");
            using (StreamWriter writer = new StreamWriter(groupFilePath))
            {
                foreach (var student in group.Value)
                {
                    writer.WriteLine($"{student.Name}, {student.DateOfBirth.ToShortDateString()}, {student.AverageScore}");
                }
            }
        }

        Console.WriteLine("Данные успешно записаны в текстовые файлы.");
    }
}

class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal AverageScore { get; set; }
}
