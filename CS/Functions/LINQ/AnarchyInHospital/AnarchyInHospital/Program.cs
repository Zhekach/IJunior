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
    private const int SortByNameCommand = 1;
    private const int SortByAgeCommand = 2;
    private const int SortByDiseaseCommand = 3;
    
    private readonly PatientGenerator _patientGenerator;
    
    private ConsoleUI _consoleUi;
    private List<Patient> _patients;

    public Application()
    {
        _patientGenerator = new PatientGenerator();

        Initialize();
    }

    public void Run()
    {
        while (true)
        {
            _consoleUi.ShowMainMenu();
            
            int command = Utility.ReadInt();
            
            switch (command)
            {
                case SortByNameCommand:
                    SortPatientsByFullName();
                    break;
                case SortByAgeCommand:
                    SortPatientsByAge();
                    break;
                case SortByDiseaseCommand:
                    SortPatientsByDisease();
                    break;
                default:
                    Console.WriteLine("Неверный ввод, попробуйте ещё раз");
                    break;
            }
            
            Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
            Console.ReadLine();
        }
    }

    private void SortPatientsByFullName()
    {
        var result = _patients.OrderBy(patient => patient.FullName).ToList();
        
        _consoleUi.PrintPatientsInfo(result, "Отсортированные пациенты по ФИО");
    }

    private void SortPatientsByAge()
    {
        var result = _patients.OrderBy(patient => patient.Age).ToList();
        
        _consoleUi.PrintPatientsInfo(result, "Отсортированные пациенты по возрасту");
    }
    
    private void SortPatientsByDisease()
    {
        Console.WriteLine("Введите название заболевания"); 
        string disease = Console.ReadLine();
        
        var result = _patients.Where(patient => patient.Disease == disease).ToList();
        
        _consoleUi.PrintPatientsInfo(result, "Отсортированные пациенты по заболеванию");
    }
    
    private void Initialize()
    {
        _patients = _patientGenerator.GeneratePatients(10);
        
        _consoleUi = new ConsoleUI(_patients, new []
        {
            SortByNameCommand, SortByAgeCommand, SortByDiseaseCommand
        });
    }
}

internal class ConsoleUI
{
    private readonly List<Patient> _patients;
    private readonly int[] _commandsArray;

    public ConsoleUI(List<Patient> patients, int[] commandsArray)
    {
        _patients = patients;
        _commandsArray = commandsArray;
    }

    public void ShowMainMenu()
    {
        Console.Clear();
        PrintPatientsInfo(_patients, "Все пациенты в больнице");

        Console.WriteLine("Выберите действие:\n" +
                          $"{_commandsArray[0]} - Отсортировать всех больных по ФИО\n" +
                          $"{_commandsArray[1]} - Отсортировать всех больных по возрасту\n" +
                          $"{_commandsArray[2]} - Вывести больных с определенным заболеванием\n");
    }

    public void PrintPatientsInfo(List<Patient> patients, string description)
    {
        Console.Clear();
        Console.WriteLine(description);

        foreach (var patient in patients)
        {
            patient.PrintInfo();
        }
    }
}

internal class Patient
{
    public Patient(string fullName, int age, string disease)
    {
        FullName = fullName;
        Age = age;
        Disease = disease;
    }

    public string FullName { get; private set; }
    public int Age { get; private set; }
    public string Disease { get; private set; }

    public void PrintInfo()
    {
        Console.WriteLine($"ФИО - {FullName}, диагноз - {Disease}, " +
                          $"возраст - {Age}");
    }
}

internal class PatientGenerator
{
    private readonly List<string> _fullNames = new List<string>()
    {
        "Иван Петрович Иванов",
        "Алексей Сергеевич Смирнов",
        "Сергей Иванович Козлов",
        "Дмитрий Александрович Попов",
        "Николай Владимирович Соколов"
    };

    private readonly List<string> _disiases = new List<string>()
    {
        "Грипп", "Пневмония", "Диабет", "Гипертония", "Астма"
    };

    private readonly int _minAge = 18;
    private readonly int _maxAge = 70;

    public List<Patient> GeneratePatients(int count)
    {
        var patients = new List<Patient>();

        for (int i = 0; i < count; i++)
        {
            string name = _fullNames[Utility.GetRandomInt(_fullNames.Count)];
            string disiase = _disiases[Utility.GetRandomInt(_disiases.Count)];
            int age = Utility.GetRandomInt(_minAge, _maxAge + 1);

            patients.Add(new Patient(name, age, disiase));
        }

        return patients;
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