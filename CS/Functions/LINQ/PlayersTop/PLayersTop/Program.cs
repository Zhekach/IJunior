internal class Program
{
    
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