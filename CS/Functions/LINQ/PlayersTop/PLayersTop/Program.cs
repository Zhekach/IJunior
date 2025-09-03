internal class Program
{
    public static void Main()
    {
        Application application = new Application(10);
        application.Run();
    }
}

internal class Application
{
    private const int TopPlayersCount = 3;
    
    private readonly PlayerGenerator _playerGenerator;
    private readonly List<Player> _players;
    
    public Application(int playersCount)
    {
        _playerGenerator = new PlayerGenerator();
        _players = _playerGenerator.GeneratePlayers(playersCount);
    }
    
    public void Run()
    {
        PrintPlayersInfo(_players, "Исходные игроки:");
        
        var topPlayersLevel = SelectTopPlayersLevel(_players);
        PrintPlayersInfo(topPlayersLevel, "Топ игроки по уровню:");
        
        var topPlayersPower = SelectTopPlayersPower(_players);
        PrintPlayersInfo(topPlayersPower, "Топ игроки по силе:");
    }

    private List<Player> SelectTopPlayersLevel(List<Player> players)
    {
        var result = players
            .OrderBy(player => player.Level)
            .Reverse()
            .Take(TopPlayersCount).ToList();
        
        return result;
    }
    
    private List<Player> SelectTopPlayersPower(List<Player> players)
    {
        var result = players
            .OrderBy(player => player.Power)
            .Reverse()
            .Take(TopPlayersCount).ToList();
        
        return result;
    }

    private void PrintPlayersInfo(List<Player> players, string description)
    {
        Console.WriteLine(description);
        
        foreach (var player in players)
        {
            Console.WriteLine($"Имя: {player.Name}, Уровень: {player.Level}, Сила: {player.Power}");
        }
    }
}

internal class Player
{
    public Player(string name, int level, int power)
    {
        Name = name;
        Level = level;
        Power = power;
    }
    
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int Power { get; private set; }
}

internal class PlayerGenerator
{
    private readonly List<string> _names = new List<string>()
    {
        "John",
        "Jane",
        "Jack",
        "Jill",
        "Joe",
        "Jenny",
        "Jasper",
        "Jasmine",
        "Jared",
        "Jenna"
    };

    private readonly int _minLevel = 1;
    private readonly int _maxLevel = 100;

    private readonly int _minPower = 10;
    private readonly int _maxPower = 500;

    public List<Player> GeneratePlayers(int count)
    {
        var players = new List<Player>();

        for (int i = 0; i < count; i++)
        {
            string name = _names[Utility.GetRandomInt(_names.Count)];
            int level = Utility.GetRandomInt(_minLevel, _maxLevel + 1);
            int score = Utility.GetRandomInt(_minPower, _maxPower + 1);

            players.Add(new Player(name, level, score));
        }

        return players;
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