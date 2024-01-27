using System;

namespace Functions.OOP
{
    internal class WorkWithProperties
    {
        static void Main1(string[] args)
        {
            Player player1 = new Player(20,25,'@');
            Player player2 = new Player(200,45, '$');

            Renderer render1 = new Renderer();
            Renderer render2 = new Renderer();

            render1.PrintPlayer(player1);
            render2.PrintPlayer(player2);
        }
    }

    class Renderer
    {
        public Renderer() { }

        public void PrintPlayer(Player player)
        {
            Console.SetCursorPosition(player.XPosition, player.YPosition);
            Console.WriteLine(player.Character);
        }
    }

    class Player
    {
        static private int s_xMaxPosition = 100;
        static private int s_xMinPosition = 0;
        static private int s_yMaxPosition = 150;
        static private int s_yMinPosition = 0;
        private char _character;
        private int _xPosition;
        private int _yPosition;

        public Player(int xPosition, int yPosition, char character)
        {
            _xPosition = SetValidPosition(xPosition, s_xMinPosition, s_xMaxPosition);
            _yPosition = SetValidPosition(yPosition, s_yMinPosition, s_yMaxPosition);
            _character = character;
        }

        public char Character { get { return _character; } }
        public int XPosition
        {
            get { return _xPosition; }
            private set {_xPosition = SetValidPosition(XPosition, s_xMinPosition, s_xMaxPosition); }
        }
        public int YPosition
        {
            get { return _yPosition; }
            private set { _yPosition = SetValidPosition(YPosition, s_yMinPosition, s_yMaxPosition); }
        }

        private int SetValidPosition(int position, int minValue, int maxValue)
        {
            if(position >= minValue && position <= maxValue)
            {
                return position;
            }

            return minValue;
        }
    }
}