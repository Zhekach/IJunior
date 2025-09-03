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
    private readonly CriminalFinder _criminalFinder;
    private readonly CriminalGenerator _criminalGenerator;

    private List<Criminal> _criminals;

    public Application()
    {
        _criminalFinder = new CriminalFinder();
        _criminalGenerator = new CriminalGenerator();
        Initialize();
    }
    
    public void Run()
    {
        PrintCriminalsInfo(_criminals, "Данные об известных преступниках:");
        
        Console.WriteLine("Введите данные для поиска:");

        int height = HandleIntInput("Рост:");
        int weight = HandleIntInput("Вес:");
        string nationality = HandleStringInput("Национальность:");
        
        List<Criminal> selectCriminals = _criminalFinder.SelectCriminals(_criminals, height, weight, nationality);

        PrintCriminalsInfo(selectCriminals, "Найденные преступники:");
    }
    
    private int HandleIntInput(string description)
    {
        Console.WriteLine(description);
        int result = Utility.ReadInt();

        return result;
    }

    private string HandleStringInput(string description)
    {
        Console.WriteLine(description);
        string result = Console.ReadLine();
        
        return result;
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
        _criminals = _criminalGenerator.GenerateCriminals(5);
    }
}

internal class CriminalFinder
{
    public List<Criminal> SelectCriminals(List<Criminal> criminals, int height, int weight, string nationality)
    {
        var selectedCriminals = from criminal in criminals
            where criminal.IsArrested == false && 
                  criminal.Height == height && 
                  criminal.Weight == weight && 
                  criminal.Nationality == nationality
            select criminal;
        
        var result = selectedCriminals.ToList();
        
        return result;
    }
}

internal class Criminal
{
    public Criminal(string fullName, bool isArrested, int height, int weight, string nationality)
    {
        FullName = fullName;
        IsArrested = isArrested;
        Height = height;
        Weight = weight;
        Nationality = nationality;
    }
    
    public string FullName { get; private set; }
    public bool IsArrested { get; private set; }
    public int Height { get; private set; }
    public int Weight { get; private set; }
    public string Nationality { get; private set; }

    public void PrintInfo()
    {
        Console.WriteLine($"ФИО - {FullName}, национальность - {Nationality}, " +
                          $"рост - {Height}, вес, - {Weight}, арестован - {IsArrested}");
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

    private readonly List<string> _nationalities = new List<string>() { "Русский", "Украинец", "Казах" };

    private readonly int _minHeight = 150;
    private readonly int _maxHeight = 200;
    private readonly int _minWeight = 50;
    private readonly int _maxWeight = 120;

    public List<Criminal> GenerateCriminals(int count)
    {
        var criminals = new List<Criminal>();

        for (int i = 0; i < count; i++)
        {
            string name = _fullNames[Utility.GetRandomInt(_fullNames.Count)];
            string nationality = _nationalities[Utility.GetRandomInt(_nationalities.Count)];

            int height = Utility.GetRandomInt(_minHeight, _maxHeight + 1); 
            int weight = Utility.GetRandomInt(_minWeight, _maxWeight + 1);

            bool isArrested = Utility.GetRandomInt(2) == 0 ? false : true;

            criminals.Add(new Criminal(name, isArrested, height, weight, nationality));
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