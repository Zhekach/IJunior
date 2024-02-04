using System;
using System.Collections.Generic;

namespace Functions.OOP.PlayersDataBase
{
    internal class PlayersDataBase
    {
        static void Main1(string[] args)
        {
            const string AddCommand = "add";
            const string DeleteCommand = "del";
            const string BanCommand = "ban";
            const string UnbanCommand = "unban";
            const string ShowCommand = "show";
            const string ExitCommand = "exit";

            bool isUserExited = false;

            DataBase dataBase = new DataBase();

            while (isUserExited == false)
            {
                Console.Clear();

                PrintUI(AddCommand,DeleteCommand,BanCommand, UnbanCommand, ShowCommand, ExitCommand);

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case AddCommand:
                        AddPlayerUI(dataBase);
                        break;
                    case DeleteCommand:
                        DeletePlayerUI(dataBase);
                        break;
                    case BanCommand:
                        BanPlayerUI(dataBase);
                        break;
                    case UnbanCommand:
                        UnbanPlayerUI(dataBase);
                        break;
                    case ShowCommand:
                        dataBase.PrintAllPlayersInfo();
                        break;
                    case ExitCommand:
                        isUserExited = true;
                        break;
                    default:
                        Console.WriteLine("Нет такой команды, попробуйте снова.");
                        break;
                }

                Console.WriteLine("Для продолжения нажмите любую клавишу.");
                Console.ReadKey();
            }
        }
        static void PrintUI(string addCommand, string deleteCommand, string banCommand, string unbanCommand, string showCommand, string exitCommand)
        {
            Console.WriteLine("Введите команду для работы с базой данных:\n" +
                $"{addCommand} - добавить игрока\n" +
                $"{deleteCommand} - удалить игрока\n" +
                $"{banCommand} - забанить игрока\n" +
                $"{unbanCommand} - разбанить игрока\n" +
                $"{showCommand} - показать список всех игроков\n" +
                $"{exitCommand} - выход из программы\n");
        }

        static void AddPlayerUI(DataBase database)
        {
            int id;
            Console.WriteLine("Введите id игрока");
            id = ReadInt();

            string nickname;
            Console.WriteLine("Введите ник игрока");
            nickname = Console.ReadLine();

            int level;
            Console.WriteLine("Введите уровень игрока");
            level = ReadInt();

            Player player = new Player(id,nickname, level);

            database.AddPlayer(player);
        }

        static int ReadInt()
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

        static void DeletePlayerUI(DataBase database)
        {
            int id;
            Console.WriteLine("Введите id игрока");
            id = ReadInt();

            database.RemovePlayerByIndex(id);
        }

        static void BanPlayerUI(DataBase database)
        {
            int id;
            Console.WriteLine("Введите id игрока");
            id = ReadInt();

            database.BanPlayer(id);
        }
        
        static void UnbanPlayerUI(DataBase database)
        {
            int id;
            Console.WriteLine("Введите id игрока");
            id = ReadInt();

            database.UnbanPlayer(id);
        }
    }

    class DataBase
    {
        private Dictionary<int, Player> _players;

        public DataBase()
        {
            _players = new Dictionary<int, Player>();
        }

        static void PrintNoPlayerMessage()
        {
            Console.WriteLine("Игрока с таким номером в базе и нет вовсе");
        }

        public void AddPlayer(Player player)
        {
            int id = player.Id;

            if (_players.ContainsKey(id) == false)
            {
                _players.Add(id, player);
            }
            else
            {
                Console.WriteLine("Игрок с таким номером уже есть в базе, а номер должен быть уникальным!");
            }
        }

        public void RemovePlayerByIndex(int id)
        {
            if (_players.ContainsKey(id))
            {
                _players.Remove(id);
            }
            else
            {
                PrintNoPlayerMessage();
            }
        }

        public void BanPlayer(int id)
        {
            if (_players.ContainsKey(id))
            {
                _players[id].Ban();

                Console.WriteLine("Игрок с таким id забанен");
            }
            else
            {
                PrintNoPlayerMessage();
            }
        }

        public void UnbanPlayer(int id)
        {
            if (_players.ContainsKey(id))
            {
                _players[id].Unban();

                Console.WriteLine("Игрок с таким id разабанен");
            }
            else
            {
                PrintNoPlayerMessage();
            }
        }

        public void PrintAllPlayersInfo()
        {
            foreach (Player player in _players.Values)
            {
                player.PrintInfo();
            }
        }
    }

    class Player
    {
        private int _id;
        private string _nickname;
        private int _level = 0;

        public Player(int id, string nickname, int level)
        {
            _id = id;
            _nickname = nickname;

            if (level > 0)
            {
                _level = level;
            }
        }

        public int Id { get { return _id; } }
        public string Name { get { return _nickname; } }
        public int Level { get { return _level; } }
        public bool IsBanned { get; private set; }

        public void PrintInfo()
        {
            Console.WriteLine($"ID {_id}: Игрок с ником {_nickname}, Уровень: {_level}, Забанен: {IsBanned}.");
        }

        public void LevelUp() { _level++; }

        public void LevelDown() { _level--; }

        public void Ban() { IsBanned = true; }

        public void Unban() { IsBanned = false; }
    }
}