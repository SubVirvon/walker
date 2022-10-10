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
            int maxLineLength = int.MinValue;

            Console.WriteLine($"Нарисуйте карту\nстены - {wallIcon}\nначальная позиция игрока - {playerIcon} (только одна)\nenter - следующий слой карты\n{CommandFinish} - закончить создание карыты");

            while (isFinish == false)
            {
                string input = Console.ReadLine();

                if (input != CommandFinish)
                {
                    if (maxLineLength < input.Length)
                    {
                        maxLineLength = input.Length;
                    }

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

            AlignLayers(mapLayers, maxLineLength, wallIcon);

            char[,] map = new char[mapLayers.Length, mapLayers[0].Length];

            for(int i = 0; i < map.GetLength(0); i++)
            {
                for(int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = mapLayers[i][j];
                }
            }

            CreateFrame(ref map, wallIcon);

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
                    GetDirections(ref directionX, ref directionY);
                    if (map[positionX + directionX, positionY + directionY] != wallIcon)
                    {
                        Move(directionX, ref positionX, directionY, ref positionY, playerIcon, emptyElement);
                    }
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

        static void GetDirections(ref int directionX, ref int directionY)
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

        static void Move(int directionX, ref int positionX, int directionY, ref int positionY, char playerIcon, char emptyElement)
        {
            
            Console.SetCursorPosition(positionY, positionX);
            Console.Write(emptyElement);
            positionX += directionX;
            positionY += directionY;
            Console.SetCursorPosition(positionY, positionX);
            Console.Write(playerIcon);
        }

        static void AlignLayers (string[] layers, int maxLength, char wallIcon)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].Length < maxLength)
                {
                    int cyclesCount = maxLength - layers[i].Length;

                    for (int j = 0; j < cyclesCount; j++)
                    {
                        layers[i] += wallIcon;
                    }
                }
            }
        }

        static void CreateFrame(ref char[,] map, char wallIcon)
        {
            char[,] tempArray = new char[map.GetLength(0) + 2, map.GetLength(1) + 2];

            for (int i = 0; i < tempArray.GetLength(0); i++)
            {
                for(int j = 0; j < tempArray.GetLength(1); j++)
                {
                    if(i == 0 || i == tempArray.GetLength(0) - 1 || j == 0 || j == tempArray.GetLength(1) - 1)
                    {
                        tempArray[i, j] = wallIcon;
                    }
                    else
                    {
                        tempArray[i, j] = map[i - 1, j - 1];
                    }
                }
            }

            map = tempArray;
        }
    }
}
