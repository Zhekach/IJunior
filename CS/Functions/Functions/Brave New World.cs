using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Functions
{
    internal class BraveNewWorld
    {
        static void Main(string[] args)
        {
            const char PlayerMapChar = '&';
            const char MoveUpChar = 'w';

            Console.CursorVisible = false;

            char[,] map = CreateMap();

            int playerXPosition = 1;
            int playerYPosition = 1;
            int score = 0;

            bool isEditMode = false;
            bool isUserExited = false;

            ConsoleKeyInfo consoleKeyInfo;

            char pressedChar = MoveUpChar;

            Task.Run(() =>
            {
                while (isUserExited == false)
                {
                    isEditMode = Console.NumberLock;
                }
            });

            do
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Gray;
                PrintMap(map);
                PrintUI(isEditMode, pressedChar, score);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(playerXPosition, playerYPosition);
                Console.Write(PlayerMapChar);

                consoleKeyInfo = Console.ReadKey(true);

                pressedChar = consoleKeyInfo.KeyChar;
                isUserExited = (consoleKeyInfo.Key == ConsoleKey.Escape);

                HandlePlayerMovementComand(pressedChar, ref playerXPosition, ref playerYPosition, map, isEditMode, ref score);
            } while (isUserExited == false);
        }

        public static char[,] CreateMap()
        {
            char[,] plane = { { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
                            { 'x', ' ', 'x', '*', ' ', ' ', ' ', ' ', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', 'x', 'x', 'x', ' ', 'x' },
                            { 'x', ' ', ' ', ' ', 'x', ' ', ' ', 'x', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', ' ', ' ', 'x', ' ', 'x' },
                            { 'x', ' ', 'x', ' ', 'x', ' ', '*', 'x', ' ', 'x' },
                            { 'x', '*', 'x', ' ', 'x', ' ', 'x', 'x', ' ', 'x' },
                            { 'x', '*', 'x', ' ', 'x', ' ', ' ', ' ', ' ', 'x' },
                            { 'x', ' ', ' ', ' ', 'x', ' ', ' ', ' ', '*', 'x' },
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

        public static void PrintUI(bool isEditMode, char pressedKey, int score)
        {
            const char TreasureMapChar = '*';
            const char EmptyMapChar = ' ';
            const char WallMapChar = 'x';

            const char TreasureKeyChar = 'q';
            const char EmptyKeyChar = 'e';
            const char WallKeyChar = 'r';

            Console.SetCursorPosition(20, 2);
            if (isEditMode)
            {
                Console.Write("Mode = Edit");
            }
            else
            {
                Console.Write("Mode = Game");
            }

            Console.SetCursorPosition(20, 4);
            Console.Write("Score = " + score);

            Console.SetCursorPosition(20, 6);
            Console.Write("Pressed key = " + pressedKey);

            Console.SetCursorPosition(0, 12);
            Console.Write("Press NumLock to enter EditMode. \n\n" +
                            " In EditMode: \n" +
                            $" {TreasureKeyChar} - to add treasure ({TreasureMapChar}) \n" +
                            $" {EmptyKeyChar} - to add space ({EmptyMapChar}) \n" +
                            $" {WallKeyChar} - to add wall ({WallMapChar})\n" +
                       "\nPress Escape to exit.");
        }

        public static void HandlePlayerMovementComand(char pressedKey, ref int playerXPosition, ref int playerYPosition, char[,] map, bool isEditMode, ref int score)
        {
            int[] direction = GetDirection(pressedKey);

            if (isEditMode)
            {
                MovePlayerEditMode(direction, ref playerXPosition, ref playerYPosition, map);
                EditMap(pressedKey, ref playerXPosition, ref playerYPosition, map);
            }
            else
            {
                MovePlayerGameMode(direction, ref playerXPosition, ref playerYPosition, map, ref score);
            }
        }

        private static void EditMap(char pressedKey, ref int playerXPosition, ref int playerYPosition, char[,] map)
        {
            const char TreasureMapChar = '*';
            const char EmptyMapChar = ' ';
            const char WallMapChar = 'x';

            const char TreasureKeyChar = 'q';
            const char EmptyKeyChar = 'e';
            const char WallKeyChar = 'r';

            switch (pressedKey)
            {
                case TreasureKeyChar:
                    EditCharIn2DArray(TreasureMapChar, playerXPosition, playerYPosition, map);
                    break;
                case EmptyKeyChar:
                    EditCharIn2DArray(EmptyMapChar, playerXPosition, playerYPosition, map);
                    break;
                case WallKeyChar:
                    EditCharIn2DArray(WallMapChar, playerXPosition, playerYPosition, map);
                    break;
            }
        }

        private static void EditCharIn2DArray(char newChar, int charXPosition, int charYPosition, char[,] baseArray)
        {
            baseArray[charXPosition, charYPosition] = newChar;
        }

        private static void MovePlayerEditMode(int[] direction, ref int playerXPosition, ref int playerYPosition, char[,] map)
        {
            int nextPlayerXPosition = playerXPosition + direction[0];
            int nextPlayerYPosition = playerYPosition + direction[1];

            if (nextPlayerXPosition > 0 && nextPlayerXPosition < map.GetLength(0) - 1 &&
                nextPlayerYPosition > 0 && nextPlayerYPosition < map.GetLength(1) - 1)
            {
                playerXPosition = nextPlayerXPosition;
                playerYPosition = nextPlayerYPosition;
            }
        }

        private static void MovePlayerGameMode(int[] direction, ref int playerXPosition, ref int playerYPosition, char[,] map, ref int score)
        {
            const char TreasureMapChar = '*';
            const char EmptyMapChar = ' ';

            int nextPlayerXPosition = playerXPosition + direction[0];
            int nextPlayerYPosition = playerYPosition + direction[1];

            char nextCell = map[nextPlayerXPosition, nextPlayerYPosition];

            if (nextCell == EmptyMapChar || nextCell == TreasureMapChar)
            {
                playerXPosition = nextPlayerXPosition;
                playerYPosition = nextPlayerYPosition;

                if (nextCell == TreasureMapChar)
                {
                    map[nextPlayerXPosition, nextPlayerYPosition] = EmptyMapChar;
                    score++;
                }
            }
        }

        private static int[] GetDirection(char pressedKey)
        {
            const char MoveUpChar = 'w';
            const char MoveDownChar = 's';
            const char MoveLeftChar = 'a';
            const char MoveRightChar = 'd';

            int[] direction = { 0, 0 };

            switch (pressedKey)
            {
                case MoveUpChar:
                    direction[1] = -1;
                    break;
                case MoveDownChar:
                    direction[1] = 1;
                    break;
                case MoveLeftChar:
                    direction[0] = -1;
                    break;
                case MoveRightChar:
                    direction[0] = 1;
                    break;
            }

            return direction;
        }
    }
}