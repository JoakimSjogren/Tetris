using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Shape
    {
        public int shapeType;
        public int rootPosition = Game.width + (Game.width/2);
        public int rotation = 0;

        public int colorInt;
        public static List<ConsoleColor> colorScheme = new List<ConsoleColor>() 
        {
        ConsoleColor.Green,
        ConsoleColor.Red,
        ConsoleColor.Yellow,
        ConsoleColor.White,
        ConsoleColor.DarkBlue,
        };
        public static List<List<int>> shapeTypes = new List<List<int>>
        {   
            new List<int>(){1, Game.width, Game.width+1},
            new List<int>(){-1, -Game.width, 1},
            new List<int>(){-Game.width, Game.width, Game.width*2},
            new List<int>(){1, 2, -Game.width},
            new List<int>(){-1, -2, -Game.width},
            new List<int>(){1, Game.width, Game.width-1},
            new List<int>(){-1, Game.width, Game.width+1},
        };
        public ConsoleColor color;

        public List<int> boxes = new List<int>();

        public Shape()
        {
            Random rnd = new Random();

            colorInt = rnd.Next(0, colorScheme.Count);
            color = colorScheme[colorInt];

            shapeType = rnd.Next(1, shapeTypes.Count +1);
            GenerateShape();
        }
        
        void GenerateShape()
        {
            if (shapeTypes.Count > shapeType-1)
            {
                var shapeTypeList = shapeTypes[shapeType - 1];
                for (int i = 0; i < shapeTypeList.Count; i++)
                {
                    boxes.Add(shapeTypeList[i]);
                }
            }
        }
    }
}

