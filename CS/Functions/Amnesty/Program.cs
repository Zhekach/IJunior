internal class Program
{
    private static void Main()
    {
        Application application = new Application();
        application.Run();
    }
}

internal class Application
{
    private readonly CriminalListEditor _criminalListEditor;
    private readonly CriminalGenerator _criminalGenerator;

    private List<Criminal> _criminals;

    public Application()
    {
        _criminalListEditor = new CriminalListEditor();
        _criminalGenerator = new CriminalGenerator();
        Initialize();
    }
    
    public void Run()
    {
        PrintCriminalsInfo(_criminals, "Данные о преступниках до амнистии:");
        
        _criminalListEditor.RemoveByCrimeType(_criminals, "Антиправительственное");

        PrintCriminalsInfo(_criminals, "Данные о преступниках после амнистии:");
    }

    private void PrintCriminalsInfo(List<Criminal> criminals, string description)
    {
        Console.WriteLine(description);
        
        foreach (var criminal in criminals)
        {
            criminal.PrintInfo();
        }
    }

    private void Initialize()
    {
        _criminals = _criminalGenerator.GenerateCriminals(10);
    }
}

internal class CriminalListEditor
{
    public List<Criminal> RemoveByCrimeType(List<Criminal> criminals, string crimeType)
    {
        var criminalsToRemove = criminals.Where(c => c.CrimeType == crimeType).ToList();
        
        criminals = criminals.Except(criminalsToRemove).ToList();
        
        //criminals.RemoveAll(c => c.CrimeType == crimeType);
    }
}

internal class Criminal
{
    public Criminal(string fullName, string crimeType)
    {
        FullName = fullName;
        CrimeType = crimeType;
    }
    
    public string FullName { get; private set; }
    public string CrimeType { get; private set; }

    public void PrintInfo()
    {
        Console.WriteLine($"ФИО - {FullName}, Тип преступления - {CrimeType}, ");
    }
}

internal class CriminalGenerator
{
    private readonly List<string> _fullNames = new List<string>()
    {
        "Иван Петрович Иванов",
        "Алексей Сергеевич Смирнов",
        "Сергей Иванович Козлов",
        "Дмитрий Александрович Попов",
        "Николай Владимирович Соколов"
    };

    private readonly List<string> _crimeTypes = new List<string>()
    {
        "Антиправительственное", "Финансовое", "Правовое", "Коррупционное", "Международное"
    };

    public List<Criminal> GenerateCriminals(int count)
    {
        var criminals = new List<Criminal>();

        for (int i = 0; i < count; i++)
        {
            string name = _fullNames[Utility.GetRandomInt(_fullNames.Count)];
            string crimeType = _crimeTypes[Utility.GetRandomInt(_crimeTypes.Count)];

            criminals.Add(new Criminal(name, crimeType));
        }

        return criminals;
    }
}

internal static class Utility
{
    private static readonly Random s_random = new Random();

    public static int GetRandomInt(int maxValue)
    {
        return s_random.Next(maxValue);
    }

    public static int GetRandomInt(int minValue, int maxValue)
    {
        return s_random.Next(minValue, maxValue);
    }

    public static int ReadInt()
    {
        bool isIntEntered = false;
        int parsedInt = 0;

        while (isIntEntered == false)
        {
            string? enteredString;

            Console.WriteLine("Введите целое число:");
            enteredString = Console.ReadLine();

            isIntEntered = int.TryParse(enteredString, out parsedInt);

            if (isIntEntered)
            {
                Console.WriteLine($"Введенное число распознанно, это: {parsedInt}");
            }
            else
            {
                Console.WriteLine("Вы ввели неверно, попробуйте ещё раз)\n");
            }
        }

        return parsedInt;
    }

    public static T GetRandomEnumValue<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        int index = GetRandomInt(values.Length);
        var result = (T)values.GetValue(index)!;

        return result;
    }
}