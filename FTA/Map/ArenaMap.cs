using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SFML.System;

namespace FTA.Map
{
    class ArenaMap
    {
        private ArenaTile[,] ArenaTiles;    // Stores ArenaTiles in order to have more infos than just booleans (for future spells)
        private bool[][] ArenaBool;         // Stores the map with booleans: every obstacles (low or high) is set to false (for PathFinding)
        private bool[][] ArenaLdV;          // Stores the map with booleans: only high obstacles are set to false (for Ligns of Sight)

        public ArenaMap()
        {
            GenerateMap();
        }

        // Generate a pseudo-random map with axial symmetry
        public void GenerateMap()
        {
            // Init
            ArenaTile[,] map = new ArenaTile[Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y];
            bool[][] boolMap = Utils.CreateMap(Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y, true);
            bool[][] ldvMap = Utils.CreateMap(Utils.SIZE_MAP_X, Utils.SIZE_MAP_Y, true);
            Random random = new Random();

            // First half of the map is pseudo-random
            for (int i = 0; i < (Utils.SIZE_MAP_X / 2); i++)
            {
                for (int k = 0; k < Utils.SIZE_MAP_Y; k++)
                {
                    // Set values for all 3 map layers
                    int value = random.Next(100);
                    if      (value < 10)    { map[i, k] = new ArenaTile(i, k, 1); boolMap[i][k] = false; }
                    else if (value < 20)    { map[i, k] = new ArenaTile(i, k, 2); boolMap[i][k] = false; ldvMap[i][k] = false; }
                    else                    { map[i, k] = new ArenaTile(i, k, 0); }
                }
            }

            // Second half only mirrors the first half
            int decount = Utils.SIZE_MAP_X / 2;
            for (int i = (Utils.SIZE_MAP_X / 2); i < Utils.SIZE_MAP_X; i++)
            {
                decount--;
                for (int k = 0; k < Utils.SIZE_MAP_Y; k++)
                {
                    // Set values for all 3 map layers
                    if (map[decount, k].TileType == 1) { map[i, k] = new ArenaTile(i, k, 1); boolMap[i][k] = false; }
                    else if (map[decount, k].TileType == 2) { map[i, k] = new ArenaTile(i, k, 2); boolMap[i][k] = false; ldvMap[i][k] = false; }
                    else                                    { map[i, k] = new ArenaTile(i, k, 0); }
                }
            }

            this.ArenaTiles = map;
            this.ArenaBool = boolMap;
            this.ArenaLdV = ldvMap;
        }

        // Renders the map
        public void Render(SFML.Graphics.RenderWindow window)
        {
            // Defines a square that is then displaced an rendered everywhere to draw the map (saves memory)
            var square = new SFML.Graphics.RectangleShape(new Vector2f(Utils.SQUARE_SIZE, Utils.SQUARE_SIZE))
            {
                OutlineColor = SFML.Graphics.Color.Black,
                OutlineThickness = 1f
            };

            // Draws the squares everywhere to create a grid
            for (int i = 0; i < Utils.SIZE_MAP_X; i++)
            {
                for (int k = 0; k < Utils.SIZE_MAP_Y; k++)
                {
                    square.Position = new Vector2f(i * Utils.SQUARE_SIZE, k * Utils.SQUARE_SIZE);

                    // Fill color is changed for obstacles
                    if (ArenaTiles[i, k].TileType == 1)         { square.FillColor = SFML.Graphics.Color.Black; }
                    else if (ArenaTiles[i, k].TileType == 2)    { square.FillColor = new SFML.Graphics.Color(128, 128, 128); }
                    else                                        { square.FillColor = SFML.Graphics.Color.White; }

                    window.Draw(square);
                }
            }
        }

        // Renders the path from start to target
        public void RenderPath(SFML.Graphics.RenderWindow window, int startX, int startY, int targetX, int targetY)
        {
            Vector2i[] path;
            int pathLen;

            if (ArenaTiles[targetX, targetY].TileType == 0)
            {
                pathLen = Pathfinder.FindPath(ArenaBool, new Vector2i(startX, startY), new Vector2i(targetX, targetY), out path);

                // Same as the map: only one square that is rendered in multiple places
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

        // Renders all LoS in a defined range
        public void RenderLOS(SFML.Graphics.RenderWindow window, int startX, int startY, int range)
        {
            // As always, only 1 square for multiple renders
            var square = new SFML.Graphics.RectangleShape(new Vector2f(Utils.SQUARE_SIZE, Utils.SQUARE_SIZE))
            {
                FillColor = new SFML.Graphics.Color(128, 128, 255),
                OutlineColor = SFML.Graphics.Color.Black,
                OutlineThickness = Utils.OUTLINE_THICKNESS
            };

            for (int i = 0; i < Utils.SIZE_MAP_X; i++)
            {
                for (int k = 0; k < Utils.SIZE_MAP_Y; k++)
                {
                    // Only calculate a LoS if the target is in range
                    if (ArenaBool[i][k] && (Math.Abs(startX - i) + Math.Abs(startY - k)) <= range) { 
                        if (LignOfSight.RayTracing(window, in ArenaLdV, in ArenaBool, startX, startY, i, k)) {
                            square.Position = new Vector2f(i * Utils.SQUARE_SIZE, k * Utils.SQUARE_SIZE);
                            window.Draw(square);
                        }
                    }
                }
            }
        }

        // Converts the arenamap to a boolean map (not used atm)
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
