using System;
using System.Threading;
using System.Collections.Generic;

namespace Functions.OOP.GladiatorFights
{
    internal class GladiatorFigts
    {
        static void Main()
        {
            Arena arena = new Arena();
            arena.Run();
        }
    }

    class Arena
    {
        private Fighter _fighterFirst;
        private Fighter _fighterSecond;
        private readonly Dictionary<int, Fighter> _fightersDictionary;

        public Arena()
        {
            _fightersDictionary = new Dictionary<int, Fighter>
            {
                { 1, new Knight() },
                { 2, new Guardian() },
                { 3, new Assassin() },
                { 4, new Healer() },
                { 5, new Warlock() }
            };
        }


        public void Run()
        {
            bool isUserExited = false;

            while (isUserExited == false)
            {
                PrintUi();

                int userInput = ReadInt();

                switch (userInput)
                {
                    case (int)UserCommands.SelectFirstFighter:
                        _fighterFirst = CreateFighter();
                        break;
                    case (int)UserCommands.SelectSecondFighter:
                        _fighterSecond = CreateFighter();
                        break;
                    case (int)UserCommands.StartBattle:
                        PerformBattle(_fighterFirst, _fighterSecond);
                        break;
                    case (int)UserCommands.Exit:
                        isUserExited = true;
                        break;
                    default:
                        Console.WriteLine("Вы ввели неверную команду. Попробуйте снова.");
                        break;
                }
            }
        }

        private void PrintUi()
        {
            Console.Clear();
            Console.WriteLine("Введите команду:\n" +
                             $"{(int)UserCommands.SelectFirstFighter} - выбрать первого бойца\n" +
                             $"{(int)UserCommands.SelectSecondFighter} - выбрать второго бойца\n" +
                             $"{(int)UserCommands.StartBattle} - начать бой\n" +
                             $"{(int)UserCommands.Exit} - выйти из программы\n");
            if (_fighterFirst != null)
            {
                Console.WriteLine($"Первый боец - {_fighterFirst.ClassName}");
            }

            if (_fighterSecond != null)
            {
                Console.WriteLine($"Второй боец - {_fighterSecond.ClassName}");
            }
        }

        private Fighter CreateFighter()
        {
            Fighter fighter;

            Console.Clear();
            Console.WriteLine("Введите номер типа бойца:");

            foreach (KeyValuePair<int, Fighter> pair in _fightersDictionary)
            {
                Console.WriteLine($"Номер {pair.Key} - {pair.Value.ClassName}");
            }


            int userInput = ReadInt();

            if (_fightersDictionary.ContainsKey(userInput))
            {
                fighter = _fightersDictionary[userInput].Clone();
            }
            else
            {
                Console.WriteLine("Введено некорректное значение, будет рыцарь)");
                fighter = new Knight();
            }

            return fighter;
        }

        private void PerformBattle(Fighter fighter1, Fighter fighter2)
        {
            Console.Clear();

            if (fighter1 == null || fighter2 == null)
            {
                Console.WriteLine("Бойцы не выбраны. Сделайте выбор снова.\n" +
                                  "Для продолжения нажмите любую кнопку");
                Console.ReadKey();

                return;
            }

            _fighterFirst.PrintInfo();
            _fighterSecond.PrintInfo();

            while (fighter1.Health > 0 && fighter2.Health > 0)
            {
                Console.WriteLine("\n-====Новый ход!====-\n");

                fighter1.Attack(fighter2);
                if (fighter2.Health > 0)
                {
                    fighter2.Attack(fighter1);
                }

                Console.WriteLine("---Итоги---");

                fighter1.PrintHPInfo();
                fighter2.PrintHPInfo();

                Thread.Sleep(5000);
            }

            Console.WriteLine("Для продолжения нажмите любую кнопку");
            Console.ReadKey();
        }

        private int ReadInt()
        {
            bool isIntEntered = false;
            int parsedInt = 0;

            while (isIntEntered == false)
            {
                string enteredString;

                Console.WriteLine("Введите число:");
                enteredString = Console.ReadLine();

                isIntEntered = int.TryParse(enteredString, out parsedInt);

                if (isIntEntered)
                {
                    Console.WriteLine($"Введенное число распознано: {parsedInt}");
                }
                else
                {
                    Console.WriteLine("Вы ввели некорректное число. Попробуйте ещё.\n");
                }
            }

            return parsedInt;
        }
    }

    abstract class Fighter
    {
        private readonly int _baseHealthValue = 45;
        private readonly int _basePowerValue = 15;
        private readonly int _baseArmorValue = 30;
        protected float HealthMax;
        protected float Power;
        protected float Armor;
        protected float ArmorDivider = 4;
        protected float Damage;

        public string ClassName { get; protected set; }
        public float Health { get; protected set; }

        protected Fighter()
        {
            Health = _baseHealthValue;
            HealthMax = Health;
            Power = _basePowerValue;
            Armor = _baseArmorValue;
        }

        public abstract void PrintInfo();

        public void PrintHPInfo()
        {
            Console.WriteLine($"{ClassName}: Уровень жизни {Health} ");
        }

        public abstract Fighter Clone();

        public abstract void Attack(Fighter enemy);

        public abstract void TakeDamageClass(float damage);

        protected void AttackBase(Fighter enemy, float damage = 0)
        {
            if (damage == 0)
            {
                damage = Power;
            }

            Console.WriteLine($"{ClassName}: Пытается нанести {damage} урона");

            enemy.TakeDamageClass(damage);
        }

        protected void TakeDamageBase(float damage)
        {
            if (damage > Armor / ArmorDivider)
            {
                Health -= (damage - Armor / ArmorDivider);
                Console.WriteLine($"{ClassName}: Получено {damage - Armor / ArmorDivider} урона");
            }
            else
            {
                Console.WriteLine($"{ClassName}: Урон не прошёл");
            }

            if (Health < 0)
            {
                Health = 0;
            }
        }
    }

    abstract class Warrior : Fighter
    {
        protected Random _random = new Random();
        protected int _baseBonusValue = 50;
        protected float _bonusParameter;
        protected float _bonusMultiplier = 0.33f;

        protected float _healthBonus;
        protected float _powerBonus;
        protected float _armorBonus;

        protected string _criticalDescription;
        protected float _criticalChance;
        protected int _criticalChanceMax = 100;
        protected float _criticalMultiplier = 2;

        protected Warrior() : base()
        {
            _bonusParameter = _random.Next(_baseBonusValue);
            _criticalChance = _baseBonusValue - _bonusParameter;
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"Класс: {ClassName}\n" +
                              $"Жизнь: {Health}\n" +
                              $"Сила: {Power}\n" +
                              $"Защита: {Armor}");
            Console.WriteLine($"{_criticalDescription}: {_criticalChance}");
            Console.WriteLine("=========");
        }
    }

    class Knight : Warrior
    {
        public Knight() : base()
        {
            ClassName = "Рыцарь";
            _criticalDescription = "Шанс крита";

            _powerBonus = _bonusParameter * _bonusMultiplier * 2;
            _healthBonus = _bonusParameter * _bonusMultiplier;

            Power += _powerBonus;
            Health += _healthBonus;
        }

        public override Fighter Clone()
        {
            return new Knight();
        }

        public override void Attack(Fighter enemy)
        {
            float damage;

            if (_random.Next(_criticalChanceMax) < _criticalChance)
            {
                damage = Power * _criticalMultiplier;
                Console.WriteLine($"{ClassName}: Критическая атака");
            }
            else
            {
                damage = Power;
            }

            AttackBase(enemy, damage);
        }

        public override void TakeDamageClass(float damage)
        {
            TakeDamageBase(damage);
        }
    }

    class Guardian : Warrior
    {
        public Guardian() : base()
        {
            ClassName = "Защитник";
            _criticalDescription = "Шанс блока";

            _armorBonus = _bonusParameter * _bonusMultiplier * 2;
            _healthBonus = _bonusParameter * _bonusMultiplier;

            Armor += _armorBonus;
            Health += _healthBonus;
        }

        public override Fighter Clone()
        {
            return new Guardian();
        }

        public override void Attack(Fighter enemy)
        {
            AttackBase(enemy, Power);
        }

        public override void TakeDamageClass(float damage)
        {
            if (_random.Next(_criticalChanceMax) < _criticalChance)
            {
                Console.WriteLine($"{ClassName}: Блок атаки");
            }
            else if (damage > Armor / ArmorDivider)
            {
                TakeDamageBase(damage);
            }
        }
    }

    class Assassin : Warrior
    {
        public Assassin() : base()
        {
            ClassName = "Ассасин";
            _criticalDescription = "Шанс уворота";

            _healthBonus = _bonusParameter * _bonusMultiplier;
            _powerBonus = _bonusParameter * _bonusMultiplier;
            _armorBonus = _bonusParameter * _bonusMultiplier;

            Health += _healthBonus;
            Power += _powerBonus;
            Armor += _armorBonus;
        }

        public override Fighter Clone()
        {
            return new Assassin();
        }

        public override void Attack(Fighter enemy)
        {
            AttackBase(enemy, Power);
        }

        public override void TakeDamageClass(float damage)
        {
            if (_random.Next(_baseBonusValue) < _criticalChance)
            {
                Console.WriteLine($"{ClassName}: Уворот от атаки");
            }
            else if (damage > Armor / ArmorDivider)
            {
                TakeDamageBase(damage);
            }
        }
    }

    abstract class Wizzard : Fighter
    {
        protected float _manaBaseValue = 40;
        protected float _mana;
        protected float _powerMagical = 15;
        protected float _spellManaPrice = 10;
        protected string _spellDescription;

        protected Wizzard() : base()
        {
            _mana = _manaBaseValue;
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"Класс: {ClassName}\n" +
                              $"Жизнь: {Health}\n" +
                              $"Сила: {Power}\n" +
                              $"Защита: {Armor}");
            Console.WriteLine($"Уровень маны: {_mana}");
            Console.WriteLine($"Сила заклинаний: {_powerMagical}");
            Console.WriteLine("=========");
        }
    }

    class Healer : Wizzard
    {
        public Healer() : base()
        {
            ClassName = "Лекарь";
            _spellDescription = "Лечение себя";
        }

        public override Fighter Clone()
        {
            return new Healer();
        }

        public override void Attack(Fighter enemy)
        {
            AttackBase(enemy, Power);
        }

        public override void TakeDamageClass(float damage)
        {
            TakeDamageBase(damage);

            if (HealthMax - Health >= _powerMagical)
            {
                if (Health > 0 && _mana >= _spellManaPrice)
                {
                    _mana -= _spellManaPrice;
                    Health += _powerMagical;

                    Console.WriteLine($"{ClassName}: Вылечено {_powerMagical} жизни");

                    if (Health > HealthMax)
                    {
                        Health = HealthMax;
                    }
                }
            }
        }
    }

    class Warlock : Wizzard
    {
        public Warlock() : base()
        {
            ClassName = "Боевой маг";
            _spellDescription = "Урон магией";
        }

        public override Fighter Clone()
        {
            return new Warlock();
        }

        public override void Attack(Fighter enemy)
        {
            float commonDamage;

            if (_mana >= _spellManaPrice)
            {
                _mana -= _spellManaPrice;
                commonDamage = Power + _powerMagical;
            }
            else
            {
                commonDamage = Power;
            }

            AttackBase(enemy, commonDamage);
        }

        public override void TakeDamageClass(float damage)
        {
            TakeDamageBase(damage);
        }
    }

    public enum FighterClasses
    {
        Knight = 1,
        Guardian = 2,
        Assassin = 3,
        Healer = 4,
        Warlock = 5
    }

    public enum UserCommands
    {
        SelectFirstFighter = 1,
        SelectSecondFighter = 2,
        StartBattle = 3,
        Exit = 4
    }
}