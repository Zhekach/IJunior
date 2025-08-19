class Program
{
    static void Main(string[] args)
    {
        AutoServiceFactory autoServiceFactory = new AutoServiceFactory();
        AutoService autoService = autoServiceFactory.GenerateAutoService();
        autoService.PrintInfo();
    }
}

internal class AutoService
{
    private Queue<Car> _cars;
    private List<Detail> _details;
    private int _balance;

    private readonly Appraiser _appraiser = new Appraiser();
    private readonly Mechanic _mechanic = new Mechanic();

    public AutoService(Queue<Car> cars, List<Detail> details, int balance)
    {
        _cars = cars;
        _details = details;
        _balance = balance;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Баланс: {_balance} рублей.");

        Console.WriteLine("Список автомобилей:");
        foreach (var car in _cars)
        {
            car.PrintInfo();
        }

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

    public int AppraiseCarPenalty(Car car)
    {
        int penalty = Utility.GetRandomInt(MinCarPenalty, MaxCarPenalty);

        return penalty;
    }

    public int AppraiseDetailPenalty(Detail detail)
    {
        int penalty = (int)(detail.Price * CoefficientDetailPenalty);

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
    public bool CheckIsCarRepaired(Car car)
    {
        bool isCarRepaired = false;

        foreach (var detail in car.Details)
        {
            if (detail.IsBroken)
            {
                return false;
            }
        }

        return true;
    }

    public bool TryRepairDetail(Car car, List<Detail> detailsInStock)
    {
        bool isDetailRepaired = false;
        List<Detail> carDetails = new List<Detail>(car.Details);

        foreach (var detail in carDetails)
        {
            if (detail.IsBroken == false)
            {
                continue;
            }
            
            Detail? detailToRepair = FindDetailToRepair(detail, detailsInStock);

            if (detailToRepair == null)
            {
                continue;
            }

            if (car.TryRepairDetail(detailToRepair) == false)
            {
                continue;
            }
            
            detailsInStock.Remove(detailToRepair);
            isDetailRepaired = true;
            
            break;
        }

        return isDetailRepaired;
    }

    private Detail? FindDetailToRepair(Detail brokenDetail, List<Detail> detailsInStock)
    {
        Detail detailToRepair = null;

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
        private const int minDetailsCount = 10;
        private const int maxDetailsCount = 20;
        private const int minBalance = 100;
        private const int maxBalance = 5000;
        private const int minCarsCount = 2;
        private const int maxCarsCount = 4;

        private DetailFactory _detailFactory = new DetailFactory();
        private CarFactory _carFactory = new CarFactory();

        public AutoService GenerateAutoService()
        {
            int detailsCount = Utility.GetRandomInt(minDetailsCount, maxDetailsCount);
            int balance = Utility.GetRandomInt(minBalance, maxBalance);
            int carsCount = Utility.GetRandomInt(minCarsCount, maxCarsCount);

            List<Detail> details = _detailFactory.GenerateRandomDetails(detailsCount);
            List<Car> cars = _carFactory.GenerateCars(carsCount);

            AutoService autoService = new AutoService(new Queue<Car>(cars), details, balance);

            return autoService;
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

        private DetailFactory _detailFactory = new DetailFactory();

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
                detailToBroke.SetBroken(true);
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