using System.Collections;

namespace Lab9_10CharpT;

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Головне меню:");
            Console.WriteLine("1. Завдання 1 (Обчислення формули через Stack)");
            Console.WriteLine("2. Завдання 2 (Сортування студентів через Queue)");
            Console.WriteLine("3. Завдання 3 (Формула через ArrayList)");
            Console.WriteLine("4. Завдання 4 (Студенти через ArrayList та Інтерфейси)");
            Console.WriteLine("5. Завдання 5 (Каталог музичних дисків через Hashtable)");
            Console.WriteLine("0. Вихід");
            Console.Write("Оберіть опцію: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await new Lab9Task1().RunAsync();
                    break;
                case "2":
                    await new Lab9Task2().RunAsync();
                    break;
                case "3":
                    await Lab9Task3.RunAsync();
                    break;
                case "4":
                    await new Lab9Task4().RunAsync();
                    break;
                case "5":
                    await new Lab9Task5().RunAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }

            if (choice != "5")
            {
                Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }
    }
}

class Lab9Task1
{
    public async Task RunAsync()
    {
        string filePath = "task1_formula.txt";

        if (!File.Exists(filePath))
        {
            await File.WriteAllTextAsync(filePath, "m(9, p(p(3, 5), m(3, 8)))");
        }

        string formula = await File.ReadAllTextAsync(filePath);
        Console.WriteLine($"Зчитана формула з файлу: {formula}");

        try
        {
            int result = EvaluateFormula(formula);
            Console.WriteLine($"Результат обчислення: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка при обчисленні: {ex.Message}");
        }
    }

    private int EvaluateFormula(string formula)
    {
        Stack<int> stack = new Stack<int>();

        for (int i = formula.Length - 1; i >= 0; i--)
        {
            char c = formula[i];

            if (char.IsDigit(c))
            {
                stack.Push(c - '0');
            }
            else if (c == 'm' || c == 'p')
            {
                int a = stack.Pop();
                int b = stack.Pop();

                if (c == 'm')
                {
                    int res = (a - b) % 10;
                    if (res < 0)
                    {
                        res += 10;
                    }
                    stack.Push(res);
                }
                else if (c == 'p')
                {
                    stack.Push((a + b) % 10);
                }
            }
        }

        return stack.Pop();
    }
}

class Lab9Task2
{
    public async Task RunAsync()
    {
        string filePath = "task2_students.txt";

        if (!File.Exists(filePath))
        {
            string[] defaultData = {
                "Шевченко, Тарас, Григорович, ІПЗ-1, 4, 5, 4",
                "Франко, Іван, Якович, ІПЗ-1, 2, 3, 4",
                "Українка, Леся, Петрівна, ІПЗ-2, 5, 5, 5",
                "Коцюбинський, Михайло, Михайлович, ІПЗ-2, 3, 2, 3"
            };
            await File.WriteAllLinesAsync(filePath, defaultData);
        }

        Queue<string> unsuccessfulStudents = new Queue<string>();

        Console.WriteLine("Успішні студенти:");

        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            while ((line = await sr.ReadLineAsync()) != null)
            {
                string[] parts = line.Split(',');

                if (parts.Length >= 7)
                {
                    _ = int.TryParse(parts[4].Trim(), out int g1);
                    _ = int.TryParse(parts[5].Trim(), out int g2);
                    _ = int.TryParse(parts[6].Trim(), out int g3);

                    if (g1 >= 3 && g2 >= 3 && g3 >= 3)
                    {
                        Console.WriteLine(line);
                    }
                    else
                    {
                        unsuccessfulStudents.Enqueue(line);
                    }
                }
            }
        }

        Console.WriteLine("\nІнші студенти (боржники):");
        while (unsuccessfulStudents.Count > 0)
        {
            Console.WriteLine(unsuccessfulStudents.Dequeue());
        }
    }
}

class Lab9Task3
{
    public static async Task RunAsync()
    {
        string filePath = "task3_formula.txt";

        if (!File.Exists(filePath))
        {
            await File.WriteAllTextAsync(filePath, "m(9, p(p(3, 5), m(3, 8)))");
        }

        string formula = await File.ReadAllTextAsync(filePath);
        Console.WriteLine($"Зчитана формула з файлу: {formula}");

        try
        {
            int result = EvaluateFormula(formula);
            Console.WriteLine($"Результат обчислення (через ArrayList): {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка при обчисленні: {ex.Message}");
        }
    }

    private static int EvaluateFormula(string formula)
    {
        ArrayList stack = new ArrayList();

        for (int i = formula.Length - 1; i >= 0; i--)
        {
            char c = formula[i];

            if (char.IsDigit(c))
            {
                stack.Add(c - '0');
            }
            else if (c == 'm' || c == 'p')
            {
                int a = (int)stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);

                int b = (int)stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);

                if (c == 'm')
                {
                    int res = (a - b) % 10;
                    if (res < 0)
                    {
                        res += 10;
                    }
                    stack.Add(res);
                }
                else if (c == 'p')
                {
                    stack.Add((a + b) % 10);
                }
            }
        }

        return (int)stack[stack.Count - 1];
    }
}

class Student : ICloneable
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string Group { get; set; }
    public int[] Grades { get; set; }
    public int OriginalIndex { get; set; }

    public bool IsSuccessful => Grades[0] >= 3 && Grades[1] >= 3 && Grades[2] >= 3;

    public object Clone()
    {
        return new Student
        {
            LastName = this.LastName,
            FirstName = this.FirstName,
            MiddleName = this.MiddleName,
            Group = this.Group,
            Grades = (int[])this.Grades.Clone(),
            OriginalIndex = this.OriginalIndex
        };
    }

    public override string ToString()
    {
        return $"{LastName} {FirstName} {MiddleName}, {Group}, Оцінки: {Grades[0]}, {Grades[1]}, {Grades[2]}";
    }
}

class StudentComparer : IComparer
{
    public int Compare(object x, object y)
    {
        Student s1 = (Student)x;
        Student s2 = (Student)y;

        if (s1.IsSuccessful && !s2.IsSuccessful) return -1;
        if (!s1.IsSuccessful && s2.IsSuccessful) return 1;

        return s1.OriginalIndex.CompareTo(s2.OriginalIndex);
    }
}

class StudentCollection : IEnumerable
{
    private ArrayList _list = new ArrayList();

    public void Add(Student s)
    {
        _list.Add(s);
    }

    public void Sort()
    {
        _list.Sort(new StudentComparer());
    }

    public IEnumerator GetEnumerator()
    {
        return _list.GetEnumerator();
    }
}

class Lab9Task4
{
    public async Task RunAsync()
    {
        string filePath = "task4_students.txt";

        if (!File.Exists(filePath))
        {
            string[] defaultData = {
                "Шевченко, Тарас, Григорович, ІПЗ-1, 4, 5, 4",
                "Франко, Іван, Якович, ІПЗ-1, 2, 3, 4",
                "Українка, Леся, Петрівна, ІПЗ-2, 5, 5, 5",
                "Коцюбинський, Михайло, Михайлович, ІПЗ-2, 3, 2, 3"
            };
            await File.WriteAllLinesAsync(filePath, defaultData);
        }

        StudentCollection students = new StudentCollection();
        int index = 0;

        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            while ((line = await sr.ReadLineAsync()) != null)
            {
                string[] parts = line.Split(',');

                if (parts.Length >= 7)
                {
                    _ = int.TryParse(parts[4].Trim(), out int g1);
                    _ = int.TryParse(parts[5].Trim(), out int g2);
                    _ = int.TryParse(parts[6].Trim(), out int g3);
                        
                    Student student = new Student
                    {
                        LastName = parts[0].Trim(),
                        FirstName = parts[1].Trim(),
                        MiddleName = parts[2].Trim(),
                        Group = parts[3].Trim(),
                        Grades = new int[] { g1, g2, g3 },
                        OriginalIndex = index++
                    };

                    students.Add(student);
                }
            }
        }

        students.Sort();

        Console.WriteLine("Відсортований список студентів (через ArrayList, IComparer та IEnumerable):");
        foreach (Student s in students)
        {
            Console.WriteLine(s);
        }

        Console.WriteLine("\nДемонстрація ICloneable (клонування першого студента):");
        IEnumerator enumerator = students.GetEnumerator();
        if (enumerator.MoveNext())
        {
            Student firstStudent = (Student)enumerator.Current;
            Student clonedStudent = (Student)firstStudent.Clone();
            clonedStudent.FirstName = "КЛОН";
            Console.WriteLine($"Оригінал: {firstStudent}");
            Console.WriteLine($"Клон:   {clonedStudent}");
        }
    }


}

class Song
{
    public string Title { get; set; }
    public string Artist { get; set; }

    public Song(string title, string artist)
    {
        Title = title;
        Artist = artist;
    }

    public override string ToString()
    {
        return $"'{Title}' - {Artist}";
    }
}

class MusicDisk
{
    public string DiskName { get; set; }
    public ArrayList Songs { get; set; }

    public MusicDisk(string name)
    {
        DiskName = name;
        Songs = new ArrayList();
    }

    public void AddSong(Song song)
    {
        Songs.Add(song);
    }

    public void RemoveSong(string songTitle)
    {
        for (int i = 0; i < Songs.Count; i++)
        {
            if (((Song)Songs[i]).Title == songTitle)
            {
                Songs.RemoveAt(i);
                break;
            }
        }
    }

    public void PrintContent()
    {
        Console.WriteLine($"\nДиск: [{DiskName}]");
        if (Songs.Count == 0)
        {
            Console.WriteLine("  (порожній)");
            return;
        }

        foreach (Song s in Songs)
        {
            Console.WriteLine($"  - {s}");
        }
    }
}

class Lab9Task5
{
    private Hashtable catalog = new Hashtable();

    public async Task RunAsync()
    {
        await Task.Run(() => InitializeData());

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Каталог компакт-дисків ===");
            Console.WriteLine("1. Додати диск");
            Console.WriteLine("2. Видалити диск");
            Console.WriteLine("3. Додати пісню на диск");
            Console.WriteLine("4. Видалити пісню з диска");
            Console.WriteLine("5. Переглянути весь каталог");
            Console.WriteLine("6. Переглянути конкретний диск");
            Console.WriteLine("7. Пошук за виконавцем");
            Console.WriteLine("0. Повернутися до головного меню");
            Console.Write("Оберіть дію: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": 
                    AddDisk(); 
                    break;
                case "2": 
                    RemoveDisk(); 
                    break;
                case "3": 
                    AddSongToDisk(); 
                    break;
                case "4": 
                    RemoveSongFromDisk();
                    break;
                case "5": 
                    PrintCatalog(); 
                    break;
                case "6": 
                    PrintSpecificDisk(); 
                    break;
                case "7": 
                    SearchByArtist(); 
                    break;
                case "0": 
                    return;
            }

            Console.WriteLine("\nНатисніть клавішу...");
            Console.ReadKey();
        }
    }

    private void InitializeData()
    {
        MusicDisk disk1 = new MusicDisk("Rock Hits");
        disk1.AddSong(new Song("Bohemian Rhapsody", "Queen"));
        disk1.AddSong(new Song("Hotel California", "Eagles"));

        MusicDisk disk2 = new MusicDisk("Pop 2000s");
        disk2.AddSong(new Song("Toxic", "Britney Spears"));
        disk2.AddSong(new Song("Oops!... I Did It Again", "Britney Spears"));

        catalog.Add(disk1.DiskName, disk1);
        catalog.Add(disk2.DiskName, disk2);
    }

    private void AddDisk()
    {
        Console.Write("Введіть назву нового диска: ");
        string name = Console.ReadLine();
        if (!catalog.ContainsKey(name))
        {
            catalog.Add(name, new MusicDisk(name));
            Console.WriteLine("Диск додано!");
        }
        else
        {
            Console.WriteLine("Диск з такою назвою вже існує.");
        }
    }

    private void RemoveDisk()
    {
        Console.Write("Введіть назву диска для видалення: ");
        string name = Console.ReadLine();
        if (catalog.ContainsKey(name))
        {
            catalog.Remove(name);
            Console.WriteLine("Диск видалено!");
        }
        else
        {
            Console.WriteLine("Диск не знайдено.");
        }
    }

    private void AddSongToDisk()
    {
        Console.Write("Введіть назву диска: ");
        string diskName = Console.ReadLine();

        if (catalog.ContainsKey(diskName))
        {
            Console.Write("Введіть назву пісні: ");
            string title = Console.ReadLine();
            Console.Write("Введіть виконавця: ");
            string artist = Console.ReadLine();

            MusicDisk disk = (MusicDisk)catalog[diskName];
            disk.AddSong(new Song(title, artist));
            Console.WriteLine("Пісню додано!");
        }
        else
        {
            Console.WriteLine("Диск не знайдено.");
        }
    }

    private void RemoveSongFromDisk()
    {
        Console.Write("Введіть назву диска: ");
        string diskName = Console.ReadLine();

        if (catalog.ContainsKey(diskName))
        {
            Console.Write("Введіть назву пісні для видалення: ");
            string title = Console.ReadLine();

            MusicDisk disk = (MusicDisk)catalog[diskName];
            disk.RemoveSong(title);
            Console.WriteLine("Пісню видалено (якщо вона там була).");
        }
        else
        {
            Console.WriteLine("Диск не знайдено.");
        }
    }

    private void PrintCatalog()
    {
        Console.WriteLine("\nВміст всього каталогу:");
        if (catalog.Count == 0)
        {
            Console.WriteLine("Каталог порожній.");
            return;
        }

        foreach (DictionaryEntry entry in catalog)
        {
            ((MusicDisk)entry.Value).PrintContent();
        }
    }

    private void PrintSpecificDisk()
    {
        Console.Write("Введіть назву диска: ");
        string name = Console.ReadLine();
        if (catalog.ContainsKey(name))
        {
            ((MusicDisk)catalog[name]).PrintContent();
        }
        else
        {
            Console.WriteLine("Диск не знайдено.");
        }
    }

    private void SearchByArtist()
    {
        Console.Write("Введіть ім'я виконавця для пошуку: ");
        string artist = Console.ReadLine();
        bool found = false;

        Console.WriteLine($"\nРезультати пошуку для '{artist}':");
        foreach (DictionaryEntry entry in catalog)
        {
            MusicDisk disk = (MusicDisk)entry.Value;
            foreach (Song s in disk.Songs)
            {
                if (s.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Знайдено на диску [{disk.DiskName}]: {s}");
                    found = true;
                }
            }
        }

        if (!found)
        {
            Console.WriteLine("Записів не знайдено.");
        }
    }
}
