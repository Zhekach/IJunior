using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.OOP.War
{
    internal class WarProgram
    {
        public static void Main()
        {
            SimpleSoldier testSoldier = new SimpleSoldier();
            testSoldier.PrintInfo();
        }
    }

    internal abstract class Soldier
    {
        static readonly Random s_random = new Random();

        private readonly Dictionary<Enum, float> _statsValues = new Dictionary<Enum, float>() 
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
        private readonly Dictionary<Enum, string> _statsTranslation = new Dictionary<Enum, string>()
        {
            { SoldierSats.Health, "Здоровье"},
            { SoldierSats.Power, "Урон"},
            { SoldierSats.Armor, "Броня"}
        };

        private int StatsPoints = 9;

        protected Soldier() 
        {
            this.DistributeStats();
            Health = _statsValues[SoldierSats.Health]; 
            Power = _statsValues[SoldierSats.Power];
            Armor = _statsValues[SoldierSats.Armor];
        }

        public float Health {  get; private set; }
        public float Power {  get; private set; }
        public float Armor {  get; private set; }

        private void DistributeStats()
        {
            while (StatsPoints > 0)
            {
                List<Enum> stats = new List<Enum>(_statsValues.Keys);
                
                Enum currentStat = stats[s_random.Next(stats.Count)];

                float currentStatValue = _statsValues[currentStat];
                float currentStatIncrement = _statsIncrements[currentStat];

                if(_statsValues.TryGetValue(currentStat, out currentStatValue))
                {
                    _statsValues.Remove(currentStat);
                    _statsValues.Add(currentStat, currentStatValue += currentStatIncrement);
                    StatsPoints--;
                }
            }
        }

        //public virtual void Attack(Fighter enemy, float damage = 0)
        //{
        //    if (damage == 0)
        //    {
        //        damage = Power;
        //    }

        //    Console.WriteLine($"{Name}: Пытается нанести {damage} урона");

        //    enemy.TakeDamage(damage);
        //}

        //protected virtual void TakeDamage(float damage)
        //{
        //    if (damage > Armor / ArmorDivider)
        //    {
        //        float damageRecieved = damage - Armor / ArmorDivider;
        //        Health -= damageRecieved;
        //        Console.WriteLine($"{Name}: Получено {damageRecieved} урона");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"{Name}: Урон не прошёл");
        //    }

        //    if (Health < 0)
        //    {
        //        Health = 0;
        //    }
        //}

        public virtual void PrintInfo()
        {
            // Сначала логично указать тип конкретного воина при переопределении метода

            foreach(var stat in _statsValues)
            {
                string statName;
                string statTranslation;

                if (_statsTranslation.TryGetValue(stat.Key, out statTranslation))
                {
                    statName = statTranslation;
                }
                else
                {
                    statName = stat.Key.ToString();
                }

                Console.WriteLine($"Параметр: {statName}. Значение: {stat.Value}");
            }
        }
    }

    internal class SimpleSoldier : Soldier
    {
        public override void PrintInfo()
        {
            Console.WriteLine("Это обычный солдат, тут не на что смотреть =(");
            base.PrintInfo();
        }
    }

    enum SoldierSats
        {
            Health = 0, 
            Power = 1,
            Armor = 2
        }
}
