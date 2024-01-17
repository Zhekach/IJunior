using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Functions
{
    internal class BraveNewWorld
    {
        static void Main1(string[] args)
        {
            const char TreasureMapChar = '*';
            const char EmptyMapChar = ' ';
            const char WallMapChar = 'x';
            const char PlayerMapChar = '&';

            const char TreasureKeyChar = 'q';
            const char EmptyKeyChar = 'e';
            const char WallKeyChar = 'r';

            const char MoveUpChar = 'w';
            const char MoveDownChar = 's';
            const char MoveLeftChar = 'a';
            const char MoveRightChar = 'd';

            int playerXPosition = 1;
            int playerYPosition = 1;
            int score = 0;

            bool isEditMode = false;
            bool isUserExited = false;

            ConsoleKeyInfo consoleKeyInfo;

            char pressedChar = MoveUpChar;

            Console.CursorVisible = false;

            char[,] map = CreateMap();


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
                PrintUI(isEditMode, pressedChar, score, TreasureKeyChar, EmptyKeyChar, WallKeyChar, TreasureMapChar, EmptyMapChar, WallMapChar);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(playerXPosition, playerYPosition);
                Console.Write(PlayerMapChar);

                consoleKeyInfo = Console.ReadKey(true);

                pressedChar = consoleKeyInfo.KeyChar;
                isUserExited = (consoleKeyInfo.Key == ConsoleKey.Escape);

                HandlePlayerMovementComand(pressedChar, ref playerXPosition, ref playerYPosition, map, isEditMode, ref score,
                                            TreasureKeyChar, EmptyKeyChar, WallKeyChar, TreasureMapChar, EmptyMapChar, WallMapChar,
                                            MoveUpChar, MoveDownChar, MoveLeftChar, MoveRightChar);
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

        public static void PrintUI(bool isEditMode, char pressedKey, int score,
                                    char treasureKeyChar, char emptyKeyChar, char wallKeyChar,
                                    char treasureMapChar, char emptyMapChar, char wallMapChar)
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

            Console.SetCursorPosition(20, 4);
            Console.Write("Score = " + score);

            Console.SetCursorPosition(20, 6);
            Console.Write("Pressed key = " + pressedKey);

            Console.SetCursorPosition(0, 12);
            Console.Write("Press NumLock to enter EditMode. \n\n" +
                            " In EditMode: \n" +
                            $" {treasureKeyChar} - to add treasure ({treasureMapChar}) \n" +
                            $" {emptyKeyChar} - to add space ({emptyMapChar}) \n" +
                            $" {wallKeyChar} - to add wall ({wallMapChar})\n" +
                       "\nPress Escape to exit.");
        }

        public static void HandlePlayerMovementComand(char pressedKey, ref int playerXPosition, ref int playerYPosition, char[,] map, bool isEditMode, ref int score,
                                                       char treasureKeyChar, char emptyKeyChar, char wallKeyChar,
                                                       char treasureMapChar, char emptyMapChar, char wallMapChar,
                                                       char moveUpChar, char moveDownChar, char moveLeftChar, char moveRightChar)
        {
            int[] direction = GetDirection(pressedKey, moveUpChar, moveDownChar, moveLeftChar, moveRightChar);

            if (isEditMode)
            {
                MovePlayerEditMode(direction, ref playerXPosition, ref playerYPosition, map);
                EditMap(pressedKey, ref playerXPosition, ref playerYPosition, map, treasureKeyChar, emptyKeyChar, wallKeyChar, treasureMapChar, emptyMapChar, wallMapChar);
            }
            else
            {
                MovePlayerGameMode(direction, ref playerXPosition, ref playerYPosition, map, ref score,
                                   treasureMapChar, emptyMapChar);
            }
        }

        private static void EditMap(char pressedKey, ref int playerXPosition, ref int playerYPosition, char[,] map,
                                    char treasureKeyChar, char emptyKeyChar, char wallKeyChar,
                                    char treasureMapChar, char emptyMapChar, char wallMapChar)
        {
            if (pressedKey == treasureKeyChar)
            {
                EditCharIn2DArray(treasureMapChar, playerXPosition, playerYPosition, map);
            }
            else if (pressedKey == emptyKeyChar)
            {
                EditCharIn2DArray(emptyMapChar, playerXPosition, playerYPosition, map);
            }
            else if (pressedKey == wallKeyChar)
            {
                EditCharIn2DArray(wallMapChar, playerXPosition, playerYPosition, map);
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

        private static void MovePlayerGameMode(int[] direction, ref int playerXPosition, ref int playerYPosition, char[,] map, ref int score,
                                               char treasureMapChar, char emptyMapChar)
        {
            int nextPlayerXPosition = playerXPosition + direction[0];
            int nextPlayerYPosition = playerYPosition + direction[1];

            char nextCell = map[nextPlayerXPosition, nextPlayerYPosition];

            if (nextCell == emptyMapChar || nextCell == treasureMapChar)
            {
                playerXPosition = nextPlayerXPosition;
                playerYPosition = nextPlayerYPosition;

                if (nextCell == treasureMapChar)
                {
                    map[nextPlayerXPosition, nextPlayerYPosition] = emptyMapChar;
                    score++;
                }
            }
        }

        private static int[] GetDirection(char pressedKey, char moveUpChar, char moveDownChar, char moveLeftChar, char moveRightChar)
        {
            int[] direction = { 0, 0 };

            if (pressedKey == moveUpChar)
            {
                direction[1] = -1;
            }
            else if (pressedKey == moveDownChar)
            {
                direction[1] = 1;
            }
            else if (pressedKey == moveLeftChar)
            {
                direction[0] = -1;
            }
            else if (pressedKey == moveRightChar)
            {
                direction[0] = 1;
            }

            return direction;
        }
    }
}