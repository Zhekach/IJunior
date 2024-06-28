using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.OOP.War
{
    internal class WarProgram
    {
        public static void Main()
        {
            Soldier testSoldier1 = new SimpleSoldier(1);
            Soldier testSoldier2 = new PowerfulSoldier(2);

            testSoldier1.PrintInfo();
            testSoldier2.PrintInfo();

            Console.WriteLine();
            testSoldier1.Attack(testSoldier2);
            Console.WriteLine();
            testSoldier2.Attack(testSoldier1);

            testSoldier1.PrintInfo();
            testSoldier2.PrintInfo();
        }
    }

    internal abstract class Soldier
    {
        protected Dictionary<Enum, float> _statsValues;
        protected readonly Dictionary<Enum, float> _statsIncrements;
        protected readonly Dictionary<Enum, string> _statsTranslation;

        private int StatsPoints = 9;

        protected Soldier(int id)
        {
            _statsValues = InitiaizeStatsValues();
            _statsIncrements = InitiaizeStatsIncrements();
            _statsTranslation = InitiaizeStatsTranslators();
            this.DistributeStats();
            ID =id;
        }

        public string Type { get; protected set; }
        public int ID { get; protected set; }
        public float Health {
            get => _statsValues[SoldierStats.Health];
            set => _statsValues[SoldierStats.Health] = value;
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine();
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
                Health -= damageRecieved;

                Console.WriteLine($"{Type}{ID}: Получено {damageRecieved} урона");
            }
            else
            {
                Console.WriteLine($"{Type}{ID}: Урон не прошёл");
            }

            if (Health < 0)
            {
                Health = 0;
            }
        }

        private Dictionary<Enum, float> InitiaizeStatsValues()
        {
            Dictionary<Enum, float> result = new Dictionary<Enum, float>()
            {
                { SoldierStats.Health, 0f },
                { SoldierStats.Power, 0f },
                { SoldierStats.Armor, 0f }
            };

            return result;
        }

        private Dictionary<Enum, float> InitiaizeStatsIncrements()
        {
            Dictionary<Enum, float> result = new Dictionary<Enum, float>()
            {
                { SoldierStats.Health, 1f },
                { SoldierStats.Power, 0.75f },
                { SoldierStats.Armor, 0.5f }
            };

            return result;
        }

        private Dictionary<Enum, string> InitiaizeStatsTranslators()
        {
            Dictionary<Enum, string> result = new Dictionary<Enum, string>()
            {
                { SoldierStats.Health, "Здоровье"},
                { SoldierStats.Power, "Урон"},
                { SoldierStats.Armor, "Броня"}
            };

            return result;
        }

        private void DistributeStats()
        {
            while (StatsPoints > 0)
            {
                List<Enum> stats = new List<Enum>(_statsValues.Keys);
                Enum currentStat = stats[Util.Random.Next(stats.Count)];

                float currentStatValue = _statsValues[currentStat];
                float currentStatIncrement = _statsIncrements[currentStat];

                currentStatValue += currentStatIncrement;
                _statsValues[currentStat] = currentStatValue;

                StatsPoints--;
            }
        }
    }

    internal class SimpleSoldier : Soldier
    {
        public SimpleSoldier(int id) : base(id)
        {
            Type = "Простой";
        }
    }
    
    internal class PowerfulSoldier : Soldier
    {
        private float _powerMultiplier = 1.5f;

        public PowerfulSoldier(int id) : base(id)
        {
            Type = "Мощный";
        }

        public override void Attack(Soldier enemy, float damage = 0f)
        {
            damage = _statsValues[SoldierStats.Power] * _powerMultiplier;

            base.Attack(enemy, damage);
        }

        public override void PrintInfo()
        { 
            base.PrintInfo();

            Console.WriteLine($"Множитель урона: {_powerMultiplier}");
        }
    }

    enum SoldierStats
    {
        Health,
        Power,
        Armor

        //PowerMultiplier
        //MultipleDamage,
        //MultipleDamageRepeat
    }

    enum SoldierTypes
    {
        Simple,
        Powerful,
        MultipleUniqe,
        MultipleRepeat
    }

    internal class Util
    {
        public static Random Random = new Random();
    }
}