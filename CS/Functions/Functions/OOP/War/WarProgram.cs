using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
///TODO add IMultiAtackable for soldiers
///!!!!!Добавить солдат с атакой нескольких врагов!!!!
///Сначала без повторения
///Тест
///
/// Добавить метод удаления мертвых солдат после получения урона отрядом
/// 
///
/// Потом с повторением
///!!!!!Прописать их в рандомной генерации!!!!!
///И всё заново
///
///

namespace Functions.OOP.War
{
    internal class WarProgram
    {
        private Squad squad1;
        private Squad squad2;

        public static void Main()
        {
            WarProgram war = new WarProgram();

            war.StartDeathBattle();

            war.PrintWinnerName();
        }

        public WarProgram()
        {
            InicializeSquads();
        }

        private void InicializeSquads()
        {
            squad1 = new Squad(3, "Остолопы");
            squad2 = new Squad(2, "Бронтозавры");

            squad1.PrintInfo(true);
            Console.WriteLine();

            squad2.PrintInfo(true);
            Console.WriteLine();
        }

        private void StartDeathBattle()
        {
            int roundCounter = 1;
            while (squad1.HasSurvivor() && squad2.HasSurvivor())
            {
                Console.WriteLine($"=================" +
                    $"Раунд № {roundCounter}" +
                    $"=================");

                squad1.Attack(squad2);
                squad2.RemoveDeadSoldiers();
                squad2.PrintInfo();

                squad2.Attack(squad1);
                squad1.RemoveDeadSoldiers();
                squad1.PrintInfo();

                roundCounter++;
            }
        }

        private void PrintWinnerName()
        {
            Console.WriteLine("Победитель:");

            if (squad1.HasSurvivor())
            {
                Console.WriteLine(squad1.Name);
            }
            else
            {
                Console.WriteLine(squad2.Name);
            }
        }
    }

    internal class Squad
    {
        private int _size;
        private List<Soldier> _soldiers;
        private Squad _enemies;
        private string _name;

        public Squad(int size, string name)
        {
            _size = size;
            _soldiers = AddRandomSoldiers(_size);
            _name = name;
        }

        public string Name { get { return _name; } }

        public IEnumerable<Soldier> Soldiers { get => _soldiers; }

        public void Attack(Squad enemies)
        {
            _enemies = enemies;

            foreach (Soldier soldier in _soldiers)
            {
                if (soldier is IMultiAtackable)
                {
                    IMultiAtackable multiAtackableSoldier = (IMultiAtackable)soldier;
                    multiAtackableSoldier.AttackMultiple(enemies._soldiers);
                }
                else
                {
                    Soldier enemy = ChooseOneEnemy();

                    if (enemy != null)
                    {
                        soldier.Attack(enemy);
                    }
                }
            }
        }

        public void PrintInfo(bool isFullInfo = false)
        {
            Console.WriteLine($"Отряд: {_name}\n" +
                $"В составе:");

            foreach (Soldier soldier in _soldiers)
            {
                if (isFullInfo)
                {
                    soldier.PrintInfo(true);
                }
                else
                {
                    soldier.PrintInfo();
                }
            }
        }

        public void RemoveDeadSoldiers()
        {
            _soldiers.RemoveAll(Soldier.IsDead);
        }

        public bool HasSurvivor()
        {
            return _soldiers.Exists(Soldier.IsAlive);
        }

        private Soldier ChooseOneEnemy()
        {
            foreach (Soldier enemy in _enemies.Soldiers)
            {
                if (enemy.Health > 0)
                {
                    return enemy;
                }
            }

            return null;
        }

        private List<Soldier> ChooseSeveralEnemies()
        {
            List<Soldier> soldiers = new List<Soldier>();

            foreach (Soldier enemy in _enemies.Soldiers)
            {
                if (enemy.Health > 0)
                {
                    soldiers.Add(enemy);
                }
            }

            return soldiers;
        }

        private List<Soldier> AddRandomSoldiers(int size)
        {
            List<Soldier> soldiersResult = new List<Soldier>();

            for (int i = 0; i < size; i++)
            {
                soldiersResult.Add(CreateRandomSoldier(i + 1));
            }

            return soldiersResult;
        }

        private Soldier CreateRandomSoldier(int counter)
        {
            Soldier soldierResult;
            List<SoldierTypes> soldierTypes = Enum.GetValues(typeof(SoldierTypes)).Cast<SoldierTypes>().ToList();

            SoldierTypes randomSoldierType = soldierTypes[Util.Random.Next(soldierTypes.Count)];

            switch (randomSoldierType)
            {
                case (SoldierTypes.Simple):
                    soldierResult = new SimpleSoldier(counter);
                    break;
                case (SoldierTypes.Powerful):
                    soldierResult = new PowerfulSoldier(counter);
                    break;
                case (SoldierTypes.MultipleUniqe):
                    soldierResult = new MultipleUniqueSoldier(counter);
                    break;
                case (SoldierTypes.MultipleRepeat):
                    soldierResult = new MultipleRepeatSoldier(counter);
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

        public static bool IsDead(Soldier soldier)
        {
            return soldier.Health <= 0;
        }

        public static bool IsAlive(Soldier soldier)
        {
            return soldier.Health > 0;
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
                { SoldierStats.Health, 1f },
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
        private int _maxEnemiesCount;

        public MultipleUniqueSoldier(int id) : base(id)
        {
            Type = "Атакует группу, единожды";
            _maxEnemiesCount = 2;
        }

        public void AttackMultiple(List<Soldier> list)
        {
            int enemiesCounter = _maxEnemiesCount;
            List<Soldier> attackedEnemies = new List<Soldier>();

            foreach (Soldier soldier in list)
            {
                if (attackedEnemies.Contains(soldier) == false && enemiesCounter > 0)
                {
                    Attack(soldier);
                    attackedEnemies.Add(soldier);
                    enemiesCounter--;
                }
            }
        }
    }

    internal class MultipleRepeatSoldier : Soldier, IMultiAtackable
    {
        private int _maxEnemiesCount;

        public MultipleRepeatSoldier(int id) : base(id)
        {
            Type = "Атакует группу, повторно";
            _maxEnemiesCount = 5;
        }

        public void AttackMultiple(List<Soldier> list)
        {
            int enemiesCounter = _maxEnemiesCount;

            while (enemiesCounter > 0)
            {
                foreach (Soldier soldier in list)
                {
                    if (enemiesCounter > 0)
                    {
                        Attack(soldier);
                        enemiesCounter--;
                    }
                }
            }
        }
    }

    internal interface IMultiAtackable
    {
        void AttackMultiple(List<Soldier> list);
    }

    enum SoldierStats
    {
        Health,
        Power,
        Armor
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