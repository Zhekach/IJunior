using System.Collections.ObjectModel;

class Program
{
    static void Main(string[] args)
    {
        
    }
}

class Car
{
    private HashSet<Detail> _details;

    public Car(HashSet<Detail> details)
    {
        _details = new HashSet<Detail>(details);
    }

    public IReadOnlyCollection<Detail> Details => _details;

    public bool TryRepairDetail(Detail detailToRepair)
    {
        bool isRepaired = false;

        if (detailToRepair.IsBroken)
        {
            return false;
        }

        Detail brokenDetail = null;

        foreach (var detail in _details)
        {
            if (detail.Type == detailToRepair.Type)
            {
                brokenDetail = detail;
                break;
            }
        }

        if (brokenDetail == null)
        {
            return false;
        }

        _details.Remove(brokenDetail);
        _details.Add(detailToRepair);
        isRepaired = true;

        return isRepaired;
    }

    public void PrintInfo()
    {
        foreach (var detail in _details)
        {
            detail.PrintInfo();
        }
    }
}

class CarFactory
{
    private const int MinBrokenDetails = 1;
    
    private DetailFactory _detailFactory = new DetailFactory();
    
    public List<Car> GenerateCars(int count)
    {
        List<Car> cars = new List<Car>();
        
        for (int i = 0; i < count; i++)
        {
            cars.Add(GenerateCar());
        }
        
        return cars;
    }

    private Car GenerateCar()
    {
        HashSet<Detail> details = new HashSet<Detail>();
        
        List<DetailType> detailTypes = new List<DetailType>(
            (DetailType[])Enum.GetValues(typeof(DetailType)));

        foreach (var detailType in detailTypes)
        {
            Detail newDetail = _detailFactory.GenerateDetail(detailType);
            details.Add(newDetail);
        }
        Car car = new Car(details);
        
        return car;
    }
}

class Detail
{
    public Detail(DetailType type, int price)
    {
        Type = type;
        Price = price;
    }

    public DetailType Type { get; }
    public int Price { get; }
    public bool IsBroken { get; private set; } = false;

    public void SetBroken(bool isBroken) => IsBroken = isBroken;

    public void PrintInfo()
    {
        Console.WriteLine($"Тип детали - {Type}, стоимость - {Price} рублей." +
                          $"Состояние: сломана - {IsBroken}");
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Detail other) return false;
        return Type == other.Type && Price == other.Price;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Price);
    }
}

internal class DetailFactory
{
    private const int MaxPrice = 1000;
    private const int MinPrice = 100;

    public List<Detail> GenerateRandomDetails(int count)
    {
        List<Detail> details = new List<Detail>();

        for (int i = 0; i < count; i++)
        {
            DetailType type = Utility.GetRandomEnumValue<DetailType>();
            Detail detail = GenerateDetail(type);
            details.Add(detail);
        }

        return details;
    }

    public Detail GenerateDetail(DetailType type)
    {
        int price = Utility.GetRandomInt(MinPrice, MaxPrice);

        Detail detail = new Detail(type, price);

        return detail;
    }
}

enum DetailType
{
    Wheel,
    Engine,
    Body,
    Chassis,
    Brake,
    Gearbox,
    Suspension,
    Lights
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
            string enteredString;

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