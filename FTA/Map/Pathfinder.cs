using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.System;

namespace FTA
{
    public static class Pathfinder
    {
        // Simple A* algorithm to find shortest path quickly
        public static int FindPath(bool[][] map, Vector2i start, Vector2i end, out Vector2i[] path)
        {
            path = null;
            bool[][] closedMap = Utils.CreateMap(map.Length, map[0].Length, false);
            List<Vector2i> openList = new List<Vector2i> { start };

            Dictionary<Vector2i, float> scoreMap = new Dictionary<Vector2i, float>();
            scoreMap[start] = 0f;

            Dictionary<Vector2i, Vector2i> pathMap = new Dictionary<Vector2i, Vector2i>();

            Vector2i current, neighbor;
            int dx, dy;
            float newScore;

            while (openList.Count > 0)
            {
                openList = openList.OrderBy(n => scoreMap[n]).ToList();

                current = openList.First();
                openList.Remove(current);
                closedMap[current.X][current.Y] = true;
                if (current.Equals(end))
                {
                    List<Vector2i> pathlist = new List<Vector2i>();
                    while (pathMap.ContainsKey(current))
                    {
                        pathlist.Add(current);
                        current = pathMap[current];
                    }
                    pathlist.Add(current);
                    pathlist.Reverse();
                    path = pathlist.ToArray();
                    return closedMap.Sum(x => x.Count(y => y));
                }
                else
                {
                    for (dx = -1; dx <= 1; dx++)
                        for (dy = -1; dy <= 1; dy++)
                            if (Math.Abs(dx) == 1 ^ Math.Abs(dy) == 1)
                            {
                                neighbor = new Vector2i(current.X + dx, current.Y + dy);
                                if (PossibleMove(neighbor, map) && !closedMap[neighbor.X][neighbor.Y])
                                {
                                    if (!openList.Contains(neighbor))
                                        openList.Add(neighbor);

                                    newScore = scoreMap[current] + 1;
                                    if (!scoreMap.ContainsKey(neighbor) || newScore < scoreMap[neighbor])
                                    {
                                        scoreMap[neighbor] = newScore;
                                        pathMap[neighbor] = current;
                                    }
                                }
                            }
                }
            }
            path = new Vector2i[0];
            return closedMap.Sum(x => x.Count(y => y));
        }

        // Determines if a neighbor is a valid tile
        public static bool PossibleMove(Vector2i neighbor, bool[][] map)
        {
            if (neighbor.X >= 0 && neighbor.X < map.Length && neighbor.Y >= 0 && neighbor.Y < map[0].Length)
            {
                if (map[neighbor.X][neighbor.Y])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
