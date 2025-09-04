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
    private List<Soldier> _soldiersFirstGroup = new List<Soldier>();
    private List<Soldier> _soldiersSecondGroup = new List<Soldier>();

    public Application()
    {
        GenerateSoldiers();
    }
    
    public void Run()
    {
        PrintSoldiersInfo(_soldiersFirstGroup,"Изначально солдаты в первом отряде:");
        PrintSoldiersInfo(_soldiersSecondGroup, "Изначально солдаты во втором отряде:");
        
        SoldiersTransplantation();
        
        PrintSoldiersInfo(_soldiersFirstGroup,"После перевода солдаты в первом отряде:");
        PrintSoldiersInfo(_soldiersSecondGroup, "После перевода солдаты во втором отряде:");
    }

    private void SoldiersTransplantation()
    {
        var transplantSoldiers = _soldiersFirstGroup.
            Where(soldier => soldier.Surname.ToUpper().StartsWith("Б")).ToList();

        _soldiersFirstGroup = _soldiersFirstGroup.Except(transplantSoldiers).ToList();
        
        _soldiersSecondGroup = _soldiersSecondGroup.Union(transplantSoldiers).ToList();
    }
    
    private void PrintSoldiersInfo(List<Soldier> soldiers, string description)
    {
        Console.WriteLine(description);

        foreach (var soldier in soldiers)
        {
            Console.WriteLine($"Имя - {soldier.Name}, фамилия - {soldier.Surname}, " +
                               $"возраст {soldier.Age}");
        }
        
        Console.WriteLine();
    }

    private void GenerateSoldiers()
    {
        _soldiersFirstGroup.Add(new Soldier("Пётр", "Иванов", 25));
        _soldiersFirstGroup.Add(new Soldier("Алексей", "Белов", 22));
        _soldiersFirstGroup.Add(new Soldier("Игорь", "Сидоров", 27));
        _soldiersFirstGroup.Add(new Soldier("Николай", "Бондаренко", 24));
        _soldiersFirstGroup.Add(new Soldier("Дмитрий", "Кузнецов", 26));
        _soldiersFirstGroup.Add(new Soldier("Сергей", "Бояринов", 23));
        _soldiersFirstGroup.Add(new Soldier("Антон", "Смирнов", 21));
        
        _soldiersSecondGroup.Add(new Soldier("Михаил", "Боярцев", 20));
        _soldiersSecondGroup.Add(new Soldier("Виктор", "Буров", 28));
        _soldiersSecondGroup.Add(new Soldier("Евгений", "Фёдоров", 23));
        _soldiersSecondGroup.Add(new Soldier("Константин", "Быков", 25));
        _soldiersSecondGroup.Add(new Soldier("Олег", "Морозов", 27));
        _soldiersSecondGroup.Add(new Soldier("Станислав", "Беляев", 22));
        _soldiersSecondGroup.Add(new Soldier("Андрей", "Ковалёв", 24));
    }
}

internal class Soldier
{
    public Soldier(string name, string surname, int age)
    {
        Name = name;
        Surname = surname;
        Age = age;
    }

    public string Name { get; private set; }
    public string Surname { get; private set; }
    public int Age { get; private set; }
}