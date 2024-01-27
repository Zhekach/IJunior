using System;
using System.Collections.Generic;

namespace Functions.OOP.PlayersDataBase
{
    internal class PlayersDataBase
    {
        static void Main1(string[] args)
        {
            DataBase dataBase = new DataBase();

            Player player1 = new Player(15, "jon", 2);
            Player player2 = new Player(10, "Micky", 5);
            Player player3 = new Player(10, "Ivan", 99);

            dataBase.AddPlayer(player1);
            dataBase.AddPlayer(player2);
            dataBase.AddPlayer(player3);
            dataBase.BanPlayer(10);
            dataBase.PrintAllPlayersInfo();

            Console.WriteLine("\nТеперь разбаним игрока");

            dataBase.UnBanPlayer(10);
            dataBase.PrintAllPlayersInfo();
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
            if(_players.ContainsKey(id))
            {
                _players[id].IsBanned = true;
            }
            else
            {
                PrintNoPlayerMessage();
            }
        }

        public void UnBanPlayer(int id)
        {
            if (_players.ContainsKey(id))
            {
                _players[id].IsBanned = false;
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
        private bool _isBanned = false;

        public Player(int id, string nickname, int level)
        {
            _id = id;
            _nickname = nickname;

            if(level > 0) 
            { 
                _level = level;
            }
        }

        public int Id {  get { return _id; } }
        public string Name { get { return _nickname;} }
        public int Level { get { return _level;} }
        public bool IsBanned { get { return _isBanned; } set { _isBanned = value;} }

        public void PrintInfo()
        {
            Console.WriteLine($"ID {_id}: Игрок с ником {_nickname}, Уровень: {_level}, Забанен: {_isBanned}.");
        }

        public void LevelUp() { _level ++;}

        public void LevelDown() { _level --;}
    }
}