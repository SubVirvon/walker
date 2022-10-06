using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace walker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char wallIcon = '#';
            char playerIcon = '0';
            char emptyElement = ' '; 
            char[,] map = CreateMap(wallIcon, playerIcon);

            Console.Clear();
            DrawMap(map);
            Walk(map, wallIcon, playerIcon, emptyElement);
        }

        static char[,] CreateMap(char wallIcon, char playerIcon)
        {
            const string CommandFinish = "finish";
            bool isFinish = false;
            string[] mapLayers = new string[0];

            Console.WriteLine($"Нарисуйте карту\nстены - {wallIcon}\nначальная позиция игрока - {playerIcon} (только одна)\nenter - следующий слой карты\n{CommandFinish} - закончить создание карыты");

            while (isFinish == false)
            {
                string input = Console.ReadLine();

                if (input != CommandFinish)
                {
                    string[] tempArray = new string[mapLayers.Length + 1];

                    for(int i = 0; i < mapLayers.Length; i++)
                    {
                        tempArray[i] = mapLayers[i];
                    }

                    mapLayers = tempArray;
                    mapLayers[mapLayers.Length - 1] = input;
                }
                else
                {
                    isFinish = true;
                }
            }

            char[,] map = new char[mapLayers.Length, mapLayers[0].Length];

            for(int i = 0; i < map.GetLength(0); i++)
            {
                for(int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = mapLayers[i][j];
                }
            }

            return map;
        }

        static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }
        }

        static void Walk(char[,] map, char wallIcon, char playerIcon, char emptyElement)
        {
            int positionX = 0;
            int positionY = 0;
            int directionX = 0;
            int directionY = 0;
            bool isPlaying = true;

            Console.CursorVisible = false;
            FindObject(map, ref positionX, ref positionY, playerIcon);

            while (isPlaying)
            {
                if (Console.KeyAvailable)
                {
                    Control(ref directionX, ref directionY);
                    Step(directionX, ref positionX, directionY, ref positionY, map, playerIcon, wallIcon, emptyElement);
                }
            }
        }

        static void FindObject(char[,] map, ref int positionX, ref int positionY, char icon)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == icon)
                    {
                        positionX = i;
                        positionY = j;
                        break;
                    }
                }
            }
        }

        static void Control (ref int directionX, ref int directionY)
        {
            const ConsoleKey KeyRight = ConsoleKey.RightArrow;
            const ConsoleKey KeyLeft = ConsoleKey.LeftArrow;
            const ConsoleKey KeyUp = ConsoleKey.UpArrow;
            const ConsoleKey KeyDown = ConsoleKey.DownArrow;

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case KeyUp:
                    directionX = -1;
                    directionY = 0;
                    break;
                case KeyDown:
                    directionX = 1;
                    directionY = 0;
                    break;
                case KeyLeft:
                    directionX = 0;
                    directionY = -1;
                    break;
                case KeyRight:
                    directionX = 0;
                    directionY = 1;
                    break;
            }
        }

        static void Step (int directionX, ref int positionX, int directionY, ref int positionY, char[,] map, char playerIcon, char wallIcon, char emptyElement)
        {
            if (map[positionX + directionX, positionY + directionY] != wallIcon)
            {
                Console.SetCursorPosition(positionY, positionX);
                Console.Write(emptyElement);
                positionX += directionX;
                positionY += directionY;
                Console.SetCursorPosition(positionY, positionX);
                Console.Write(playerIcon);
            }
        }
    }
}
