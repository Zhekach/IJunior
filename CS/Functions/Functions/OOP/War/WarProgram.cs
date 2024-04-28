using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.OOP.War
{
    internal class WarProgram
    {
    }

    internal abstract class Soldier
    {
        protected readonly float BaseHealth = 50;
        protected readonly float BasePower = 15;
        protected readonly float BaseArmor = 10;

        protected readonly int UpdatingPoints = 9;

        protected Soldier() 
        {
            Health = BaseHealth; 
            Power = BasePower; 
            Armor = BaseArmor;
        }

        public float Health {  get; private set; }
        public float Power {  get; private set; }
        public float Armor {  get; private set; }
    }
}
