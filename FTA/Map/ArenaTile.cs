using System;
using System.Collections.Generic;
using System.Text;

namespace FTA
{
    public class ArenaTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public ArenaTile Parent { get; set; }
        public int TileType { get; set; }

        public ArenaTile(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public ArenaTile(int x, int y, int tileType)
        {
            this.X = x;
            this.Y = y;
            this.TileType = tileType;
        }
    }
}
