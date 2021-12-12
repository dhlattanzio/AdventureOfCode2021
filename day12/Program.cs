using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace day12
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("-- Day 12 --");
            var input = File.ReadAllText("input/input.txt").Trim();
            var lines = input.Split("\r\n");

            var caves = new Dictionary<string, HashSet<string>>();
            foreach (var line in lines)
            {
                var fromTo = line.Split("-");
                
                HashSet<string> connected = caves.GetValueOrDefault(fromTo[0], new HashSet<string>());
                connected.Add(fromTo[1]);
                caves[fromTo[0]] = connected;
                
                HashSet<string> connectedOther = caves.GetValueOrDefault(fromTo[1], new HashSet<string>());
                connectedOther.Add(fromTo[0]);
                caves[fromTo[1]] = connectedOther;
            }

            var paths = GetAllCavePaths(caves);
            Console.WriteLine($"Result part 1: {paths.Count}");
            
            paths = GetAllCavePathsPart2(caves);
            Console.WriteLine($"Result part 2: {paths.Count}");
        }

        private bool IsBigCave(string cave)
        {
            return cave != cave.ToLower();
        }

        private List<List<string>> GetAllCavePaths(Dictionary<string, HashSet<string>> caves,
            string fromCave = "start", List<string> path = null)
        {
            path ??= new List<string>();
            path.Add(fromCave);
            
            var result = new List<List<string>>();
            if (fromCave == "end")
            {
                result.Add(path);
                return result;
            }
            
            var toCaves = caves.GetValueOrDefault(fromCave, new())
                .Where(x => IsBigCave(x) || !path.Contains(x)).ToArray();

            foreach (var cave in toCaves)
            {
                var lists = GetAllCavePaths(caves, cave, new List<string>(path));
                result.AddRange(lists);
            }

            return result;
        }
        
        private List<List<string>> GetAllCavePathsPart2(Dictionary<string, HashSet<string>> caves,
            string fromCave = "start", List<string> path = null, bool canVisitTwice = true)
        {
            path ??= new List<string>();
            path.Add(fromCave);
            
            var result = new List<List<string>>();
            if (fromCave == "end")
            {
                result.Add(path);
                return result;
            }
            
            var toCaves = caves.GetValueOrDefault(fromCave, new())
                .Where(x => x != "start")
                .Where(x => IsBigCave(x) || !path.Contains(x) || canVisitTwice).ToArray();

            foreach (var cave in toCaves)
            {
                var lists = GetAllCavePathsPart2(caves, cave, new List<string>(path), 
                    canVisitTwice && (IsBigCave(cave) || !path.Contains(cave)));
                result.AddRange(lists);
            }

            return result;
        }
        
    }
}
