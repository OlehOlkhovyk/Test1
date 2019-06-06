using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        sealed class ArrayEqualityComparer : IEqualityComparer<string[]>
        {
            public bool Equals(string[] x, string[] y)
            {
                if (x == null && y == null)
                    return true;
                if (x != null && y != null)
                    return x.SequenceEqual(y);
                return false;
            }

            public int GetHashCode(string[] obj)
            {
                int ret = 0;
                foreach (var item in obj)
                {
                    ret += item.GetHashCode();
                }
                return ret;
            }
        }

        static void Main(string[] args)
        {
            List<Item> items;
            try
            {
                using (StreamReader r = new StreamReader(@"Test task #1 - Pizzas.json"))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Item>>(json);
                }
                var groups = items.GroupBy(t => t.toppings, new ArrayEqualityComparer())
                          .Select(g => new { g.Key, Count = g.Count() })
                          .OrderByDescending(r => r.Count)
                          .Take(20)
                          .ToList();
                foreach (var item in groups)
                {
                    Console.WriteLine($"count:{item.Count} toppings:");
                    foreach (var key in item.Key)
                    {
                        Console.WriteLine(key);
                    }
                    Console.WriteLine("---------------------");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error!");
            }
            Console.ReadKey();
        }
    }
    public class Item
    {
        public string[] toppings { get; set; }
    }
}