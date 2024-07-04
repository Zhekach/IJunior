using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.OOP.Aquarium
{
    internal class AquariumProgram
    {
        public void Main()
        {

        }
    }

    internal class Aquarium
    {
        private List<Fish> _fishes;
        private int _iterationTimeMillis;

        public Aquarium()
        {
            _fishes = new List<Fish>();
            _iterationTimeMillis = 1000;
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
        }

        public int Id { get; private set; }
        public bool IsDead { get; private set; }
        public float Age { get; private set; }

        delegate void OnIteration();

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
