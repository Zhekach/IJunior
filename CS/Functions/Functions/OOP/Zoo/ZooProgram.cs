using System;
using System.Collections.Generic;
using System.Linq;


namespace Functions.OOP.Zoo
{
    internal class ZooProgram
    {
        static void Main()
        {
            Animal newAnimal = new Animal();
            newAnimal.PrintInfo();
        }
        
    }

    internal class Animal
    {
        private static readonly Dictionary<string, string> s_typesVoices = new Dictionary<string, string>()
        {
            {"Волк", "Ауф"},
            {"Рыба", "Молчит как рыба"},
            {"Кот", "Критикует способ поедания бутерброда"},
            {"Лиса", "Занята, доедает колобка"},
            {"Мышь", "Фыр-фыр"},
            {"Сова", "Безвозмедно, то есть даром"},
            {"Крокодил", "Крокодит"},
            {"Собака", "Гав"},
        };

        private static readonly Dictionary<AnimalGender, string> S_genders = new Dictionary<AnimalGender, string>()
        {
            { AnimalGender.Male, "Самец" },
            { AnimalGender.Female, "Самка" },
            { AnimalGender.Other, "Пока непонятно" }
        };

        public readonly string Type;
        public readonly string Voice;
        public readonly string Gender;

        public Animal()
        {
            Type = GenerateRandomType();
            Voice = GetVoice(Type);
            Gender = GenerateRandomGender();
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Я - {Type}, мой звук - {Voice}, мой пол - {Gender}");
        }
        
        private string GetVoice(string type)
        {
            s_typesVoices.TryGetValue(type, out string result);
            return result;
        }
        
        private string GenerateRandomType()
        {
            string result;

            int index = RandomUtility.GetRandomInt(s_typesVoices.Count());
            result = s_typesVoices.Keys.ElementAt(index);

            return result;
        }
        
        private static string GenerateRandomGender()
        {
            string result;
            Array genders = Enum.GetValues(typeof(AnimalGender));
            AnimalGender gender = (AnimalGender)genders.GetValue(RandomUtility.GetRandomInt(genders.Length));
            S_genders.TryGetValue(gender, out result);

            return result;
        }
    }

    internal enum AnimalGender
    {
        Male,
        Female,
        Other
    }

    internal static class RandomUtility
    {
        private static readonly Random s_random = new Random();

        public static int GetRandomInt (int maxValue)
        {
            return s_random.Next (maxValue);
        }
    }
}