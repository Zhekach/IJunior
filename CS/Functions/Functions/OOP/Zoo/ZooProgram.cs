using System;
using System.Collections.Generic;
using System.Linq;


namespace Functions.OOP.Zoo
{
    internal class ZooProgram
    {
        static void Main()
        {
            Cage cage = new Cage(5, 1);
            cage.PrintInfo();
        }
        
    }

    internal class Cage
    {
        private readonly List<Animal> _animals = new List<Animal>();
        
        public int Id { get; private set; }

        public Cage(int animalsCount, int id)
        {
            _animals.AddRange(GenerateAnimals(animalsCount));
            Id = id;
        }
        
        public void PrintInfo()
        {
            Console.WriteLine($"В вольере № {Id} содержатся:");
            
            foreach (Animal animal in _animals)
            {
                animal.PrintInfo();
            }

            Console.WriteLine();
        }

        private static List<Animal> GenerateAnimals(int count)
        {
            List<Animal> result = new List<Animal>();
            
            for (int i = 0; i < count; i++)
            {
                result.Add(new Animal());    
            }

            return result;
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

        private static readonly Dictionary<AnimalGender, string> s_genders = new Dictionary<AnimalGender, string>()
        {
            { AnimalGender.Male, "Самец" },
            { AnimalGender.Female, "Самка" },
            { AnimalGender.Other, "Пока непонятно" }
        };

        private readonly string _type;
        private readonly string _voice;
        private readonly string _gender;

        public Animal()
        {
            _type = GenerateRandomType();
            _voice = GetVoice(_type);
            _gender = GenerateRandomGender();
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Я - {_type}, мой звук - {_voice}, мой пол - {_gender}");
        }
        
        private string GetVoice(string type)
        {
            s_typesVoices.TryGetValue(type, out string result);
            return result;
        }
        
        private string GenerateRandomType()
        {
            int index = RandomUtility.GetRandomInt(s_typesVoices.Count());
            var result = s_typesVoices.Keys.ElementAt(index);

            return result;
        }
        
        private static string GenerateRandomGender()
        {
            Array genders = Enum.GetValues(typeof(AnimalGender));
            AnimalGender gender = (AnimalGender)genders.GetValue(RandomUtility.GetRandomInt(genders.Length));
            s_genders.TryGetValue(gender, out string result);

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