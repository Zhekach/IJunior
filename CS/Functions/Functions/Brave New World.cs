using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Functions
{
    internal class BraveNewWorld
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false);

            char[,] map = CreateMap();

            int playerX = 1;
            int playerY = 1;

            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Gray;
                PrintMap(map);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(playerX, playerY);
                Console.Write("&");

                pressedKey = Console.ReadKey();
                HandleInput(pressedKey, ref playerX, ref playerY, map);

                Thread.Sleep(1000);
            }
        }

        public static char[,] CreateMap()
        {
            char[,] map = { { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                            { 'x', ' ', 'x', ' ', ' ', ' ', ' ', ' ', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', 'x', 'x', 'x', ' ', 'x' },
                            { 'x', ' ', ' ', ' ', 'x', ' ', ' ', 'x', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', ' ', ' ', 'x', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', ' ', ' ', 'x', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', ' ', 'x', 'x', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', ' ', ' ', ' ', ' ', 'x' },
                            { 'x', ' ', ' ', ' ', 'x', ' ', ' ', ' ', ' ', 'x' },
                            { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }};
            return map;
        }

        public static void PrintMap(char[,] map)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Console.Write(map[x, y]);
                }

                Console.WriteLine();
            }
        }

        public static void HandleInput(ConsoleKeyInfo pressedKey, ref int playerX, ref int playerY, char[,] map)
        {
            int[] direction = GetDirection(pressedKey);

            int nextPlayerX = playerX + direction[0];
            int nextPlayerY = playerY + direction[1];

            char nextChar = map[nextPlayerX, nextPlayerY];

            if (nextChar == ' ')
            {
                playerX = nextPlayerX;
                playerY = nextPlayerY;
            }
        }

        private static int[] GetDirection(ConsoleKeyInfo pressedKey)
        {
            int[] direction = { 0, 0 };

            if (pressedKey.Key == ConsoleKey.W)
            {
                direction[1] = -1;
            }

            if (pressedKey.Key == ConsoleKey.S)
            {
                direction[1] = 1;
            }

            if (pressedKey.Key == ConsoleKey.D)
            {
                direction[0] = 1;
            }

            if (pressedKey.Key == ConsoleKey.A)
            {
                direction[0] = -1;
            }

            return direction;
        }
    }
}
