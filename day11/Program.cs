using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace day11
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("-- Day 11 --");
            var input = File.ReadAllText("input/input.txt").Trim();
            
            var grid = input.Split("\r\n").Select(x =>
            {
                return x.ToCharArray().Select(n => int.Parse(n.ToString())).ToArray();
            }).ToArray();

            int totalFlashes = 0;
            for (int i = 0; i < 100; i++)
            {
                totalFlashes += Step(grid);
            }
            
            Console.WriteLine($"Result part 1: {totalFlashes}");

            grid = input.Split("\r\n").Select(x =>
            {
                return x.ToCharArray().Select(n => int.Parse(n.ToString())).ToArray();
            }).ToArray();

            int step = 0;
            int firstStepAllFlash = -1;
            while (firstStepAllFlash < 0)
            {
                step++;
                totalFlashes = Step(grid);
                if (totalFlashes == grid.Length * grid[0].Length)
                {
                    firstStepAllFlash = step;
                }
            }
            
            Console.WriteLine($"Result part 2: {firstStepAllFlash}");
        }

        private void DrawGrid(int[][] grid)
        {
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    Console.Write(grid[y][x] + " ");
                }
                Console.WriteLine();
            }
        }

        private List<int[]> GetNeighbors(int[][] grid, int x, int y)
        {
            var list = new List<int[]>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int px = x + j;
                    int py = y + i;
                    if ((py==y && px==x) || py < 0 || py >= grid.Length
                        || px < 0 || px >= grid[py].Length || grid[py][px] == -1) continue;
                    list.Add(new[] {px, py});
                }
            }
            
            return list;
        }
        
        private int Step(int[][] grid)
        {
            var totalFlashed = 0;
            bool anyFlash;
            bool firstTick = true;
            do
            {
                anyFlash = false;
                for (int y = 0; y < grid.Length; y++)
                {
                    for (int x = 0; x < grid[y].Length; x++)
                    {
                        if (grid[y][x] >= 0)
                        {
                            if (firstTick) grid[y][x]++;
                            if (grid[y][x] > 9)
                            {
                                totalFlashed++;
                                grid[y][x] = -1;
                                var neighbors = GetNeighbors(grid, x, y);
                                foreach (var neighbor in neighbors)
                                {
                                    grid[neighbor[1]][neighbor[0]]++;
                                    if (grid[neighbor[1]][neighbor[0]] == 10)
                                    {
                                        anyFlash = true;
                                    }
                                }
                            }
                        }
                    }
                }

                firstTick = false;
            } while (anyFlash);

            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x] < 0) grid[y][x] = 0;
                }
            }

            return totalFlashed;
        }
    }
}
