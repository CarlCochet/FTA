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

        public ArenaMap()
        {
            GenerateMap();
        }

        

        public void GenerateMap()
        {
            ArenaTile[,] map = new ArenaTile[Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y];
            bool[][] boolMap = Utils.CreateMap(Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y, true);
            Random random = new Random();

            for (int i = 0; i < (Utils.SIZE_MAP_X / 2); i++)
            {
                for (int k = 0; k < Utils.SIZE_MAP_Y; k++)
                {
                    int value = random.Next(100);
                    if      (value < 10)    { map[i, k] = new ArenaTile(i, k, 1); boolMap[i][k] = false; }
                    else if (value < 20)    { map[i, k] = new ArenaTile(i, k, 2); boolMap[i][k] = false; }
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
                    else if (map[decount, k].TileType == 2) { map[i, k] = new ArenaTile(i, k, 2); boolMap[i][k] = false; }
                    else                                    { map[i, k] = new ArenaTile(i, k, 0); }
                }
            }

            this.ArenaTiles = map;
            this.ArenaBool = boolMap;
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

        static int ComputeHScore(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }

        static List<ArenaTile> GetWalkableAdjacentTiles(int x, int y, ArenaTile[,] map)
        {
            List<ArenaTile> proposedTiles = new List<ArenaTile>();

            if (x + 1 < Utils.SIZE_MAP_X) { proposedTiles.Add(new ArenaTile(x + 1, y)); }
            if (x - 1 >= 0) { proposedTiles.Add(new ArenaTile(x - 1, y)); }
            if (y + 1 < Utils.SIZE_MAP_Y) { proposedTiles.Add(new ArenaTile(x, y + 1)); }
            if (y - 1 >= 0) { proposedTiles.Add(new ArenaTile(x, y - 1)); }

            return proposedTiles.Where(l => map[l.X, l.Y].TileType == 0).ToList();
        }

        public List<ArenaTile> FindPath(SFML.Graphics.RenderWindow window, int startX, int startY, int targetX, int targetY)
        {
            ArenaTile current = null;
            ArenaTile start = new ArenaTile(startX, startY);
            ArenaTile target = new ArenaTile(targetX, targetY);

            List<ArenaTile> openList = new List<ArenaTile>();
            List<ArenaTile> closedList = new List<ArenaTile>();
            List<ArenaTile> path = new List<ArenaTile>();

            int g = 0;

            /*var square = new SFML.Graphics.RectangleShape(new Vector2f(Constants.SQUARE_SIZE, Constants.SQUARE_SIZE))
            {
                FillColor = SFML.Graphics.Color.Red,
                OutlineColor = SFML.Graphics.Color.Black,
                OutlineThickness = 1f
            };*/

            openList.Add(start);
            bool pathFound = false;

            while (openList.Count > 0)
            {
                var lowest = openList.Min(l => l.F);
                current = openList.First(l => l.F == lowest);

                closedList.Add(current);
                openList.Remove(current);

                if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                {
                    pathFound = true;
                    break;
                }

                var adjacentTiles = GetWalkableAdjacentTiles(current.X, current.Y, this.ArenaTiles);

                /*foreach (ArenaTile tile in adjacentTiles)
                {
                    square.Position = new Vector2f(tile.X * Constants.SQUARE_SIZE, tile.Y * Constants.SQUARE_SIZE);
                    window.Draw(square);
                }*/

                g = current.G + 1;

                foreach (var adjacentTile in adjacentTiles)
                {
                    if (closedList.FirstOrDefault(l => l.X == adjacentTile.X && l.Y == adjacentTile.Y) != null) { continue; }

                    if (openList.FirstOrDefault(l => l.X == adjacentTile.X && l.Y == adjacentTile.Y) == null)
                    {
                        adjacentTile.G = g;
                        adjacentTile.H = ComputeHScore(adjacentTile.X, adjacentTile.Y, target.X, target.Y);
                        adjacentTile.F = adjacentTile.G + adjacentTile.H;
                        adjacentTile.Parent = current;

                        openList.Insert(0, adjacentTile);
                    }
                    else
                    {
                        if (g + adjacentTile.H < adjacentTile.F)
                        {
                            adjacentTile.G = g;
                            adjacentTile.F = adjacentTile.G + adjacentTile.H;
                            adjacentTile.Parent = current;
                        }
                    }
                }
            }

            if (pathFound)
            {
                while (current != null)
                {
                    path.Add(current);
                    current = current.Parent;
                }
            }

            return path;
        }

        public void RenderPath(SFML.Graphics.RenderWindow window, int startX, int startY, int targetX, int targetY)
        {
            List<ArenaTile> path = new List<ArenaTile>();

            if (ArenaTiles[targetX, targetY].TileType == 0)
            {
                path = FindPath(window, startX, startY, targetX, targetY);

                var square = new SFML.Graphics.RectangleShape(new Vector2f(Utils.SQUARE_SIZE, Utils.SQUARE_SIZE))
                {
                    FillColor = SFML.Graphics.Color.Green,
                    OutlineColor = SFML.Graphics.Color.Black,
                    OutlineThickness = Utils.OUTLINE_THICKNESS
                };

                foreach (ArenaTile tile in path)
                {
                    square.Position = new Vector2f(tile.X * Utils.SQUARE_SIZE, tile.Y * Utils.SQUARE_SIZE);
                    window.Draw(square);
                }
            }
        }

        public bool[][] Arena2Bool()
        {
            bool[][] map = Utils.CreateMap(Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y, false);

            return map;
        }
    }
}
