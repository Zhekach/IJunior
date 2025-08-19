using System;
using System.Collections.Generic;
using System.Linq;

namespace Functions.OOP.Zoo
{
    internal class Program
    {
        private static void Main()
        {
            ZooView zooView = new ZooView(4);
            zooView.ManageZoo();
        }
    }

    internal class ZooView
    {
        private readonly Zoo _zoo;
        
        private const ConsoleKey ExitKey = ConsoleKey.Q;
        private const ConsoleKey EnterKey = ConsoleKey.E;

        public ZooView(int cagesCount)
        {
            _zoo = ZooFactory.GenerateZoo(cagesCount);
        }

        public void ManageZoo()
        {
            bool isUserExited = false;

            while (isUserExited == false)
            {
                Console.WriteLine(BuildMainMenu());
                ConsoleKeyInfo pressedKey = Console.ReadKey();

                switch (pressedKey.Key)
                {
                    case (EnterKey):
                        ChooseCage();
                        break;
                    case (ExitKey):
                        isUserExited = true;
                        break;
                }

                Console.Clear();
            }

            Console.WriteLine("Всего доброго!");
        }

        private void ChooseCage()
        {
            Console.Clear();
            Console.WriteLine(BuildCagesMenu());

            int choiceNumber = Utility.ReadInt();
            _zoo.PrintCageInfo(choiceNumber);

            Console.WriteLine("Нажмите любую клавишу для выхода в главное меню");
            Console.ReadKey();
        }

        private string BuildMainMenu()
        {
            return "Добро пожаловать в зоопарк\n" +
                   $"{EnterKey} - для входа в зоопарк\n" +
                   $"{ExitKey} - для выхода\n";
        }

        private string BuildCagesMenu()
        {
            return $"В зоопарке {_zoo.CagesCount} вольеров\n" +
                   "Введите номер вольера в диапазоне:" +
                   $"1 - {_zoo.CagesCount}\n";
        }
    }

    internal class Zoo
    {
        private readonly List<Cage> _cages;

        public Zoo(List<Cage> cages)
        {
            _cages = new List<Cage>(cages);
        }

        public int CagesCount => _cages.Count;

        public void PrintCageInfo(int cageNumber)
        {
            if (cageNumber < 1 || cageNumber > _cages.Count)
            {
                Console.WriteLine("В зоопарке нет такого вольера");
                return;
            }

            _cages[cageNumber - 1].PrintInfo();
        }
    }

    internal static class ZooFactory
    {
        private const int AnimalsInCageMaxCount = 5;

        public static Zoo GenerateZoo(int cagesCount)
        {
            List<Cage> cages = GenerateCages(cagesCount);
            Zoo newZoo = new Zoo(cages);

            return newZoo;
        }

        private static List<Cage> GenerateCages(int count)
        {
            List<Cage> cages = new List<Cage>();

            for (int i = 0; i < count; i++)
            {
                int animalsCount = Utility.GetRandomInt(1, AnimalsInCageMaxCount);
                Cage newCage = CageFactory.GenerateCage(animalsCount);
                cages.Add(newCage);
            }

            return cages;
        }
    }

    internal class Cage
    {
        private readonly List<Animal> _animals;
        private readonly int _id;

        public Cage(List<Animal> animals, int id)
        {
            _animals = new List<Animal>(animals);
            _id = id;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"В вольере № {_id} содержатся:");

            foreach (Animal animal in _animals)
            {
                animal.PrintInfo();
            }

            Console.WriteLine();
        }
    }

    internal static class CageFactory
    {
        private static int s_currentCageId = 1;

        public static Cage GenerateCage(int animalsCount)
        {
            List<Animal> animals = GenerateAnimals(animalsCount);
            Cage newCage = new Cage(animals, s_currentCageId);
            s_currentCageId++;

            return newCage;
        }

        private static List<Animal> GenerateAnimals(int count)
        {
            List<Animal> result = new List<Animal>();

            for (int i = 0; i < count; i++)
            {
                result.Add(AnimalFactory.GenerateAnimal());
            }

            return result;
        }
    }

    internal class Animal
    {
        private readonly string _type;
        private readonly string _voice;
        private readonly string _gender;

        public Animal(string type, string voice, string gender)
        {
            _type = type;
            _voice = voice;
            _gender = gender;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Я - {_type}, мой звук - {_voice}, мой пол - {_gender}");
        }
    }

    internal static class AnimalFactory
    {
        private static readonly Dictionary<string, string> s_typesVoices = new Dictionary<string, string>()
        {
            { "Волк", "Ауф" },
            { "Рыба", "Молчит как рыба" },
            { "Кот", "Критикует способ поедания бутерброда" },
            { "Лиса", "Занята, доедает колобка" },
            { "Мышь", "Фыр-фыр" },
            { "Сова", "Безвозмедно, то есть даром" },
            { "Крокодил", "Крокодит" },
            { "Собака", "Гав" },
        };

        private static readonly Dictionary<AnimalGender, string> s_genders = new Dictionary<AnimalGender, string>()
        {
            { AnimalGender.Male, "Самец" },
            { AnimalGender.Female, "Самка" },
            { AnimalGender.Other, "Пока непонятно" }
        };

        public static Animal GenerateAnimal()
        {
            string type = GenerateRandomType();
            string voice = s_typesVoices[type];
            string gender = GenerateRandomGender();

            Animal animal = new Animal(type, voice, gender);

            return animal;
        }

        private static string GenerateRandomType()
        {
            int index = Utility.GetRandomInt(s_typesVoices.Count());
            var result = s_typesVoices.Keys.ElementAt(index);

            return result;
        }

        private static string GenerateRandomGender()
        {
            Array genders = Enum.GetValues(typeof(AnimalGender));
            AnimalGender gender = (AnimalGender)genders.GetValue(Utility.GetRandomInt(genders.Length));
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

    internal static class Utility
    {
        private static readonly Random s_random = new Random();

        public static int GetRandomInt(int maxValue)
        {
            return s_random.Next(maxValue);
        }

        public static int GetRandomInt(int minValue, int maxValue)
        {
            return s_random.Next(minValue, maxValue);
        }

        public static int ReadInt()
        {
            bool isIntEntered = false;
            int parsedInt = 0;

            while (isIntEntered == false)
            {
                string enteredString;

                Console.WriteLine("Введите целое число:");
                enteredString = Console.ReadLine();

                isIntEntered = int.TryParse(enteredString, out parsedInt);

                if (isIntEntered)
                {
                    Console.WriteLine($"Введенное число распознанно, это: {parsedInt}");
                }
                else
                {
                    Console.WriteLine("Вы ввели неверно, попробуйте ещё раз)\n");
                }
            }

            return parsedInt;
        }
    }
}