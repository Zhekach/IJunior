using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.OOP.War
{
    internal class WarProgram
    {
        public static void Main1()
        {
            Soldier testSoldier = new Soldier();
            testSoldier.PrintInfo();
        }
    }

    internal class Soldier
    {
        static Random s_random = new Random();

        private readonly Dictionary<Enum, float> _stats = new Dictionary<Enum, float>() 
        {
            { SoldierSats.Health, 0 },
            { SoldierSats.Power, 0 },
            { SoldierSats.Armor, 0 }
        };
        private readonly Dictionary<Enum, float> _statsIncrements = new Dictionary<Enum, float>()
        {
            { SoldierSats.Health, 1f },
            { SoldierSats.Power, 0.75f },
            { SoldierSats.Armor, 0.5f }
        };
        private int StatsPoints = 9;

        //protected Soldier() 
        public Soldier() 
        {
            this.DistributeStats();
            Health = _stats[SoldierSats.Health]; 
            Power = _stats[SoldierSats.Power];
            Armor = _stats[SoldierSats.Armor];
        }

        public float Health {  get; private set; }
        public float Power {  get; private set; }
        public float Armor {  get; private set; }

        private void DistributeStats()
        {
            while (StatsPoints > 0)
            {
                List<Enum> stats = new List<Enum>(_stats.Keys);
                
                Enum currentStat = stats[s_random.Next(stats.Count)];

                float currentStatValue = _stats[currentStat];
                float currentStatIncrement = _statsIncrements[currentStat];

                if(_stats.TryGetValue(currentStat, out currentStatValue))
                {
                    _stats.Remove(currentStat);
                    _stats.Add(currentStat, currentStatValue += currentStatIncrement);
                    StatsPoints--;
                }
            }
        }

        public void PrintInfo()
        {
            Console.WriteLine("Тут должен быть тип солдата");

            foreach(var stat in _stats)
            {
                Console.WriteLine($"Параметр: {stat.Key} Значение: {stat.Value}");
            }
        }
    }

    enum SoldierSats
        {
            Health = 0, 
            Power = 1,
            Armor = 2
        }
}
