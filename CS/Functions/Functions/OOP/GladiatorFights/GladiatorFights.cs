using System;
using System.Collections.Generic;
using System.Threading;

namespace Functions.OOP.GladiatorFights
{
    internal static class GladiatorFights
    {
        private static void Main()
        {
            Arena arena = new Arena();
            arena.Run();
        }
    }

    internal class Arena
    {
        private readonly List<Fighter> _fightersList;

        private Fighter _fighterFirst;
        private Fighter _fighterSecond;

        public Arena()
        {
            _fightersList = new List<Fighter>
            {
                { new Knight() },
                { new Guardian() },
                { new Assassin() },
                { new Healer() },
                { new Warlock() }
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
                Console.WriteLine($"Первый боец - {_fighterFirst.Name}");
            }

            if (_fighterSecond != null)
            {
                Console.WriteLine($"Второй боец - {_fighterSecond.Name}");
            }
        }

        private Fighter CreateFighter()
        {
            Fighter newFighter;

            Console.Clear();
            Console.WriteLine("Введите номер типа бойца:");

            foreach (Fighter fighter in _fightersList)
            {
                int index = _fightersList.IndexOf(fighter) + 1;
                Console.WriteLine($"Номер {index} - {fighter.Name}");
            }

            int userInput = ReadInt();

            if (userInput <= _fightersList.Count && userInput > 0)
            {
                newFighter = _fightersList[userInput - 1].Clone();
            }
            else
            {
                Console.WriteLine("Введено некорректное значение, будет рыцарь)");

                newFighter = new Knight();

                Console.WriteLine("Для продолжения нажмите любую клавишу");
                Console.ReadKey();
            }

            return newFighter;
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

                fighter1.PrintHealthInfo();
                fighter2.PrintHealthInfo();

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
                Console.WriteLine("Введите число:");
                string enteredString = Console.ReadLine();

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

    internal abstract class Fighter
    {
        protected readonly float ArmorDivider = 4;
        protected readonly float HealthMax;
        protected float Armor;
        protected float Power;
        private readonly int _baseArmorValue = 30;
        private readonly int _baseHealthValue = 45;
        private readonly int _basePowerValue = 15;

        protected Fighter()
        {
            Health = _baseHealthValue;
            HealthMax = Health;
            Power = _basePowerValue;
            Armor = _baseArmorValue;
        }

        public string Name { get; protected set; }
        public float Health { get; protected set; }

        public abstract void PrintInfo();

        public void PrintHealthInfo()
        {
            Console.WriteLine($"{Name}: Уровень жизни {Health} ");
        }

        public abstract Fighter Clone();

        public abstract void Attack(Fighter enemy);

        protected abstract void TakeDamage(float damage);

        protected void AttackBase(Fighter enemy, float damage = 0)
        {
            if (damage == 0)
            {
                damage = Power;
            }

            Console.WriteLine($"{Name}: Пытается нанести {damage} урона");

            enemy.TakeDamage(damage);
        }

        protected void TakeDamageBase(float damage)
        {
            if (damage > Armor / ArmorDivider)
            {
                float damageRecieved = damage - Armor / ArmorDivider;
                Health -= damageRecieved;
                Console.WriteLine($"{Name}: Получено {damageRecieved} урона");
            }
            else
            {
                Console.WriteLine($"{Name}: Урон не прошёл");
            }

            if (Health < 0)
            {
                Health = 0;
            }
        }
    }

    internal abstract class Warrior : Fighter
    {
        protected readonly int BaseBonusValue = 50;
        protected readonly float BonusMultiplier = 0.33f;
        protected readonly float BonusParameter;
        protected readonly float CriticalChance;
        protected readonly int CriticalChanceMax = 100;
        protected readonly float CriticalMultiplier = 2;
        protected readonly Random Random = new Random();
        protected float ArmorBonus;

        protected string CriticalDescription;

        protected float HealthBonus;
        protected float PowerBonus;

        protected Warrior()
        {
            BonusParameter = Random.Next(BaseBonusValue);
            CriticalChance = BaseBonusValue - BonusParameter;
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"Класс: {Name}\n" +
                              $"Жизнь: {Health}\n" +
                              $"Сила: {Power}\n" +
                              $"Защита: {Armor}");
            Console.WriteLine($"{CriticalDescription}: {CriticalChance}");
            Console.WriteLine("=========");
        }
    }

    internal class Knight : Warrior
    {
        public Knight()
        {
            Name = "Рыцарь";
            CriticalDescription = "Шанс крита";

            PowerBonus = BonusParameter * BonusMultiplier * 2;
            HealthBonus = BonusParameter * BonusMultiplier;

            Power += PowerBonus;
            Health += HealthBonus;
        }

        public override Fighter Clone()
        {
            return new Knight();
        }

        public override void Attack(Fighter enemy)
        {
            float damage;

            if (Random.Next(CriticalChanceMax) < CriticalChance)
            {
                damage = Power * CriticalMultiplier;
                Console.WriteLine($"{Name}: Критическая атака");
            }
            else
            {
                damage = Power;
            }

            AttackBase(enemy, damage);
        }

        protected override void TakeDamage(float damage)
        {
            TakeDamageBase(damage);
        }
    }

    internal class Guardian : Warrior
    {
        public Guardian()
        {
            Name = "Защитник";
            CriticalDescription = "Шанс блока";

            ArmorBonus = BonusParameter * BonusMultiplier * 2;
            HealthBonus = BonusParameter * BonusMultiplier;

            Armor += ArmorBonus;
            Health += HealthBonus;
        }

        public override Fighter Clone()
        {
            return new Guardian();
        }

        public override void Attack(Fighter enemy)
        {
            AttackBase(enemy, Power);
        }

        protected override void TakeDamage(float damage)
        {
            if (Random.Next(CriticalChanceMax) < CriticalChance)
            {
                Console.WriteLine($"{Name}: Блок атаки");
            }
            else if (damage > Armor / ArmorDivider)
            {
                TakeDamageBase(damage);
            }
        }
    }

    internal class Assassin : Warrior
    {
        public Assassin()
        {
            Name = "Ассасин";
            CriticalDescription = "Шанс уворота";

            HealthBonus = BonusParameter * BonusMultiplier;
            PowerBonus = BonusParameter * BonusMultiplier;
            ArmorBonus = BonusParameter * BonusMultiplier;

            Health += HealthBonus;
            Power += PowerBonus;
            Armor += ArmorBonus;
        }

        public override Fighter Clone()
        {
            return new Assassin();
        }

        public override void Attack(Fighter enemy)
        {
            AttackBase(enemy, Power);
        }

        protected override void TakeDamage(float damage)
        {
            if (Random.Next(BaseBonusValue) < CriticalChance)
            {
                Console.WriteLine($"{Name}: Уворот от атаки");
            }
            else if (damage > Armor / ArmorDivider)
            {
                TakeDamageBase(damage);
            }
        }
    }

    internal abstract class Wizard : Fighter
    {
        protected readonly float ManaBaseValue = 40;
        protected readonly float PowerMagical = 15;
        protected readonly float SpellManaPrice = 10;
        protected float Mana;
        protected string SpellDescription;

        protected Wizard()
        {
            Mana = ManaBaseValue;
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"Класс: {Name}\n" +
                              $"Жизнь: {Health}\n" +
                              $"Сила: {Power}\n" +
                              $"Защита: {Armor}");
            Console.WriteLine($"Уровень маны: {Mana}");
            Console.WriteLine($"Сила заклинаний: {PowerMagical}");
            Console.WriteLine("=========");
        }
    }

    internal class Healer : Wizard
    {
        public Healer()
        {
            Name = "Лекарь";
            SpellDescription = "Лечение себя";
        }

        public override Fighter Clone()
        {
            return new Healer();
        }

        public override void Attack(Fighter enemy)
        {
            AttackBase(enemy, Power);
        }

        protected override void TakeDamage(float damage)
        {
            TakeDamageBase(damage);

            if (HealthMax - Health >= PowerMagical)
            {
                if (Health > 0 && Mana >= SpellManaPrice)
                {
                    Mana -= SpellManaPrice;
                    Health += PowerMagical;

                    Console.WriteLine($"{Name}: Вылечено {PowerMagical} жизни");

                    if (Health > HealthMax)
                    {
                        Health = HealthMax;
                    }
                }
            }
        }
    }

    internal class Warlock : Wizard
    {
        public Warlock()
        {
            Name = "Боевой маг";
            SpellDescription = "Урон магией";
        }

        public override Fighter Clone()
        {
            return new Warlock();
        }

        public override void Attack(Fighter enemy)
        {
            float commonDamage;

            if (Mana >= SpellManaPrice)
            {
                Mana -= SpellManaPrice;
                commonDamage = Power + PowerMagical;
            }
            else
            {
                commonDamage = Power;
            }

            AttackBase(enemy, commonDamage);
        }

        protected override void TakeDamage(float damage)
        {
            TakeDamageBase(damage);
        }
    }

    public enum UserCommands
    {
        SelectFirstFighter = 1,
        SelectSecondFighter = 2,
        StartBattle = 3,
        Exit = 4
    }
}