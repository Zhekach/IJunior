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
    private const ConsoleKey AgryKey = ConsoleKey.Y;
    private const ConsoleKey RefuseKey = ConsoleKey.N;
    private const ConsoleKey ExitKey = ConsoleKey.Q;
    private const int RefuseCommandId = 0;
    private const int ExceptionCommandId = -1; 
    
    private readonly AutoService _autoService;
    private readonly ConsoleUi _consoleUi;

    public Application()
    {
        AutoServiceFactory autoServiceFactory = new AutoServiceFactory();

        _autoService = autoServiceFactory.GenerateAutoService();
        _consoleUi = new ConsoleUi(_autoService, AgryKey, RefuseKey, ExitKey, RefuseCommandId, ExceptionCommandId);
    }

    public void Run()
    {
        bool isUserExited = false;
        
        while (isUserExited == false && _autoService.HasCarsInQueue())
        {
            _consoleUi.ShowMainMenu();
            ConsoleKeyInfo pressedKey = Console.ReadKey();

            switch (pressedKey.Key)
            {
                case AgryKey: 
                    ChooseDetail();
                    break;
                
                case RefuseKey:
                    RefuseCar();
                    break;
                 
                case ExitKey:
                    isUserExited = true;
                    break;
            }
        }
    }

    private void ChooseDetail()
    {
        _autoService.SetNewCurrentCar();
        
        Car? car = _autoService.CurrentCar;

        if (car == null)
        {
            return;
        }

        while (_autoService.HasCurrentCar() && _autoService.HasDetails())
        {
            int detailIndex = _consoleUi.ShowChooseDetailMenu();

            switch (detailIndex)
            {
                case ExceptionCommandId:
                    Console.WriteLine("Ошибка, нет машины в ремонте.");
                    break;
                
                case RefuseCommandId:
                    _autoService.RefuseCar();
                    break;
                
                default:
                    RepairDetail(car, detailIndex);
                    break;
            }
        }
                
        Console.Clear();
    }

    private void RefuseCar()
    {
        _autoService.RefuseCar();
        _consoleUi.ShowWaitingMenu();
        
        Console.Clear();
    }

    private void RepairDetail(Car car, int detailIndex)
    {
        Detail detail = car.Details.ElementAt(detailIndex - 1);
        _autoService.RepairDetail(detail);
        _consoleUi.ShowWaitingMenu();
    }
}

internal class ConsoleUi
{
    private readonly AutoService _autoService;
    private readonly ConsoleKey _agryKey;
    private readonly ConsoleKey _refuseKey;
    private readonly ConsoleKey _exitKey;
    private readonly int _refuseCommandId;
    private readonly int _exceptionCommandId;

    public ConsoleUi(AutoService autoService,
        ConsoleKey agryKey, ConsoleKey refuseKey, ConsoleKey exitKey,
        int refuseCommandId, int exceptionCommandId)
    {
        _autoService = autoService;
        _agryKey = agryKey;
        _refuseKey = refuseKey;
        _exitKey = exitKey;
        _refuseCommandId = refuseCommandId;
        _exceptionCommandId = exceptionCommandId;
    }

    public void ShowMainMenu()
    {
        Console.Clear();
        _autoService.PrintInfo();

        Console.WriteLine("Выберите действие:\n" +
                          $"{_agryKey} - начать ремонт\n" +
                          $"{_refuseKey} - отказаться от ремонта\n" +
                          $"{_exitKey} - выход\n");
    }

    public void ShowWaitingMenu()
    {
        Console.WriteLine("Нажмите любую клавишу для продолжения.");
        Console.ReadKey();
    }

    public int ShowChooseDetailMenu()
    {
        Console.Clear();

        Car? car = _autoService.CurrentCar;

        if (car == null)
        {
            return _exceptionCommandId;
        }

        Console.WriteLine("Выберите деталь для ремонта:");

        int index = 1;

        foreach (Detail detail in car.Details)
        {
            if (detail.IsBroken)
            {
                Console.WriteLine($"{index} - {detail.Type}.");
            }

            index++;
        }

        Console.WriteLine($"Для отказа от ремонта введите \"{_refuseCommandId}\".");

        int result = Utility.ReadInt();

        return result;
    }
}

internal class AutoService
{
    private readonly Queue<Car> _cars;
    private readonly List<Detail> _details;
    private int _balance;
    private Car? _currentCar;

    private readonly Mechanic _mechanic;
    private readonly Appraiser _appraiser = new Appraiser();

    public AutoService(Queue<Car> cars, List<Detail> details, int balance)
    {
        _cars = cars;
        _details = details;
        _balance = balance;
        _mechanic = new Mechanic(details);
    }

    public bool HasCarsInQueue() => _cars.Count > 0;
    public bool HasCurrentCar() => _currentCar != null;
    public bool HasDetails() => _details.Count > 0;
    public Car? CurrentCar => _currentCar;

    public void SetNewCurrentCar()
    {
        if (_currentCar == null && _cars.Count > 0)
        {
            _currentCar = _cars.Dequeue();
        }
        else
        {
            Console.WriteLine("Нет автомобилей в очереди.");
        }
    }

    public void RefuseCar()
    {
        if (_currentCar == null)
        {
            int penalty = _appraiser.GeneratePreRepairPenalty();
            _balance -= penalty;
            Console.WriteLine($"Штраф за отказ от начала ремонта: {penalty} рублей.");
        }
        else
        {
            int penalty = _appraiser.GenerateInProgressPenalty(_currentCar);
            _balance -= penalty;
            _currentCar = null;
            Console.WriteLine($"Штраф за отказ от продолжения ремонта: {penalty} рублей.");
        }
    }
    
    public void RepairDetail(Detail brokenDetail)
    {
        if (_currentCar == null || brokenDetail.IsBroken == false)
        {
            return;
        }

        _mechanic.RepairDetail(_currentCar, brokenDetail);
        int bonus = _appraiser.AppraiseDetailBonus(brokenDetail);
        _balance += bonus;
        Console.WriteLine($"Начислено {bonus} рублей за ремонт детали {brokenDetail.Type}.");

        if (_mechanic.CheckIsCarRepaired(_currentCar))
        {
            _currentCar = null;
            Console.WriteLine("Ремонт машины завершен.");
        }
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Баланс: {_balance} рублей.");
        Console.WriteLine($"Автомобилей в очереди: {_cars.Count}");
        Console.WriteLine("Следующий автомобиль:");

        Car car = _cars.Peek();
        car.PrintInfo();

        Console.WriteLine("Список деталей:");
        
        foreach (var detail in _details)
        {
            detail.PrintInfo();
        }
    }
}

internal class Appraiser
{
    private const int MinCarPenalty = 200;
    private const int MaxCarPenalty = 500;
    private const float CoefficientDetailPenalty = 1.5f;
    private const float CoefficientDetailBonus = 1.2f;
    
    public int GeneratePreRepairPenalty()
    {
        int penalty = Utility.GetRandomInt(MinCarPenalty, MaxCarPenalty);

        return penalty;
    }

    public int GenerateInProgressPenalty(Car car)
    {
        int penalty = 0;

        foreach (var detail in car.Details)
        {
            if (detail.IsBroken)
            {
                penalty += (int)(detail.Price * CoefficientDetailPenalty);
            }
        }

        return penalty;
    }

    public int AppraiseDetailBonus(Detail detail)
    {
        int bonus = (int)(detail.Price * CoefficientDetailBonus);

        return bonus;
    }
}

internal class Mechanic
{
    private readonly List<Detail> _detailsInStock;

    public Mechanic(List<Detail> detailsInStock)
    {
        _detailsInStock = detailsInStock;
    }

    public bool CheckIsCarRepaired(Car car)
    {
        bool isCarRepaired = false;

        foreach (var detail in car.Details)
        {
            if (detail.IsBroken)
            {
                return isCarRepaired;
            }
        }

        isCarRepaired = true;

        return isCarRepaired;
    }

    public void RepairDetail(Car car, Detail brokenDetail)
    {
        var newDetail = FindDetailToRepair(brokenDetail, _detailsInStock);

        if (newDetail == null ||
            car.TryRepairDetail(brokenDetail, newDetail) == false)
        {
            return;
        }

        _detailsInStock.Remove(brokenDetail);
    }

    private Detail? FindDetailToRepair(Detail brokenDetail, List<Detail> detailsInStock)
    {
        Detail? detailToRepair = null;

        foreach (var detail in detailsInStock)
        {
            if (detail.Type == brokenDetail.Type)
            {
                detailToRepair = detail;
                break;
            }
        }

        return detailToRepair;
    }
}

internal class AutoServiceFactory
{
    private const int MinDetailsCount = 20;
    private const int MaxDetailsCount = 50;
    private const int MinBalance = 100;
    private const int MaxBalance = 5000;
    private const int MinCarsCount = 2;
    private const int MaxCarsCount = 4;

    private readonly DetailFactory _detailFactory = new ();
    private readonly CarFactory _carFactory = new ();

    public AutoService GenerateAutoService()
    {
        int detailsCount = Utility.GetRandomInt(MinDetailsCount, MaxDetailsCount);
        int balance = Utility.GetRandomInt(MinBalance, MaxBalance);
        int carsCount = Utility.GetRandomInt(MinCarsCount, MaxCarsCount);

        List<Detail> details = _detailFactory.GenerateRandomDetails(detailsCount);
        List<Car> cars = _carFactory.GenerateCars(carsCount);

        AutoService autoService = new AutoService(new Queue<Car>(cars), details, balance);

        return autoService;
    }
}

internal class Car
{
    private readonly HashSet<Detail> _details;

    public Car(HashSet<Detail> details)
    {
        _details = new HashSet<Detail>(details);
    }

    public IReadOnlyCollection<Detail> Details => _details;

    public bool TryRepairDetail(Detail? brokenDetail, Detail newDetail)
    {
        bool isRepaired;

        if (newDetail.IsBroken)
        {
            return false;
        }

        if (brokenDetail == null)
        {
            return false;
        }

        _details.Remove(brokenDetail);
        _details.Add(newDetail);
        isRepaired = true;

        return isRepaired;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Информация об автомобиле, сломаны детали:");

        foreach (var detail in _details)
        {
            if (detail.IsBroken)
            {
                detail.PrintInfo();
            }
        }
    }
}

class CarFactory
{
    private const int MinBrokenDetails = 1;

    private readonly DetailFactory _detailFactory = new DetailFactory();

    public List<Car> GenerateCars(int count)
    {
        List<Car> cars = new List<Car>();

        for (int i = 0; i < count; i++)
        {
            Car car = GenerateCar();
            BrokeRandomDetails(car);
            cars.Add(car);
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

    private void BrokeRandomDetails(Car car)
    {
        int brokenDetailsCount = Utility.GetRandomInt(MinBrokenDetails, car.Details.Count);

        for (int i = 0; i < brokenDetailsCount; i++)
        {
            int index = Utility.GetRandomInt(0, car.Details.Count - 1);
            Detail detailToBroke = car.Details.ElementAt(index);
            detailToBroke.SetBroken();
        }
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

    public void SetBroken()
    {
        IsBroken = true;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Тип детали - {Type}, стоимость - {Price} рублей." +
                          $"Состояние: сломана - {IsBroken}");
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Detail other)
        {
            return false;
        }
        
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