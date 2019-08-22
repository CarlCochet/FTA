using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;

namespace FTA
{
    public static class LignOfSight
    {
        public static bool RayTracing(bool[][] map, int startX, int startY, int targetX, int targetY) 
        {
            var dx = Math.Abs(startX - targetX);
            var dy = Math.Abs(startY - targetY);
            var x = startX;
            var y = startY;
            var n = 1 + dx + dy;
            var vectorX = (targetX > startX) ? 1 : -1;
            var vectorY = (targetY > startY) ? 1 : -1;
            var error = dx - dy;
            dx *= 2;
            dy *= 2;

            while (n > 0 && map[x][y])
            {
                if (error > 0)
                {
                    x += vectorX;
                    error -= dy;
                }
                else if (error == 0)
                {
                    x += vectorX;
                    y += vectorY;
                    n--;
                    error += dx - dy;
                }
                else
                {
                    y += vectorY;
                    error += dx;
                }

                if (x < Utils.SIZE_MAP_X & x >= 0 && y < Utils.SIZE_MAP_Y && y >= 0)
                {
                    if (map[x][y])
                    {
                        Console.WriteLine("Break!");
                        break;
                    }
                }
            }

            return (x == targetX && y == targetY);

        }
    }
}
