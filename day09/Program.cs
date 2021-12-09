using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day09
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("-- Day 09 --");
            var input = File.ReadAllText("input/input.txt").Trim();
            
            var heightmap = input.Split("\r\n").Select(x =>
            {
                var tmp = x.ToCharArray();
                return tmp.Select(x => int.Parse(x.ToString())).ToArray();
            }).ToArray();

            int totalLowPoints = 0;
            List<int> listBasin = new();
            for (int y = 0; y < heightmap.Length; y++)
            {
                for (int x = 0; x < heightmap[y].Length; x++)
                {
                    int value = heightmap[y][x];

                    Dictionary<string, int> neighbors = GetNeighbors(heightmap, x, y);

                    if (value < neighbors["left"]
                        && value < neighbors["right"]
                        && value < neighbors["up"]
                        && value < neighbors["down"])
                    {
                        totalLowPoints += value + 1;
                        HashSet<string> basin = FindBasin(heightmap, x, y);
                        listBasin.Add(basin.Count);
                    }
                }
            }
            
            Console.WriteLine($"Result part 1: {totalLowPoints}");

            listBasin.Sort();
            int totalBasin = listBasin.AsEnumerable().Reverse().Take(3).Aggregate(1, (acc, value) => acc * value);
            Console.WriteLine($"Result part 2: {totalBasin}");
        }

        public HashSet<string> FindBasin(int[][] heightmap, int x, int y)
        {
            HashSet<string> positions = new HashSet<string> {$"{x},{y}"};

            int value = heightmap[y][x];
            Dictionary<string, int> neighbors = GetNeighbors(heightmap, x, y);
            
            if (!positions.Contains($"{x-1},{y}") && neighbors["left"] < 9 && neighbors["left"] > value)
                positions.UnionWith(FindBasin(heightmap, x - 1, y));
            if (!positions.Contains($"{x+1},{y}") && neighbors["right"] < 9 && neighbors["right"] > value)
                positions.UnionWith(FindBasin(heightmap, x + 1, y));
            if (!positions.Contains($"{x},{y+1}") && neighbors["down"] < 9 && neighbors["down"] > value)
                positions.UnionWith(FindBasin(heightmap, x, y + 1));
            if (!positions.Contains($"{x},{y-1}") && neighbors["up"] < 9 && neighbors["up"] > value)
                positions.UnionWith(FindBasin(heightmap, x, y - 1));
            
            return positions;
        }

        public Dictionary<string, int> GetNeighbors(int[][] heightmap, int x, int y)
        {
            int left = (x == 0) ? int.MaxValue : heightmap[y][x - 1];
            int right = (x == heightmap[y].Length - 1) ? int.MaxValue : heightmap[y][x + 1];
            int up = (y == 0) ? int.MaxValue : heightmap[y - 1][x];
            int down = (y == heightmap.Length - 1) ? int.MaxValue : heightmap[y + 1][x];

            return new Dictionary<string, int>()
            {
                {"left", left},
                {"right", right},
                {"up", up},
                {"down", down},
            };
        }
    }
}
