using System;
using System.Collections.Generic;
using System.Text;

namespace FTA
{
    public static class Utils
    {
        public const int WINDOW_WIDTH = 1600;
        public const int WINDOW_HEIGHT = 800;

        public const int SIZE_MAP_X = 32;
        public const int SIZE_MAP_Y = 16;

        public const int SQUARE_SIZE = WINDOW_WIDTH / SIZE_MAP_X;

        public const float OUTLINE_THICKNESS = 1f;

        public static bool[][] CreateMap(int width, int height, bool value)
        {
            bool[][] map = new bool[width][];
            for (int i = 0; i < width; i++)
            {
                map[i] = new bool[height];
                for (int k = 0; k < height; k++)
                {
                    map[i][k] = value;
                }
            }

            return map;
        }
    }
}
