using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
///TODO add IMultiAtackable for soldiers
///!!!!!Добавить солдат с атакой нескольких врагов!!!!
///!!!!!Прописать их в рандомной генерации!!!!!
///Написать механику для солдат с множественной атакой
///

namespace Functions.OOP.War
{
    internal class WarProgram
    {
        public static void Main1()
        {
            Soldier testSoldier1 = new SimpleSoldier(1);
            Soldier testSoldier2 = new PowerfulSoldier(2);

            testSoldier1.PrintInfo(true);
            testSoldier2.PrintInfo(true);

            Console.WriteLine();
            testSoldier1.Attack(testSoldier2);
            Console.WriteLine();
            testSoldier2.Attack(testSoldier1);

            testSoldier1.PrintInfo();
            testSoldier2.PrintInfo();
        }
    }

    internal class Squad
    {
        private int _size;
        private List<Soldier> _soldiers;

        public Squad(int size)
        {
            _size = size;
            _soldiers = AddRandomSoldiers(_size);
        }

        private List<Soldier> AddRandomSoldiers(int size)
        {
            List<Soldier> soldiersResult = new List<Soldier>();

            for (int i = 0; i < size; i++)
            {
                soldiersResult.Add(CreateRandomSoldier());
            }

            return soldiersResult;
        }

        private Soldier CreateRandomSoldier()
        {
            Soldier soldierResult;
            int counter = 0;
            List<SoldierTypes> soldierTypes = Enum.GetValues(typeof(SoldierTypes)).Cast<SoldierTypes>().ToList();

            SoldierTypes randomSoldierType = soldierTypes[Util.Random.Next(soldierTypes.Count)];

            switch (randomSoldierType)
            {
                case (SoldierTypes.Simple):
                    soldierResult = new SimpleSoldier(counter);
                    counter++;
                    break;
                case (SoldierTypes.Powerful):
                    soldierResult = new PowerfulSoldier(counter);
                    counter++;
                    break;
                default:
                    soldierResult = new SimpleSoldier(counter);

                    Console.WriteLine("Произошла ошибка выбора типа случайного солдата");

                    return soldierResult;
            }

            return soldierResult;
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
            ID = id;
        }

        public string Type { get; protected set; }
        public int ID { get; protected set; }
        public float Health
        {
            get => _statsValues[SoldierStats.Health];
            set => _statsValues[SoldierStats.Health] = value;
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

        public virtual void PrintInfo(bool isFullInfo = false)
        {
            Console.WriteLine();
            Console.WriteLine($"Солдат типа: {Type}, номер {ID}");

            if (isFullInfo == false)
            {
                Console.WriteLine($"Здоровье - {Health}");
            }
            else
            {
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

        public override void PrintInfo(bool isFullInfo = false)
        {
            base.PrintInfo(isFullInfo);

            if (isFullInfo)
            {
                Console.WriteLine($"Параметр: Множитель урона. {_powerMultiplier}");
            }
        }
    }

    internal class MultipleUniqueSoldier : Soldier, IMultiAtackable
    {
        private bool _isMultipleRepeat;

        public MultipleUniqueSoldier(int id) : base(id)
        {
            Type = "Атакует группу, единожды";
        }

        public bool IsMultipleRepeat { get => _isMultipleRepeat; }

        public override void Attack(Soldier enemy, float damage = 0)
        {
            base.Attack(enemy, damage);
        }

        public void AttackMultiple(List<Soldier> list)
        {

        }
    }

    internal interface IMultiAtackable
    {
        bool IsMultipleRepeat { get; }

        void AttackMultiple(List<Soldier> list);
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