using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.OOP.Zoo
{
    internal class ZooProgramm
    {
    }

    internal abstract class Animal
    {
        private const string GenderMale = "Самец";
        private const string GenderFemale = "Самка";
        private const int AvailibleGendersCount = 2;

        protected readonly string _gender;

        protected Animal()
        {
            _gender = GetRandomGender();
        }

        protected string GetRandomGender()
        {
            int randomGenderIndex = RandomUtility.GetRandomInt(AvailibleGendersCount);

            if (randomGenderIndex == 0)
            {
                return GenderMale;
            }
            else
            {
                return GenderFemale;
            }
        }
    }

    internal interface IVoicebale
    {
        void MakeSound();
    }

    internal class RandomUtility
    {
        private static readonly Random s_Random = new Random();

        public static int GetRandomInt (int maxValue)
        {
            return s_Random.Next (maxValue);
        }
    }
}
