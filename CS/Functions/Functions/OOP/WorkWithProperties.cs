using System;

namespace Functions.OOP
{
    internal class WorkWithProperties
    {
        static void Main1(string[] args)
        {
            PlayerForRender player1 = new PlayerForRender(20,25);
            PlayerForRender player2 = new PlayerForRender(20,45);

            Renderer render1 = new Renderer();
            Renderer render2 = new Renderer('#');

            render1.PrintPlayer(player1);
            render2.PrintPlayer(player2);
        }
    }

    class Renderer
    {
        private char _character = '&';

        public Renderer(char character) 
        {
            this._character = character;
        }

        public Renderer() { }

        public void PrintPlayer(PlayerForRender player)
        {
            Console.SetCursorPosition(player.XPosition, player.YPosition);
            Console.WriteLine(_character);
        }
    }

    class PlayerForRender
    {
        private int _xPosition;
        private int _yPosition;
        private int _xMaxPosition = 100;
        private int _yMaxPosition = 150;

        public PlayerForRender(int xPosition, int yPosition)
        {
            _xPosition = xPosition;
            _yPosition = yPosition;
        }

        public int XPosition
        {
            get { return _xPosition; }
            private set
            {
                if (value > 0 && value < _xMaxPosition)
                {
                    _xPosition = value;
                }
            }
        }
        public int YPosition
        {
            get { return _yPosition; }
            private set
            {
                if (value > 0 && value < _yMaxPosition)
                {
                    _yPosition = value;
                }
            }
        }
    }
}