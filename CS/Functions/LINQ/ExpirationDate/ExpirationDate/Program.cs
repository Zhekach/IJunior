internal class Program
{
    public static void Main()
    {
        Application application = new Application();
        application.Run();
    }
}

internal class Application
{
    private readonly List<StewCan> _stewCans;

    public Application()
    {
        _stewCans = GenerateStewCans();
    }
    
    public void Run()
    {
        PrintCansInfo(_stewCans, "Все имеющиеся банки:");
        
        var expiredCans = _stewCans
            .Where(can => can.ProductionDate.Year + can.LifeTime < DateTime.Now.Year).ToList(); 
        
        PrintCansInfo(expiredCans, "Срок годности истек:");
    }

    private void PrintCansInfo(List<StewCan> stewCans, string description)
    {
        Console.WriteLine(description);

        foreach (var can in stewCans)
        {
            Console.WriteLine($"Название: {can.Name}, Дата производства: {can.ProductionDate}, Срок годности: {can.LifeTime}");
        }
    }

    private List<StewCan> GenerateStewCans()
    {
        List<StewCan> stewCans = new List<StewCan>();
        
        stewCans.Add(new StewCan("Говядина", new DateTime(2022, 5, 10), 1)); // просрочена
        stewCans.Add(new StewCan("Свинина", new DateTime(2023, 3, 15), 2));  // годна
        stewCans.Add(new StewCan("Курица", new DateTime(2021, 11, 1), 2));   // просрочена
        stewCans.Add(new StewCan("Говядина", new DateTime(2024, 1, 20), 2)); // годна
        stewCans.Add(new StewCan("Свинина", new DateTime(2022, 8, 5), 2));   // годна
        stewCans.Add(new StewCan("Курица", new DateTime(2020, 6, 12), 2));   // просрочена
        stewCans.Add(new StewCan("Говядина", new DateTime(2023, 9, 1), 1));  // годна
        stewCans.Add(new StewCan("Свинина", new DateTime(2021, 2, 25), 2));  // просрочена
        stewCans.Add(new StewCan("Курица", new DateTime(2023, 12, 30), 3));  // годна
        stewCans.Add(new StewCan("Говядина", new DateTime(2022, 7, 14), 1)); // просрочена
        
        return stewCans;
    }
}

internal class StewCan
{
    public StewCan(string name, DateTime productionDate, int lifeTime)
    {
        Name = name;
        ProductionDate = productionDate;
        LifeTime = lifeTime;
    }

    public string Name { get; private set; }
    public DateTime ProductionDate { get; private set; }
    public int LifeTime { get; private set; }
}