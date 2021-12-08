using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("-- Day 5 --");
            string input = File.ReadAllText("input/input.txt").Trim();
            string[] lines = input.Split("\r\n");

            Dictionary<int, int> diagram = new();
            foreach (var line in lines)
            {
                string[] vents = line.Split(" -> ");
                foreach (var coord in GetAllPointsOfVent(vents[0], vents[1], ignoreDiagonal: true))
                {
                    diagram[coord.Value] = diagram.GetValueOrDefault(coord.Value, 0) + 1;
                }
            }

            int result = 0;
            foreach (var point in diagram.Values)
            {
                result += point > 1 ? 1 : 0;
            }
            
            Console.WriteLine("Result part 1: "+result);
            
            diagram = new();
            foreach (var line in lines)
            {
                string[] vents = line.Split(" -> ");
                foreach (var coord in GetAllPointsOfVent(vents[0], vents[1], ignoreDiagonal: false))
                {
                    diagram[coord.Value] = diagram.GetValueOrDefault(coord.Value, 0) + 1;
                }
            }

            result = 0;
            foreach (var point in diagram.Values)
            {
                result += point > 1 ? 1 : 0;
            }
            
            Console.WriteLine("Result part 2: "+result);
        }

        private List<Coordinate> GetAllPointsOfVent(string vent1, string vent2, bool ignoreDiagonal = false)
        {
            List<Coordinate> list = new List<Coordinate>();
            int[] tmp = vent1.Split(",").Select(int.Parse).ToArray();
            var coord1 = new Coordinate {X = tmp[0], Y = tmp[1]};
            tmp = vent2.Split(",").Select(int.Parse).ToArray();
            var coord2 = new Coordinate {X = tmp[0], Y = tmp[1]};

            if (ignoreDiagonal && !(coord1.X == coord2.X || coord1.Y == coord2.Y))
                return list;

            while (coord1.Value != coord2.Value)
            {
                list.Add(new Coordinate {X = coord1.X, Y = coord1.Y});
                if (coord1.X != coord2.X) coord1.X += coord1.X < coord2.X ? 1 : -1;
                if (coord1.Y != coord2.Y) coord1.Y += coord1.Y < coord2.Y ? 1 : -1;
            }
            list.Add(new Coordinate {X = coord1.X, Y = coord1.Y});

            return list;
        }

        private class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Value => Y * 1000000 + X;
        }
    }
}
