using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Functions.OOP.Aquarium
{
    internal class AquariumProgram
    {
        private const ConsoleKey AddFishKey = ConsoleKey.A;
        private const ConsoleKey RemoveFishKey = ConsoleKey.D;
        private const ConsoleKey ExitKey = ConsoleKey.E;
        static void Main()
        {
            bool isUserExited = false;
            Aquarium aqua = new Aquarium();

            aqua.Run();

            while (isUserExited == false)
            {
                ManageAqua(aqua, ref isUserExited);
            }
        }

        private static void ManageAqua(Aquarium aqua, ref bool isUserExited)
        {
            var pressedKey = Console.ReadKey();

            switch (pressedKey.Key)
            {
                case AddFishKey:
                    aqua.AddFish();
                    break;
                case RemoveFishKey:
                    aqua.RemoveFish();
                    break;
                case ExitKey:
                    isUserExited = true;
                    break;
                default:
                    break;
            }
        }
    }
    internal class Aquarium
    {
        private List<Fish> _fishes = new List<Fish>();
        private readonly int _iterationTimeMillis = 1000;
        private int _currentFishId = 1;
        private int _currentIteration = 1;

        public void Run()
        {
            TimerCallback TimerCallback = new TimerCallback(ReleaseIteration);
            Timer timer = new Timer(TimerCallback, null, 0, _iterationTimeMillis);
        }

        public void AddFish()
        {
            _fishes.Add(new Fish(_currentFishId));
            _currentFishId++;
        }

        public void RemoveFish()
        {
            _fishes.RemoveAt(Util.GetRandomInt(_fishes.Count()));
        }

        public void PrintInfo()
        {
            Console.WriteLine($"======{_currentIteration}=====");

            foreach (Fish fish in _fishes)
            {
                fish.PrintInfo();
            }
        }

        public void PrintUserGuide()
        {
            Console.WriteLine("Для управления аквариумом нажмите:\n" +
                "А - Добавить рыбку\n" +
                "D - Удалить случайную рыбку\n" +
                "Е - Выход из программы\n");
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
        private readonly float _ageIncrement = 0.1f;
        private readonly int _chanceToLive = 95;
        private int _id;
        private bool _isDead = false;
        private float _age = 0f;

        public Fish(int id)
        {
            _id = id;

            OnIteration += IncrementAge;
            OnIteration += CheckDead;
        }

        public IterationReaction OnIteration { get; private set; }

        public delegate void IterationReaction();

        public void PrintInfo()
        {
            string aliveText = "жива";
            string deadText = "не очень";
            string statusText = aliveText;

            if (_isDead) { statusText = deadText; }

            Console.WriteLine($"Рыбка под номером {_id}: {statusText}." +
                $" Ей {_age} рыбьих лет");
        }

        private void IncrementAge()
        {
            if (_isDead == false)
            {
                _age += _ageIncrement;
            }
        }

        private void CheckDead()
        {
            if (Util.GetRandomInt(100) > _chanceToLive)
            {
                _isDead = true;
            }
        }
    }

    internal class Util
    {
        private static readonly Random s_random = new Random();

        public static int GetRandomInt(int maxValue)
        {
            return s_random.Next(maxValue);
        }
    }
}