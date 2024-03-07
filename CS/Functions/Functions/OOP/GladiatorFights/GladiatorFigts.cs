﻿using System;
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
                Console.WriteLine($"Первый боец - {_fighterFirst.Name}");
            }

            if (_fighterSecond != null)
            {
                Console.WriteLine($"Второй боец - {_fighterSecond.Name}");
            }
        }

        private Fighter CreateFighter()
        {
            Fighter fighter;

            Console.Clear();
            Console.WriteLine("Введите номер типа бойца:");

            foreach (KeyValuePair<int, Fighter> pair in _fightersDictionary)
            {
                Console.WriteLine($"Номер {pair.Key} - {pair.Value.Name}");
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

                fighter1.PrintHpInfo();
                fighter2.PrintHpInfo();

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

        public void PrintHpInfo()
        {
            Console.WriteLine($"{Name}: Уровень жизни {Health} ");
        }

        public abstract Fighter Clone();

        public abstract void Attack(Fighter enemy);

        public abstract void TakeDamage(float damage);

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
                Health -= (damage - Armor / ArmorDivider);
                Console.WriteLine($"{Name}: Получено {damage - Armor / ArmorDivider} урона");
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

    abstract class Warrior : Fighter
    {
        protected Random Random = new Random();
        protected int BaseBonusValue = 50;
        protected float BonusParameter;
        protected float BonusMultiplier = 0.33f;

        protected float HealthBonus;
        protected float PowerBonus;
        protected float ArmorBonus;

        protected string CriticalDescription;
        protected float CriticalChance;
        protected int CriticalChanceMax = 100;
        protected float CriticalMultiplier = 2;

        protected Warrior() : base()
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

    class Knight : Warrior
    {
        public Knight() : base()
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

        public override void TakeDamage(float damage)
        {
            TakeDamageBase(damage);
        }
    }

    class Guardian : Warrior
    {
        public Guardian() : base()
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

        public override void TakeDamage(float damage)
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

    class Assassin : Warrior
    {
        public Assassin() : base()
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

        public override void TakeDamage(float damage)
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

    abstract class Wizzard : Fighter
    {
        protected float ManaBaseValue = 40;
        protected float Mana;
        protected float PowerMagical = 15;
        protected float SpellManaPrice = 10;
        protected string SpellDescription;

        protected Wizzard() : base()
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

    class Healer : Wizzard
    {
        public Healer() : base()
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

        public override void TakeDamage(float damage)
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

    class Warlock : Wizzard
    {
        public Warlock() : base()
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

        public override void TakeDamage(float damage)
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