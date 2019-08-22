using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SFML.System;

namespace FTA
{
    class ArenaMap
    {
        private ArenaTile[,] ArenaTiles;
        private bool[][] ArenaBool;
        private bool[][] ArenaLdV;

        public ArenaMap()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            ArenaTile[,] map = new ArenaTile[Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y];
            bool[][] boolMap = Utils.CreateMap(Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y, true);
            bool[][] ldvMap = Utils.CreateMap(Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y, true);
            Random random = new Random();

            for (int i = 0; i < (Utils.SIZE_MAP_X / 2); i++)
            {
                for (int k = 0; k < Utils.SIZE_MAP_Y; k++)
                {
                    int value = random.Next(100);
                    if      (value < 10)    { map[i, k] = new ArenaTile(i, k, 1); boolMap[i][k] = false; }
                    else if (value < 20)    { map[i, k] = new ArenaTile(i, k, 2); boolMap[i][k] = false; ldvMap[i][k] = false; }
                    else                    { map[i, k] = new ArenaTile(i, k, 0); }
                }
            }

            int decount = Utils.SIZE_MAP_X / 2;
            for (int i = (Utils.SIZE_MAP_X / 2); i < Utils.SIZE_MAP_X; i++)
            {
                decount--;
                for (int k = 0; k < Utils.SIZE_MAP_Y; k++)
                {
                    if      (map[decount, k].TileType == 1) { map[i, k] = new ArenaTile(i, k, 1); boolMap[i][k] = false; }
                    else if (map[decount, k].TileType == 2) { map[i, k] = new ArenaTile(i, k, 2); boolMap[i][k] = false; ldvMap[i][k] = false; }
                    else                                    { map[i, k] = new ArenaTile(i, k, 0); }
                }
            }

            this.ArenaTiles = map;
            this.ArenaBool = boolMap;
            this.ArenaLdV = ldvMap;
        }

        public void Render(SFML.Graphics.RenderWindow window)
        {
            var square = new SFML.Graphics.RectangleShape(new Vector2f(Utils.SQUARE_SIZE, Utils.SQUARE_SIZE))
            {
                OutlineColor = SFML.Graphics.Color.Black,
                OutlineThickness = 1f
            };

            StringBuilder strMap = new StringBuilder();

            for (int i = 0; i < Utils.SIZE_MAP_X; i++)
            {
                for (int k = 0; k < Utils.SIZE_MAP_Y; k++)
                {
                    square.Position = new Vector2f(i * Utils.SQUARE_SIZE, k * Utils.SQUARE_SIZE);

                    if      (ArenaTiles[i, k].TileType == 1)    { square.FillColor = SFML.Graphics.Color.Black; }
                    else if (ArenaTiles[i, k].TileType == 2)    { square.FillColor = new SFML.Graphics.Color(128, 128, 128); }
                    else                                        { square.FillColor = SFML.Graphics.Color.White; }

                    window.Draw(square);
                }
            }
        }

        public void RenderPath(SFML.Graphics.RenderWindow window, int startX, int startY, int targetX, int targetY)
        {
            Vector2i[] path;
            int pathLen;

            if (ArenaTiles[targetX, targetY].TileType == 0)
            {
                pathLen = Pathfinder.FindPath(ArenaBool, new Vector2i(startX, startY), new Vector2i(targetX, targetY), out path);

                var square = new SFML.Graphics.RectangleShape(new Vector2f(Utils.SQUARE_SIZE, Utils.SQUARE_SIZE))
                {
                    FillColor = SFML.Graphics.Color.Green,
                    OutlineColor = SFML.Graphics.Color.Black,
                    OutlineThickness = Utils.OUTLINE_THICKNESS
                };

                foreach (Vector2i tile in path)
                {
                    square.Position = new Vector2f(tile.X * Utils.SQUARE_SIZE, tile.Y * Utils.SQUARE_SIZE);
                    window.Draw(square);
                }
            }
        }

        public void RenderLOS(SFML.Graphics.RenderWindow window, int startX, int startY, int range)
        {
            var square = new SFML.Graphics.RectangleShape(new Vector2f(Utils.SQUARE_SIZE, Utils.SQUARE_SIZE))
            {
                FillColor = SFML.Graphics.Color.Blue,
                OutlineColor = SFML.Graphics.Color.Black,
                OutlineThickness = Utils.OUTLINE_THICKNESS
            };

            for (int i = 0; i < Utils.SIZE_MAP_X; i++)
            {
                for (int k = 0; k < Utils.SIZE_MAP_Y; k++)
                {
                    if (ArenaBool[i][k]) { 
                        if (LignOfSight.RayTracing(window, in ArenaLdV, in ArenaBool, startX, startY, i, k)) {
                            square.Position = new Vector2f(i * Utils.SQUARE_SIZE, k * Utils.SQUARE_SIZE);
                            window.Draw(square);
                        }
                    }
                }
            }
        }

        public bool[][] Arena2Bool()
        {
            bool[][] map = Utils.CreateMap(Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y, false);

            foreach (ArenaTile tile in ArenaTiles)
            {
                map[tile.X][tile.Y] = tile.TileType == 0;
            }

            return map;
        }
    }
}
