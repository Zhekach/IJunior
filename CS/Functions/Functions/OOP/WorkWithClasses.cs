using System;

namespace Functions.OOP
{
    internal class WorkWithClasses
    {
        static void Main1(string[] args)
        {
            PlayerForInfo player1 = new PlayerForInfo("Nagibaotr", 2, "warior");
            PlayerForInfo player2 = new PlayerForInfo("mega-lego", -3, "healler");

            player1.PrintInfo();
            player2.PrintInfo();
        }
    }

    class PlayerForInfo
    {
        private string _nickName;
        private int _level = 0;
        private string _gameClass;

        public PlayerForInfo(string nickName, int level, string gameClass)
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