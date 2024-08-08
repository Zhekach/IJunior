using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Functions.OOP.Aquarium
{
    internal class AquariumProgram
    {
        static void Main1()
        {
            AquariumController controller = new AquariumController();

            controller.ControlAquarium();
        }
    }

    internal class AquariumController
    {
        private const ConsoleKey AddFishKey = ConsoleKey.A;
        private const ConsoleKey RemoveFishKey = ConsoleKey.D;
        private const ConsoleKey ExitKey = ConsoleKey.E;
        
        private readonly Aquarium _aquarium;
        private readonly string _userInterfaceDescription =
            "Для управления аквариумом нажмите:\n" +
            $"{AddFishKey} - Добавить рыбку\n" +
            $"{RemoveFishKey} - Удалить случайную рыбку\n" +
            $"{ExitKey} - Выход из программы\n";
        
        private bool _isUserExited;

        public AquariumController()
        {
            _aquarium = new Aquarium(_userInterfaceDescription);
        }

        public void ControlAquarium()
        {
            _aquarium.Run();

            while (_isUserExited == false)
            {
                ManageAqua();
            }
        }

        private void ManageAqua()
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey();

            switch (pressedKey.Key)
            {
                case AddFishKey:
                    _aquarium.AddFish();
                    break;

                case RemoveFishKey:
                    _aquarium.RemoveFish();
                    break;

                case ExitKey:
                    _isUserExited = true;
                    break;
            }
        }
    }

    internal class Aquarium
    {
        private readonly int _iterationTimeMilliseconds = 1000;
        private readonly string _userInterfaceDescription;
        private readonly List<Fish> _fishes = new List<Fish>();
        
        private int _currentFishId = 1;
        private int _currentIteration = 1;

        public Aquarium(string userInterfaceDescription)
        {
            _userInterfaceDescription = userInterfaceDescription;
        }

        public void Run()
        {
            TimerCallback timerCallback = new TimerCallback(ReleaseIteration);
            Timer timer = new Timer(timerCallback, null, 0, _iterationTimeMilliseconds);
        }

        public void AddFish()
        {
            _fishes.Add(new Fish(_currentFishId));
            _currentFishId++;
        }

        public void RemoveFish()
        {
            _fishes.RemoveAt(RandomUtility.GetRandomInt(_fishes.Count()));
        }

        private void PrintInfo()
        {
            Console.WriteLine($"======{_currentIteration}=====");

            foreach (Fish fish in _fishes)
            {
                fish.PrintInfo();
            }
        }

        private void PrintUserGuide()
        {
            Console.WriteLine(_userInterfaceDescription);
        }

        private void ReleaseIteration(object obj)
        {
            foreach (Fish fish in _fishes)
            {
                fish.OnIteration();
            }

            _currentIteration++;

            Console.Clear();

            PrintUserGuide();
            PrintInfo();
        }
    }

    internal class Fish
    {
        private const float AgeIncrement = 0.1f;
        private const int MaxChanceToLive = 95;
        private const int ChanceToLive = 95;
        private const string AliveText = "жива";
        private const string DeadText = "не очень";

        private readonly int _id;
        private string _statusText = AliveText;
        private bool _isDead;
        private float _age;

        public Fish(int id)
        {
            _id = id;

            OnIteration += IncrementAge;
            OnIteration += TryDead;
        }

        public IterationReaction OnIteration { get; private set; }

        public delegate void IterationReaction();

        public void PrintInfo()
        {
            if (_isDead)
            {
                _statusText = DeadText;
            }

            Console.WriteLine($"Рыбка под номером {_id}: {_statusText}." +
                $" Ей {_age} рыбьих лет");
        }

        private void IncrementAge()
        {
            if (_isDead == false)
            {
                _age += AgeIncrement;
            }
        }

        private void TryDead()
        {
            if (RandomUtility.GetRandomInt(MaxChanceToLive) > ChanceToLive)
            {
                _isDead = true;
                
                OnIteration -= IncrementAge;
                OnIteration -= TryDead;
            }
        }
    }

    internal static class RandomUtility
    {
        private static readonly Random s_random = new Random();

        public static int GetRandomInt(int maxValue)
        {
            return s_random.Next(maxValue);
        }
    }
}