using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("-- Day 13 --");
            var input = File.ReadAllText("input/input.txt").Trim();
            var parts = input.Split("\r\n\r\n");

            List<int[]> points = new List<int[]>();
            List<int[]> folds = new List<int[]>();

            // Parse points
            foreach (var point in parts[0].Split("\r\n"))
            {
                points.Add(point.Split(",").Select(int.Parse).ToArray());
            }

            // Parse folds
            foreach (var fold in parts[1].Split("\r\n"))
            {
                string[] data = fold.Split(" ")[^1].Split("=");
                folds.Add(new[] {(data[0] == "x" ? 0 : 1), int.Parse(data[1])});
            }

            // Get paper size
            int[] size = points.Aggregate(new[] {0, 0}, (acc, point) =>
            {
                acc[0] = Math.Max(acc[0], point[0] + 1);
                acc[1] = Math.Max(acc[1], point[1] + 1);
                return acc;
            });

            // Mark dots
            bool[,] paper = new bool[size[1],size[0]];
            foreach (var point in points)
            {
                paper[point[1], point[0]] = true;
            }

            // Fold
            int[] first = folds[0];
            paper = FoldPaper(paper, first[1], first[0] == 1);
            Console.WriteLine($"Result part 1: {GetTotalDots(paper)}");
            
            for (int i=1;i<folds.Count;i++)
            {
                int[] fold = folds[i];
                paper = FoldPaper(paper, fold[1], fold[0] == 1);
            }
            
            Console.WriteLine("Result part 2: ");
            DrawPaper(paper);
        }

        private int GetTotalDots(bool[,] paper)
        {
            int total = 0;
            for (int y = 0; y < paper.GetLength(0); y++)
            {
                for (int x = 0; x < paper.GetLength(1); x++)
                {
                    total += paper[y, x] ? 1 : 0;
                }
            }

            return total;
        }
        
        private bool[,] FoldPaper(bool[,] paper, int position, bool vertical)
        {
            bool[,] newPaper;
            if (vertical)
            {
                newPaper = new bool[position, paper.GetLength(1)];
                for (int y = 0; y < position; y++)
                {
                    for(int x = 0; x < newPaper.GetLength(1); x++)
                    {
                        int index = position * 2 - y;
                        newPaper[y, x] = paper[y, x] || (paper.GetLength(0) > index && paper[index, x]);
                    }
                }
            }
            else
            {
                newPaper = new bool[paper.GetLength(0), position];
                for (int y = 0; y < newPaper.GetLength(0); y++)
                {
                    for(int x = 0; x < newPaper.GetLength(1); x++)
                    {
                        int index = position * 2 - x;
                        newPaper[y, x] = paper[y, x] || (paper.GetLength(1) > index && paper[y, index]);
                    }
                }
            }
            
            return newPaper;
        }
        
        private void DrawPaper(bool[,] paper)
        {
            for (int y = 0; y < paper.GetLength(0); y++)
            {
                for (int x = 0; x < paper.GetLength(1); x++)
                {
                    Console.Write(paper[y, x] ? "# " : ". ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
