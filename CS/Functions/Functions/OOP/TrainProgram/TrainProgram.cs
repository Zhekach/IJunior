using System;
using System.Collections.Generic;

namespace Functions.OOP.TrainProgram
{
    internal class TrainProgram
    {
        static void Main1()
        {
            RailwayStation railwayStation = new RailwayStation();   
            railwayStation.Run();
        }
    }

    class RailwayStation
    {
        private const string UserExitCommand = "выход";
        private bool _isUserExited;

        public void Run()
        {
            while(_isUserExited == false)
            {
                TrainBuilder trainBuilder = new TrainBuilder();
                Train train = trainBuilder.GetTrain();

                Console.WriteLine("Для отправки поезда введите любой текст.");

                train.Depart();

                Console.WriteLine("Для создания следующего поезда введите любой текст\n" +
                                  $"Для выхода введите \"{UserExitCommand}\".");

                if(UserExitCommand == Console.ReadLine())
                {
                    _isUserExited = true;
                } 
            }
        }
    }

    class TrainBuilder
    {
        private readonly Dictionary<string, int> _carTypes = new Dictionary<string, int>()
        {
            {"Сидячий", 107},
            {"Общий", 72},
            {"Плацкартный", 54},
            {"Купе", 36},
            {"СВ", 20},
            {"Люкс", 8},
        };
        private Train _train;
        private Route _route;
        private int _passengersCount;
        private List<TrainCar> _cars;

        private Random _random;

        public TrainBuilder()
        {
            _random = new Random();
            _cars = new List<TrainCar>();
        }

        private void BuildRoute()
        {
            bool isRouteOk = false;
            string arrival;
            string departure;

            while(isRouteOk == false)
            {
                PrintUI();

                Console.WriteLine("Введите пункт отправления: ");
                arrival = Console.ReadLine();

                Console.WriteLine("Введите пункт прибытия: ");
                departure = Console.ReadLine();

                if(arrival.ToLower() != departure.ToLower())
                {
                    _route = new Route(arrival, departure);
                    _route.PrintInfo();
                    isRouteOk = true;
                }
                else
                {
                    Console.WriteLine("Пункт отправления и прибытия совпадают. Так нельзя.\n" +
                                      "Нажмите любую клавишу для повтора.");
                    Console.ReadKey();
                }
            }
        }

        private void BuildPassangers()
        {
            PrintUI();

            int maxPassengers = 0;
            bool isMaxPassengersOk = false;

            while (isMaxPassengersOk == false)
            {
                Console.WriteLine("Введите максимальное количество билетов, которое можно продать." +
                                  "Число должно быть целым и положительным");
                maxPassengers = ReadInt();

                if (maxPassengers >= 1)
                {
                    isMaxPassengersOk = true;
                }
            }

            Console.WriteLine("Нажмите любую кнопку для продажи билетов на поезд");
            Console.ReadKey();

            _passengersCount = _random.Next(1, maxPassengers + 1);

            Console.WriteLine($"\nБыло продано {_passengersCount} билетов");
        }

        private void BuildCars()
        {
            int unallocatedPassengers = _passengersCount;

            while (unallocatedPassengers > 0)
            {
                PrintUI();
                string carType;
                TrainCar car = null;

                Console.WriteLine($"Осталось распределить {unallocatedPassengers} пассажиров.");
                Console.WriteLine("Введите тип вагона:");

                foreach(var type in _carTypes)
                {
                    Console.WriteLine($"Вагон {type.Key}, вместительность - {type.Value} мест.");
                }

                carType = Console.ReadLine();

                if(_carTypes.ContainsKey(carType))
                {
                    car = new TrainCar(carType, _carTypes[carType]);
                    _cars.Add(car);
                    unallocatedPassengers -= car.Capacity;

                    Console.WriteLine("Добавлен:");
                    car.PrintInfo();
                }
                else
                {
                    Console.WriteLine("Похоже, вы ошиблись с типом вагона =(");
                }

                Console.WriteLine("Для продолжения нажмите любую клавишу.");
                Console.ReadKey();
            }
        }

        private void PrintUI()
        {
            Console.Clear();

            Console.WriteLine("Текущий поезд:");
            _route.PrintInfo();
            Console.WriteLine($"Продано билетов: {_passengersCount}");
            Console.WriteLine("Список вагонов:");

            foreach (TrainCar car in _cars)
            {
                car.PrintInfo();
            }

            Console.WriteLine("\n===================\n");
        }

        public Train GetTrain()
        {
            BuildRoute();
            BuildPassangers();
            BuildCars();

            Train train = new Train(_route, _passengersCount, _cars);
            return train;
        }

        private int ReadInt()
        {
            bool isIntEntered = false;
            int parsedInt = 0;

            while (isIntEntered == false)
            {
                string enteredString;

                Console.WriteLine("Введите число:");
                enteredString = Console.ReadLine();

                isIntEntered = int.TryParse(enteredString, out parsedInt);

                if (isIntEntered)
                {
                    Console.WriteLine($"Введенное число распознано: {parsedInt}");
                }
                else
                {
                    Console.WriteLine("Вы ввели некорректное число. Попробуйте ещё.\n");
                }
            }

            return parsedInt;
        }
    }

    class Train
    {
        private Route _route;
        private int _passengersCount = 0;
        private List<TrainCar> _cars = new List<TrainCar>();
        private bool _isDeparted = false;

        public Train(Route route, int passengersCount, List<TrainCar> cars)
        {
            _route = route;
            _passengersCount = passengersCount;
            _cars = cars;
        }

        public void Depart()
        {
            _isDeparted = true;
            Console.WriteLine("Поезд отправился. В добрый путь.");
        }
    }

    class TrainCar
    {
        public TrainCar(string type, int capacity)
        {
            Type = type;
            Capacity = capacity;
        }

        public string Type { get; private set; }
        public int Capacity { get; private set; }

        public void PrintInfo()
        {
            Console.WriteLine($"{Type} вагон, в нём {Capacity} мест");
        }
    }

    struct Route
    {
        private string _departure;
        private string _arrival;

        public Route(string departure, string arrival)
        {
            _departure = departure;
            _arrival = arrival;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Муршрут: пункт отправления - {_departure}, пункт прибытия - {_arrival}.");
        }
    }
}