using System;
using System.Collections.Generic;

namespace Functions.OOP.Zoo
{
    internal class Program
    {
        private static void Main()
        {
            ZooFactory zooFactory = new ZooFactory();
            ZooView zooView = new ZooView(4, zooFactory);
            zooView.ManageZoo();
        }
    }

    internal class ZooView
    {
        private readonly Zoo _zoo;

        private const ConsoleKey ExitKey = ConsoleKey.Q;
        private const ConsoleKey EnterKey = ConsoleKey.E;

        public ZooView(int cagesCount, ZooFactory zooFactory)
        {
            _zoo = zooFactory.GenerateZoo(cagesCount);
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

    internal class ZooFactory
    {
        private const int AnimalsInCageMaxCount = 5;
        private readonly CageFactory _cageFactory = new CageFactory();

        public Zoo GenerateZoo(int cagesCount)
        {
            List<Cage> cages = GenerateCages(cagesCount);
            Zoo newZoo = new Zoo(cages);

            return newZoo;
        }

        private List<Cage> GenerateCages(int count)
        {
            List<Cage> cages = new List<Cage>();

            for (int i = 0; i < count; i++)
            {
                int animalsCount = Utility.GetRandomInt(1, AnimalsInCageMaxCount);
                Cage newCage = _cageFactory.GenerateCage(animalsCount);
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

    internal class CageFactory
    {
        private int _currentCageId = 1;
        private readonly AnimalFactory _animalFactory = new AnimalFactory();

        public Cage GenerateCage(int animalsCount)
        {
            List<Animal> animals = GenerateAnimals(animalsCount);
            Cage newCage = new Cage(animals, _currentCageId);
            _currentCageId++;

            return newCage;
        }

        private List<Animal> GenerateAnimals(int count)
        {
            List<Animal> result = new List<Animal>();
            AnimalType type = Utility.GetRandomEnumValue<AnimalType>();
            
            for (int i = 0; i < count; i++)
            {
                result.Add(_animalFactory.GenerateAnimal(type));
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

    internal class AnimalFactory
    {
        private readonly Dictionary<AnimalType, string> _typesVoices = new Dictionary<AnimalType, string>()
        {
            { AnimalType.Wolf, "Ауф" },
            { AnimalType.Fish, "Молчит как рыба" },
            { AnimalType.Cat, "Критикует способ поедания бутерброда" },
            { AnimalType.Fox, "Занята, доедает колобка" },
            { AnimalType.Mouse, "Фыр-фыр" },
            { AnimalType.Owl, "Безвозмедно, то есть даром" },
            { AnimalType.Crocodile, "Крокодит" },
            { AnimalType.Dog, "Гав" }
        };
        
        private readonly Dictionary<AnimalType, string> _animalNamesRu = new Dictionary<AnimalType, string>()
        {
            { AnimalType.Wolf, "Волк" },
            { AnimalType.Fish, "Рыба" },
            { AnimalType.Cat, "Кот" },
            { AnimalType.Fox, "Лиса" },
            { AnimalType.Mouse, "Мышь" },
            { AnimalType.Owl, "Сова" },
            { AnimalType.Crocodile, "Крокодил" },
            { AnimalType.Dog, "Собака" }
        };

        private readonly Dictionary<AnimalGender, string> _genders = new Dictionary<AnimalGender, string>()
        {
            { AnimalGender.Male, "Самец" },
            { AnimalGender.Female, "Самка" },
            { AnimalGender.Other, "Пока непонятно" }
        };

        public Animal GenerateAnimal(AnimalType type)
        {
            string typeString = _animalNamesRu[type];
            string voice = _typesVoices[type];
            string gender = GenerateRandomGender();

            Animal animal = new Animal(typeString, voice, gender);

            return animal;
        }

        private string GenerateRandomGender()
        {
            Array genders = Enum.GetValues(typeof(AnimalGender));
            AnimalGender gender = (AnimalGender)genders.GetValue(Utility.GetRandomInt(genders.Length));
            _genders.TryGetValue(gender, out string result);

            return result;
        }
    }

    internal enum AnimalGender
    {
        Male,
        Female,
        Other
    }

    public enum AnimalType
    {
        Wolf,
        Fish,
        Cat,
        Fox,
        Mouse,
        Owl,
        Crocodile,
        Dog
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
        
        public static T GetRandomEnumValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            int index = GetRandomInt(values.Length);
            var result = (T)values.GetValue(index);

            return result;
        }

        public static int ReadInt()
        {
            bool isIntEntered = false;
            int parsedInt = 0;

            while (isIntEntered == false)
            {
                Console.WriteLine("Введите целое число:");
                string enteredString = Console.ReadLine();

                isIntEntered = int.TryParse(enteredString, out parsedInt);

                Console.WriteLine(isIntEntered
                    ? $"Введенное число распознанно, это: {parsedInt}"
                    : "Вы ввели неверно, попробуйте ещё раз)\n");
            }

            return parsedInt;
        }
    }
}