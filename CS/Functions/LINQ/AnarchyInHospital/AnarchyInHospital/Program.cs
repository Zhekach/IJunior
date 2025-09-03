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
    private readonly PatientSorter _patientSorter;
    private readonly PatientGenerator _patientGenerator;

    private List<Patient> _patients;

    public Application()
    {
        _patientSorter = new PatientSorter();
        _patientGenerator = new PatientGenerator();
        Initialize();
    }
    
    public void Run()
    {
        PrintPatientsInfo(_patients, "Данные обо всех пациентах:");

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

    private void PrintPatientsInfo(List<Patient> patients, string description)
    {
        Console.WriteLine(description);
        
        foreach (var patient in patients)
        {
            patient.PrintInfo();
        }
    }

    private void Initialize()
    {
        _patients = _patientGenerator.GeneratePatients(5);
    }
}

internal class ConsoleUI
{
    private 
    
    public void ShowMainMenu()
}

internal class PatientSorter
{
    public List<Patient> SortByFullName(List<Patient> patients)
    {
        var result = patients.OrderBy(p => p.FullName).ToList();
        
        return result;
    }
    
    public List<Patient> SortByAge(List<Patient> patients)
    {
        var result = patients.OrderBy(p => p.Age).ToList();
        
        return result;
    }

    public List<Patient> FindByDisease(List<Patient> patients, string disease)
    {
        var result = patients.Where(p => p.Disease == disease).ToList();
        
        return result;
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