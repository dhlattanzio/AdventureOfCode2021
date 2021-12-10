using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        private Dictionary<char, char> chunks = new()
        {
            {'(', ')'},
            {'[', ']'},
            {'{', '}'},
            {'<', '>'}
        };
        
        private Dictionary<string, int> errorPoints = new()
        {
            {")", 3},
            {"]", 57},
            {"}", 1197},
            {">", 25137}
        };
        
        private Dictionary<string, int> autocompletePoints = new()
        {
            {")", 1},
            {"]", 2},
            {"}", 3},
            {">", 4}
        };
        
        public Program()
        {
            Console.WriteLine("-- Day 10 --");
            var input = File.ReadAllText("input/input.txt").Trim();
            var lines = input.Split("\r\n");

            int total = 0;
            foreach (var line in lines)
            {
                string result = CheckLine(line);
                total += errorPoints.GetValueOrDefault(result, 0);
            }
            
            Console.WriteLine($"Result part 1: {total}");

            var listPoints = new List<long>();
            foreach (var line in lines)
            {
                string result = CheckLine(line);
                if (result == "incomplete")
                {
                    var closes = GetCompleteSequence(line);
                    long points = 0;
                    foreach(char ch in closes)
                    {
                        points *= 5;
                        points += autocompletePoints[ch.ToString()];
                    }
                    listPoints.Add(points);
                }
            }

            listPoints.Sort();
            Console.WriteLine($"Result part 2: {listPoints[listPoints.Count / 2]}");
        }

        public string CheckLine(string line)
        {
            char error = GetFirstError(line);
            if (error == '0')
            {
                return "ok";
            }

            if (error == '1')
            {
                return "incomplete";
            }

            return error.ToString();
        }

        public char GetFirstError(string line)
        {
            var array = line.ToCharArray();
            var queue = new Stack<char>();

            foreach (var ch in array)
            {
                if (chunks.ContainsKey(ch))
                {
                    queue.Push(ch);
                }
                else
                {
                    var key = chunks.Single(x => x.Value == ch).Key;
                    char last = queue.Pop();
                    if (key != last)
                    {
                        return ch;
                    }
                }
            }

            return (queue.Count == 0) ? '0' : '1';
        }

        public List<char> GetCompleteSequence(string line)
        {
            var list = new List<char>();
            var array = line.ToCharArray();
            var queue = new Stack<char>();

            foreach (var ch in array)
            {
                if (chunks.ContainsKey(ch))
                {
                    queue.Push(ch);
                }
                else
                {
                    queue.Pop();
                }
            }

            while (queue.Count > 0)
            {
                var open = queue.Pop();
                list.Add(chunks[open]);
            }
            
            return list;
        }
    }
}
