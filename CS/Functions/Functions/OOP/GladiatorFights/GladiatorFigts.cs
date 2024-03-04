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
        public void Run()
        {
            Warrior warrior1 = new Knight();
            warrior1.PrintInfo();

            Thread.Sleep(10);

            Warrior warrior2 = new Guardian();
            warrior2.PrintInfo();

            StartBattle(warrior1, warrior2);
        }

        public void StartBattle (Warrior warrior1, Warrior warrior2)
        {
            while(warrior1.Health > 0 && warrior2.Health > 0)
            {
                Console.WriteLine("\n-====Новый ход!====-\n");

                warrior1.CauseClassDamage(warrior2 );
                if(warrior2.Health > 0 )
                {
                    warrior2.CauseClassDamage(warrior1 );
                }

                Console.WriteLine("---Итоги---");

                warrior1.PrintHPInfo() ;
                warrior2.PrintHPInfo() ;

                Thread.Sleep(5000);
            }
        }
    }

    abstract class Fighter
    {
        protected string _className;
        private int _baseParameterValue = 45;
        protected float _power;
        protected float _armor;
        protected float _armorDivider = 4;

        protected Random _random = new Random();
        protected int _baseBonusValue = 100;
        protected float _damage;

        public float Health { get; protected set; }

        protected Fighter()
        {
            Health = _baseParameterValue;
            _power = _baseParameterValue;
            _armor = _baseParameterValue;
        }

        public void PrintBaseInfo()
        {
            Console.WriteLine($"Класс: {_className}\n" +
                              $"Жизнь: {Health}\n" +
                              $"Сила: {_power}\n" +
                              $"Защита: {_armor}");
        }

        public void PrintHPInfo()
        {
            Console.WriteLine($"{_className}: Уровень жизни {Health} ");
        }

        protected void CauseBaseDamage(Fighter enemy, float damage)
        {
            damage = _power;

            Console.WriteLine($"{_className}: Пытается нанести {damage} урона");

            enemy.TakeDamage(damage);
        }

        public abstract void CauseClassDamage(Fighter enemy);

        public void TakeBaseDamage(float damage)
        {
            if (damage > _armor / _armorDivider)
            {
                Health -= (damage - _armor / _armorDivider);
                Console.WriteLine($"{_className}: Получено {damage - _armor / _armorDivider} урона");
            }
            else
            {
                Console.WriteLine($"{_className}: Урон не прошёл");
            }

            if (Health < 0)
            {
                Health = 0;
            }
        }

        public abstract void TakeDamage(float damage);
    }

    abstract class Warrior : Fighter
    {
        protected float _bonusParameter;
        protected float _healthBonus;
        protected float _damageBonus;
        protected float _armorBonus;

        protected string _criticalDescription;
        protected float _criticalChance;
        protected float _criticalMultiplier;

        protected Warrior() : base() 
        {
            _bonusParameter = _random.Next(_baseBonusValue);
            _criticalChance = _baseBonusValue - _bonusParameter;
        }

        public void PrintInfo()
        {
            PrintBaseInfo();
            Console.WriteLine($"{_criticalDescription}: {_criticalChance}");
            Console.WriteLine("=========");
        }
    }

    class Knight : Warrior 
    { 
        public Knight():base()
        {
            _className = "Рыцарь";
            _criticalDescription = "Шанс крита";

            _damageBonus = _bonusParameter * 2 / 3;
            _healthBonus = _bonusParameter * 1 / 3;

            _power += _damageBonus;
            Health += _healthBonus;
        }

        public override void CauseClassDamage(Fighter enemy)
        {
            float damage;

            if(_random.Next(_baseBonusValue) < _criticalChance)
            {
                damage = _power * _criticalMultiplier;
                Console.WriteLine($"{_className}: Критическая атака");
            }
            else
            {
                damage = _damageBonus;
            }

            CauseBaseDamage(enemy, damage);
        }

        public override void TakeDamage(float damage)
        {
            TakeBaseDamage(damage);
        }
    }

    class Guardian : Warrior
    {
        public Guardian() : base()
        {
            _className = "Защитник";
            _criticalDescription = "Шанс блока";

            _armorBonus = _bonusParameter * 2 / 3;
            _healthBonus = _bonusParameter * 1 / 3;

            _armor += _armorBonus;
            Health += _healthBonus;
        }

        public override void CauseClassDamage(Fighter enemy)
        {
            CauseBaseDamage(enemy, _power);
        }

        public override void TakeDamage(float damage)
        {
            if(_random.Next(_baseBonusValue) < _criticalChance)
            {
                damage = 0;
                Console.WriteLine($"{_className}: Блок атаки");
            }
            else if (damage > _armor / _armorDivider)
            {
                TakeBaseDamage(damage);
            }
        }
    }

    public enum FighterClass
    {
        Knight,
        Tank,
        Assasin,
        Healer,
        Warlock
    }
}