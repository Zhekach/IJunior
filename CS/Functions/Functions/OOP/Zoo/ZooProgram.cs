using System;
using System.Collections.Generic;
using System.Linq;


namespace Functions.OOP.Zoo
{
    internal class ZooProgram
    {
        private static void Main()
        {
            ZooController zooController = new ZooController();
            zooController.StartControl();
        }
    }

    internal class ZooController
    {
        private const ConsoleKey ExitKey = ConsoleKey.Q;
        private const ConsoleKey EnterKey = ConsoleKey.E;
        private const int CagesCount = 4;

        private readonly Zoo _zoo;

        private readonly string _mainMenu =
            "Добро пожаловать в зоопарк\n" +
            $"{EnterKey} - для входа в зоопарк\n" +
            $"{ExitKey} - для выхода\n";

        private string _chooseCageMenu =
            "Введите номер вольера в диапазоне:" +
            $"{CagesCount}";

        private bool _isUserExited;


        public ZooController()
        {
            _zoo = new Zoo(CagesCount);
        }

        public void StartControl()
        {
            while (_isUserExited == false)
            {
                ManageZoo();
                Console.Clear();
            }

            Console.WriteLine("Всего доброго!");
        }

        private void ManageZoo()
        {
            Console.WriteLine(_mainMenu);
            ConsoleKeyInfo pressedKey = Console.ReadKey();

            switch (pressedKey.Key)
            {
                case (EnterKey):
                    ChooseCage();
                    break;
                case (ExitKey):
                    _isUserExited = true;
                    break;
            }
        }

        private void ChooseCage()
        {
            Console.Clear();
            Console.WriteLine("Введите номер вольера в диапазоне:" +
                              $"1 -{_zoo.CagesCount}");

            int choiceNumber = Zoo.Utility.ReadInt();
            _zoo.PrintCageInfo(choiceNumber);

            Console.WriteLine("Нажмите любую клавишу для выхода в главное меню");
            Console.ReadKey();
        }
    }

    internal class Zoo
    {
        private readonly int _cagesCount;
        private const int AnimalsInCageMaxCount = 5;

        private readonly List<Cage> _cages = new List<Cage>();

        public int CagesCount => _cages.Count;

        public Zoo(int cagesCount)
        {
            _cagesCount = cagesCount;
            GenerateCages(_cagesCount);
        }

        public void PrintCageInfo(int cageNumber)
        {
            if (cageNumber < 1 || cageNumber > _cages.Count)
            {
                Console.WriteLine("В зоопарке нет такого вольера");
                return;
            }

            _cages[cageNumber - 1].PrintInfo();
        }

        private void GenerateCages(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int animalsCount = Utility.GetRandomInt(AnimalsInCageMaxCount);
                Cage newCage = new Cage(animalsCount, i+1);
                _cages.Add(newCage);
            }
        }

        internal class Cage
        {
            private readonly List<Animal> _animals = new List<Animal>();
            private readonly int _id;

            public Cage(int animalsCount, int id)
            {
                _animals.AddRange(GenerateAnimals(animalsCount));
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
}