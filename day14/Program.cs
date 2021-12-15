using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace day14
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("-- Day 14 --");
            var input = File.ReadAllText("input/input.txt").Trim();
            var parts = input.Split("\r\n\r\n");

            var template = parts[0];

            var rules = new Dictionary<string, string>();
            var rulesList = parts[1].Split("\r\n").Select(x => x.Split(" -> ")).ToList();
            foreach (var rule in rulesList)
            {
                rules[rule[0]] = rule[1];
            }

            var templatePart1 = template;
            for (int i = 0; i < 10; i++)
            {
                templatePart1 = Step(rules, templatePart1);
            }

            var templateCount = templatePart1.ToCharArray().GroupBy(x => x).Aggregate(new Dictionary<string, int>(), (acc, x) =>
            {
                acc[x.Key.ToString()] = x.Count();
                return acc;
            });
            int result = templateCount.Max(x => x.Value) - templateCount.Min(x => x.Value);
            
            Console.WriteLine($"Result part 1: {result}");

            var pairCounter = new Dictionary<string, long>();
            foreach (var rule in rules)
            {
                pairCounter[rule.Key] = 0;
            }

            // Start pairs
            for (int i = 1; i < template.Length; i++)
            {
                string key = $"{template[i-1]}{template[i]}";
                pairCounter[key]++;
            }

            for (int i = 0; i < 40; i++)
            {
                pairCounter = StepOpt(rules, pairCounter);
            }

            var resultCounter = pairCounter.Aggregate(new Dictionary<string, long>(), (acc, x) =>
            {
                string key = x.Key[0].ToString();
                acc[key] = acc.GetValueOrDefault(key, 0) + x.Value;
                return acc;
            });
            resultCounter[template[^1].ToString()]++;
            
            var result2 = resultCounter.Max(x => x.Value) - resultCounter.Min(x => x.Value);
            
            Console.WriteLine($"Result part 2: {result2}");
        }

        private Dictionary<string, long> StepOpt(Dictionary<string, string> rules, Dictionary<string, long> pairs)
        {
            var result = pairs.ToDictionary(x => x.Key,
                x => 0L);
            foreach (var pair in pairs)
            {
                string newPairLeft = pair.Key[0] + rules[pair.Key];
                string newPairRight = rules[pair.Key] + pair.Key[1];
                result[newPairLeft] += pair.Value;
                result[newPairRight] += pair.Value;
                
            }

            return result;
        }
        
        private string Step(Dictionary<string, string> rules, string template)
        {
            string result = template[0].ToString();
            for (int i = 1; i < template.Length; i++)
            {
                string key = $"{template[i-1]}{template[i]}";
                if (rules.ContainsKey(key))
                {
                    result += rules[key];
                }

                result += template[i];
            }

            return result;
        }
    }
}
