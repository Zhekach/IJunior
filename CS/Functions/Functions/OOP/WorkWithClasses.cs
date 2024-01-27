using System;

namespace Functions.OOP
{
    internal class WorkWithClasses
    {
        static void Main(string[] args)
        {
            Player player1 = new Player("Nagibaotr", 2, "warior");
            Player player2 = new Player("mega-lego", -3, "healler");

            player1.PrintInfo();
            player2.PrintInfo();
        }
    }

    class Player
    {
        private string _nickName;
        private int _level = 0;
        private string _gameClass;

        public Player(string nickName, int level, string gameClass)
        {
            _nickName = nickName;

            if (level >= 0)
            {
                _level = level;
            }

            _gameClass = gameClass;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Ник игрока: {_nickName} \n" +
                              $"Его уровень: {_level} \n" +
                              $"Его игровой класс: {_gameClass} \n");
        }
    }
}