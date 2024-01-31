using System;

namespace Functions.OOP
{
    internal class WorkWithProperties
    {
        static void Main(string[] args)
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
        private int _xPosition;
        private int _yPosition;

        public Player(int xPosition, int yPosition, char character)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            Character = character;
        }

        public char Character { get ; private set; } 
        public int XPosition
        {
            get => _xPosition;
            private set 
            {
                int xMaxPosition = 100;
                int xMinPosition = 0;

                _xPosition = GetValidPosition(value, xMinPosition, xMaxPosition); 
            }
        }
        public int YPosition
        {
            get => _yPosition;
            private set 
            {
                int yMaxPosition = 150;
                int yMinPosition = 0;

                _yPosition = GetValidPosition(value, yMinPosition, yMaxPosition); 
            }
        }

        private int GetValidPosition(int position, int minValue, int maxValue)
        {
            if(position >= minValue && position <= maxValue)
            {
                return position;
            }

            return minValue;
        }
    }
}