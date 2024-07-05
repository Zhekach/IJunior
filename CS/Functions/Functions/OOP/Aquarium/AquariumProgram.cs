using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Functions.OOP.Aquarium
{
    internal class AquariumProgram
    {
        private static bool s_isUserExited = false;
        private static Aquarium s_aqua;
        static void Main()
        {
            s_aqua = new Aquarium();
            s_aqua.Run();

            while (s_isUserExited == false)
            {
                ManageAqua();
            }
        }

        private static void ManageAqua()
        {
            ConsoleKeyInfo pressedKey;

            pressedKey = Console.ReadKey();

            switch (pressedKey.Key)
            {
                case ConsoleKey.A:
                    s_aqua.AddFish();
                    break;
                case ConsoleKey.D:
                    s_aqua.RemoveFish();
                    break;
                case ConsoleKey.E:
                    s_isUserExited = true;
                    break;
                default:
                    break;
            }
        }
    }
    internal class Aquarium
    {
        private List<Fish> _fishes;
        private readonly int _iterationTimeMillis;
        private int _currentFishId;
        private int _currentIteration;

        public Aquarium()
        {
            _fishes = new List<Fish>();
            _iterationTimeMillis = 1000;
            _currentFishId = 1;
            _currentIteration = 1;
        }

        public void Run()
        {
            TimerCallback tm = new TimerCallback(ReleaseIteration);
            Timer timer = new Timer(tm, null, 0, _iterationTimeMillis);
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
        private readonly float _ageIncrement;
        private readonly int _chanceToLive;
        private int _id;
        private bool _isDead;
        private float _age;

        public Fish(int id)
        {
            _ageIncrement = 0.1f;
            _chanceToLive = 95;
            _id = id;
            _isDead = false;
            _age = 0f;
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