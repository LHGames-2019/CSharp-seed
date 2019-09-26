using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Helper
{
    public static class HelperFunctions
    {

        /// <summary>
        /// ATTENTION: Do not modify
        /// Get the string value for the body assigned to a team number
        /// </summary>
        /// <param name="TeamNumber">number of the team</param>
        /// <returns></returns>
        public static char GetBodyStringByTeamNumber(int TeamNumber)
        {
            IDictionary<int, char> dict = new Dictionary<int, char>() { { 1, 'A' }, { 2, 'B' }, { 3, 'C' } , { 4, 'D' } } ;

            return dict[TeamNumber];
        }

        /// <summary>
        /// ATTENTION: Do not modify
        /// Get the string value for the Tail assigned to a team number
        /// </summary>
        /// <param name="TeamNumber">number of the team</param>
        /// <returns></returns>
        public static char GetTailStringByTeamNumber(int TeamNumber)
        {
            IDictionary<int, char> dict = new Dictionary<int, char>() { { 1, 'a' }, { 2, 'b' }, { 3, 'c' }, { 4, 'd' } };

            return dict[TeamNumber];
        }

        /// <summary>
        /// ATTENTION: Do not modify
        /// Get the number of tile that are part of a team body
        /// </summary>
        /// <param name="map">Map of the game</param>
        /// <param name="TeamNumber">Number that represent the team</param>
        /// <returns></returns>
        public static int GetSizeOfBodyByTeamNumber(string[] map, int TeamNumber)
        {
            return map.ToList().Where(s => s.Contains(GetBodyStringByTeamNumber(TeamNumber))).Count();
        }

        /// <summary>
        /// ATTENTION: Do not modify
        /// Get the number of tile that are part a team tail
        /// </summary>
        /// <param name="map">Map of the game</param>
        /// <param name="TeamNumber">Number that represent the team</param>
        /// <returns></returns>
        public static int GetSizeOfTailByTeamNumber(string[] map, int TeamNumber)
        {
            return map.ToList().Where(s => s.Contains(GetTailStringByTeamNumber(TeamNumber))).Count();
        }

        /// <summary>
        /// Get the position of a team on the map
        /// </summary>
        /// <param name="map">Map</param>
        /// <param name="dimension">dimension of a square map</param>
        /// <param name="teamNumber">Number of the team</param>
        /// <returns></returns>
        public static Point GetPositionByTeamNumber(string[] map, int dimension ,int teamNumber)
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = dimension * i; j < dimension * (i + 1); j++)
                {
                    if ( map[j].Contains(teamNumber.ToString()))
                    {
                        return new Point()
                        {
                            X = j  - (dimension * i),
                            Y = i
                        };
                    }
                }
            }

            return new Point()
            {
                X = -1,
                Y = -1
            };
        }

        /// <summary>
        /// ATTENTION: Do not modify
        /// Get a 2 dimensions version of the map. Map map need to be of a square dimension
        /// </summary>
        /// <param name="map">1 diemensions version of the map</param>
        /// <param name="dimension">Dimensions of a map</param>
        /// <returns></returns>
        public static List<List<string>> Get2DMap(string[] map, int dimension)
        {
            List<List<string>> newMap = new List<List<string>>();

            for (int i = 0; i < dimension; i++)
            {
                newMap.Add(new List<string>());
                for (int j = dimension * i; j < dimension * (i + 1); j++)
                {
                    newMap[i].Add(map[j]);
                }
            }

            return newMap;
        }

        /// <summary>
        /// ATTENTION: Do not modify
        /// Allow to print a 2D Map in the terminal
        /// </summary>
        /// <param name="map">2D Map</param>
        public static void Print2DMap(List<List<string>> map)
        {
            map.ForEach(row => {
                row.ForEach(s =>
                {
                    if (s == "")
                    {
                        Console.Write("--- ");
                    } else
                    {
                        Console.Write(s + " ");
                    }
                });

                Console.WriteLine("");
            });
        }

    }
}
