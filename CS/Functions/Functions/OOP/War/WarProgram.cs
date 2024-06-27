using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.OOP.War
{
    internal class WarProgram
    {
        public static void Main1()
        {
            SimpleSoldier testSoldier1 = new SimpleSoldier(1);
            SimpleSoldier testSoldier2 = new SimpleSoldier(2);

            testSoldier1.PrintInfo();
            testSoldier2.PrintInfo();

            testSoldier1.Attack(testSoldier2);
            testSoldier2.Attack(testSoldier1);

            testSoldier1.PrintInfo();
            testSoldier2.PrintInfo();
        }
    }

    internal abstract class Soldier
    {
        static readonly Random s_random = new Random();

        private Dictionary<Enum, float> _statsValues = new Dictionary<Enum, float>()
        {
            { SoldierStats.Health, 0 },
            { SoldierStats.Power, 0 },
            { SoldierStats.Armor, 0 }
        };
        private readonly Dictionary<Enum, float> _statsIncrements = new Dictionary<Enum, float>()
        {
            { SoldierStats.Health, 1f },
            { SoldierStats.Power, 0.75f },
            { SoldierStats.Armor, 0.5f }
        };
        private readonly Dictionary<Enum, string> _statsTranslation = new Dictionary<Enum, string>()
        {
            { SoldierStats.Health, "Здоровье"},
            { SoldierStats.Power, "Урон"},
            { SoldierStats.Armor, "Броня"}
        };

        private int StatsPoints = 9;

        protected Soldier()
        {
            this.DistributeStats();
        }

        public string Type { get; protected set; }
        public int ID { get; protected set; }
        public float Health { get => _statsValues[SoldierStats.Health];}

        private void DistributeStats()
        {
            while (StatsPoints > 0)
            {
                List<Enum> stats = new List<Enum>(_statsValues.Keys);

                Enum currentStat = stats[s_random.Next(stats.Count)];

                float currentStatValue = _statsValues[currentStat];
                float currentStatIncrement = _statsIncrements[currentStat];

                if (_statsValues.TryGetValue(currentStat, out currentStatValue))
                {
                    _statsValues.Remove(currentStat);
                    _statsValues.Add(currentStat, currentStatValue += currentStatIncrement);
                    StatsPoints--;
                }
            }
        }

        public virtual void Attack(Soldier enemy, float damage = 0)
        {
            if (damage == 0)
            {
                damage = _statsValues[SoldierStats.Power];
            }

            Console.WriteLine($"{Type}{ID}: Пытается нанести {damage} урона");

            enemy.TakeDamage(damage);
        }

        protected virtual void TakeDamage(float damage)
        {
            if (damage > _statsValues[SoldierStats.Armor])
            {
                float damageRecieved = damage - _statsValues[SoldierStats.Armor];
                _statsValues[SoldierStats.Health] -= damageRecieved;
                Console.WriteLine($"{Type}{ID}: Получено {damageRecieved} урона");
            }
            else
            {
                Console.WriteLine($"{Type}{ID}: Урон не прошёл");
            }

            if (_statsValues[SoldierStats.Health] < 0)
            {
                _statsValues[SoldierStats.Health] = 0;
            }
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine($"Солдат типа: {Type}, номер {ID}");

            foreach (var stat in _statsValues)
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

            Console.WriteLine("");
        }
    }

    internal class SimpleSoldier : Soldier
    {
        public SimpleSoldier(int id)
        {
            ID = id;
            Type = "Простой";
        }
    }

    enum SoldierStats
    {
        Health = 0,
        Power = 1,
        Armor = 2
    }
}
