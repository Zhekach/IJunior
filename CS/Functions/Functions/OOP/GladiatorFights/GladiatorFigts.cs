using Functions.OOP.CasinoProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        private bool _isUserExited = false;
        private Fighter _fighterFirst;
        private Fighter _fighterSecond;
        private Dictionary<int, Fighter> _fightersDictionary;

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
            while (_isUserExited == false)
            {
                PrintUI();

                int userInput = ReadInt();

                switch (userInput)
                {
                    case (int)UserCommands.SelectFirstFighter:
                        _fighterFirst = SelectFighter();
                        break;
                    case (int)UserCommands.SelectSecondFighter:
                        _fighterSecond = SelectFighter();
                        break;
                    case (int)UserCommands.StartBattle:
                        StartBattle(_fighterFirst, _fighterSecond);
                        break;
                    case (int)UserCommands.Exit:
                        _isUserExited = true;
                        break;
                    default:
                        Console.WriteLine("Вы ввели неверную команду. Попробуйте снова.");
                        break;
                }
            }
        }

        private void PrintUI()
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

        private Fighter SelectFighter()
        {
            Fighter fighter;

            Console.Clear();
            Console.WriteLine("Введите номер типа бойца:");

            foreach(KeyValuePair<int, Fighter> pair in _fightersDictionary)
            {
                Console.WriteLine($"Номер {pair.Key} - {pair.Value.ClassName}");
            }


            int userInput = ReadInt();

            if(_fightersDictionary.ContainsKey(userInput))
            {
                fighter = _fightersDictionary[userInput]; 
            }
            else
            {
                Console.WriteLine("Введено некорректное значение, будет рыцарь)");
                fighter = new Knight();
            }
            switch (userInput)
            {
                case (int)FighterClasses.Knight:
                    fighter = new Knight();
                    break;
                case (int)FighterClasses.Guardian:
                    fighter = new Guardian();
                    break;
                case (int)FighterClasses.Assassin:
                    fighter = new Assassin();
                    break;
                case (int)FighterClasses.Healer:
                    fighter = new Healer();
                    break;
                case (int)FighterClasses.Warlock:
                    fighter = new Warlock();
                    break;
                default:
                    fighter = new Knight();
                    Console.WriteLine("Введено некорректное значение, будет рыцарь)");
                    break;
            }

            return fighter;
        }

        private void StartBattle(Fighter fighter1, Fighter fighter2)
        {
            Console.Clear();

            _fighterFirst.PrintInfo();
            _fighterSecond.PrintInfo();

            while (fighter1.Health > 0 && fighter2.Health > 0)
            {
                Console.WriteLine("\n-====Новый ход!====-\n");

                fighter1.CauseClassDamage(fighter2);
                if (fighter2.Health > 0)
                {
                    fighter2.CauseClassDamage(fighter1);
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
        private int _baseHealthValue = 45;
        private int _basePowerValue = 15;
        private int _baseArmorValue = 30;
        protected float _healthMax;
        protected float _power;
        protected float _armor;
        protected float _armorDivider = 4;
        protected float _damage;

        public string ClassName { get; protected set; }
        public float Health { get; protected set; }

        protected Fighter()
        {
            Health = _baseHealthValue;
            _healthMax = Health;
            _power = _basePowerValue;
            _armor = _baseArmorValue;
        }

        public abstract void PrintInfo();

        public void PrintHPInfo()
        {
            Console.WriteLine($"{ClassName}: Уровень жизни {Health} ");
        }

        protected void CauseBaseDamage(Fighter enemy, float damage = 0)
        {
            if (damage == 0)
            {
                damage = _power;
            }

            Console.WriteLine($"{ClassName}: Пытается нанести {damage} урона");

            enemy.TakeClassDamage(damage);
        }

        public abstract void CauseClassDamage(Fighter enemy);

        public void TakeBaseDamage(float damage)
        {
            if (damage > _armor / _armorDivider)
            {
                Health -= (damage - _armor / _armorDivider);
                Console.WriteLine($"{ClassName}: Получено {damage - _armor / _armorDivider} урона");
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

        public abstract void TakeClassDamage(float damage);
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
                              $"Сила: {_power}\n" +
                              $"Защита: {_armor}");
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

            _power += _powerBonus;
            Health += _healthBonus;
        }

        public override void CauseClassDamage(Fighter enemy)
        {
            float damage;

            if (_random.Next(_criticalChanceMax) < _criticalChance)
            {
                damage = _power * _criticalMultiplier;
                Console.WriteLine($"{ClassName}: Критическая атака");
            }
            else
            {
                damage = _power;
            }

            CauseBaseDamage(enemy, damage);
        }

        public override void TakeClassDamage(float damage)
        {
            TakeBaseDamage(damage);
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

            _armor += _armorBonus;
            Health += _healthBonus;
        }

        public override void CauseClassDamage(Fighter enemy)
        {
            CauseBaseDamage(enemy, _power);
        }

        public override void TakeClassDamage(float damage)
        {
            if (_random.Next(_criticalChanceMax) < _criticalChance)
            {
                damage = 0;
                Console.WriteLine($"{ClassName}: Блок атаки");
            }
            else if (damage > _armor / _armorDivider)
            {
                TakeBaseDamage(damage);
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
            _power += _powerBonus;
            _armor += _armorBonus;
        }

        public override void CauseClassDamage(Fighter enemy)
        {
            CauseBaseDamage(enemy, _power);
        }

        public override void TakeClassDamage(float damage)
        {
            if (_random.Next(_baseBonusValue) < _criticalChance)
            {
                damage = 0;
                Console.WriteLine($"{ClassName}: Уворот от атаки");
            }
            else if (damage > _armor / _armorDivider)
            {
                TakeBaseDamage(damage);
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
                              $"Сила: {_power}\n" +
                              $"Защита: {_armor}");
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

        public override void CauseClassDamage(Fighter enemy)
        {
            CauseBaseDamage(enemy, _power);
        }

        public override void TakeClassDamage(float damage)
        {
            TakeBaseDamage(damage);

            if (_healthMax - Health >= _powerMagical)
            {
                if (Health > 0 && _mana >= _spellManaPrice)
                {
                    _mana -= _spellManaPrice;
                    Health += _powerMagical;

                    Console.WriteLine($"{ClassName}: Вылечено {_powerMagical} жизни");

                    if (Health > _healthMax)
                    {
                        Health = _healthMax;
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

        public override void CauseClassDamage(Fighter enemy)
        {
            float commonDamage;

            if (_mana >= _spellManaPrice)
            {
                _mana -= _spellManaPrice;
                commonDamage = _power + _powerMagical;
            }
            else
            {
                commonDamage = _power;
            }

            CauseBaseDamage(enemy, commonDamage);
        }

        public override void TakeClassDamage(float damage)
        {
            TakeBaseDamage(damage);
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