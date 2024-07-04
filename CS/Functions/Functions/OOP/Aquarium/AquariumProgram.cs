﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Functions.OOP.Aquarium
{
    internal class AquariumProgram
    {
        static void Main()
        {
            Aquarium aqua = new Aquarium();
            aqua.AddFish();
            aqua.AddFish();
            aqua.AddFish();
            aqua.AddFish();
            aqua.PrintInfo();

            aqua.Run();

            Console.ReadKey();
        }
    }

    internal class Aquarium
    {
        private List<Fish> _fishes;
        private int _iterationTimeMillis;
        private int _currentFishId;

        public Aquarium()
        {
            _fishes = new List<Fish>();
            _iterationTimeMillis = 1000;
            _currentFishId = 1;
            CurrentIteration = 1;
        }

        public int CurrentIteration { get; private set; }

        public void Test()
        {
            Console.WriteLine("sdfsdf");
        }
        public void Run()
        {
            TimerCallback tm = new TimerCallback(ReleaseIteration);
            Timer timer = new Timer(tm, null, 0, 2000);
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
            Console.WriteLine($"======{CurrentIteration}=====");

            foreach (Fish fish in _fishes)
            {
                fish.PrintInfo();
            }
        }

        private void ReleaseIteration(object obj)
        {
            foreach (Fish fish in _fishes)
            {
                fish.OnIteration();
            }

            CurrentIteration++;

            PrintInfo();
        }
    }

    internal class Fish
    {
        private float _ageIncrement;
        private int _chanceToLive;

        public Fish(int id)
        {
            _ageIncrement = 0.1f;
            _chanceToLive = 95;
            Id = id;
            IsDead = false;
            Age = 0f;
            OnIteration += IncrementAge;
            OnIteration += CheckDead;
        }

        public int Id { get; private set; }
        public bool IsDead { get; private set; }
        public float Age { get; private set; }
        public IterationReaction OnIteration { get; private set; }


        public delegate void IterationReaction();

        public void PrintInfo()
        {
            string aliveText = "жива";
            string deadText = "не очень";
            string statusText = aliveText;

            if (IsDead) { statusText = deadText; } 

            Console.WriteLine($"Рыбка под номером {Id}: {statusText}." +
                $" Ей {Age} рыбьих лет");
        }

        private void IncrementAge()
        {
            if (IsDead == false)
            {
                Age += _ageIncrement;
            }
        }

        private void CheckDead()
        {
            if (Util.GetRandomInt(100) > _chanceToLive)
            {
                IsDead = true;
            }
        }
    }

    internal class Util
    {
        private static Random _random = new Random();

        public static int GetRandomInt(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}
