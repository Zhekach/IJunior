using System;
using System.Threading;
using System.Threading.Tasks;

namespace Functions
{
    internal class BraveNewWorld
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            char[,] map = CreateMap();

            int playerX = 1;
            int playerY = 1;

            bool isEditMode = false;

            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('s', ConsoleKey.W, false, false, false);

            Task.Run(() =>
            {
                while (true)
                {
                    pressedKey = Console.ReadKey();
                }
            });

            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Gray;
                PrintMap(map);
                PrintUI(isEditMode, pressedKey);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(playerX, playerY);
                Console.Write("&");

                isEditMode = Console.NumberLock;
                HandleInput(pressedKey, ref playerX, ref playerY, map, isEditMode);

                Thread.Sleep(1000);
            }
        }

        public static char[,] CreateMap()
        {
            char[,] plane = { { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                            { 'x', ' ', 'x', ' ', ' ', ' ', ' ', ' ', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', 'x', 'x', 'x', ' ', 'x' },
                            { 'x', ' ', ' ', ' ', 'x', ' ', ' ', 'x', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', ' ', ' ', 'x', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', ' ', ' ', 'x', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', ' ', 'x', 'x', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', ' ', ' ', ' ', ' ', 'x' },
                            { 'x', ' ', ' ', ' ', 'x', ' ', ' ', ' ', ' ', 'x' },
                            { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }};

            char[,] map = new char[plane.GetLength(0), plane.GetLength(1)];

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = plane[y, x];
                }
            }

            return map;
        }

        public static void PrintMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.Write(map[x, y]);
                }

                Console.WriteLine();
            }
        }

        public static void PrintUI(bool isEditMode, ConsoleKeyInfo pressedKey)
        {
            Console.SetCursorPosition(20, 2);
            if (isEditMode)
            {
                Console.Write("Mode = Edit");
            }
            else
            {
                Console.Write("Mode = Game");
            }

            Console.SetCursorPosition (20, 4);
            Console.Write("Pressed key = " + pressedKey.KeyChar);
        }

        public static void HandleInput(ConsoleKeyInfo pressedKey, ref int playerX, ref int playerY, char[,] map, bool isEditMode)
        {
            int[] direction = GetDirection(pressedKey);

            if (isEditMode)
            {
                MoveInGameMode(direction, ref playerX, ref playerY, map);
            }
            else
            {
                MoveInGameMode(direction, ref playerX, ref playerY, map);
            }
        }

        private static void MoveInGameMode(int[] direction, ref int playerX, ref int playerY, char[,] map)
        {
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
