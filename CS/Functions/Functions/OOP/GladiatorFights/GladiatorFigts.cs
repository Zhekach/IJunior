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
            while(warrior1.Health > 0 || warrior2.Health > 0)
            {
                warrior1.CauseClassDamage(warrior2 );
                if(warrior2.Health > 0 )
                {
                    warrior2.CauseClassDamage(warrior1 );
                }
                
                warrior1.PrintInfo() ;
                warrior2.PrintInfo() ;

                Thread.Sleep(5000);
            }
        }
    }

    abstract class Fighter
    {
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
            Console.WriteLine($"Жизнь: {Health}\n" +
                              $"Сила: {_power}\n" +
                              $"Защита: {_armor}");
        }

        protected void CauseBaseDamage(Fighter enemy, float damage = 3)
        {
            damage = _power;
            enemy.TakeDamage(damage);
        }

        public abstract void CauseClassDamage(Fighter enemy);

        public abstract void TakeDamage(float damage);
    }

    abstract class Warrior : Fighter
    {
        protected string _name;
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
            Console.WriteLine($"Класс: {_name}");
            PrintBaseInfo();
            Console.WriteLine($"{_criticalDescription}: {_criticalChance}");
            Console.WriteLine("=========");
        }
    }

    class Knight : Warrior 
    { 
        public Knight():base()
        {
            _name = "Рыцарь";
            _criticalDescription = "Шанс крита";

            _damageBonus = _bonusParameter * 2 / 3;
            _healthBonus = _bonusParameter * 1 / 3;

            _power += _damageBonus;
            Health += _healthBonus;
        }

        public override void CauseClassDamage(Fighter enemy)
        {
            float resultDamage;

            if(_random.Next(_baseBonusValue) < _criticalChance)
            {
                resultDamage = _power * _criticalMultiplier;
                Console.WriteLine("Критическая атака");
            }
            else
            {
                resultDamage = _damageBonus;
            }

            CauseBaseDamage(enemy);
        }

        public override void TakeDamage(float damage)
        {
            if(damage > _armor/2)
            {
                Health -= (damage - _armor/2);
                Console.WriteLine($"Получено {damage - _armor} урона");
            }
            else
            {
                Console.WriteLine("Урон не прошёл");
            }

            if(Health< 0)
            {
               Health = 0;
            }
        }
    }

    class Guardian : Warrior
    {
        public Guardian() : base()
        {
            _name = "Защитник";
            _criticalDescription = "Шанс блока";

            _armorBonus = _bonusParameter * 2 / 3;
            _healthBonus = _bonusParameter * 1 / 3;

            _armor += _armorBonus;
            Health += _healthBonus;
        }

        public override void CauseClassDamage(Fighter enemy)
        {
            CauseBaseDamage(enemy);
        }

        public override void TakeDamage(float damage)
        {
            if(_random.Next(_baseBonusValue) < _criticalChance)
            {
                damage = 0;
                Console.WriteLine("Блок атаки");
            }
            else if (damage > _armor / 2)
            {
                Health -= (damage - _armor / 2);
            }

            if (Health < 0)
            {
                Health = 0;
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
