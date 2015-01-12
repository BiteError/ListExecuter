using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ListExecuter
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Введите числа:");

                var list = Console.ReadLine().Split().Select(x => int.Parse(x)).ToList();
                var result = GetResult(list);
                Console.WriteLine(result.Aggregate("", (a, b) => a + " " + b).Trim());
            }
        }

        private static List<int> GetResult(List<int> list)
        {
            var directRoundedList = list;
            directRoundedList = RoundList(directRoundedList);
            var backRoundedList = list;
            backRoundedList.Reverse();
            backRoundedList = RoundList(backRoundedList);
            backRoundedList.Reverse();
            return backRoundedList.Count < directRoundedList.Count ? backRoundedList : directRoundedList;
        }

        private static List<int> RoundList(List<int> list)
        {
            while (true)
            {
                var result = GetNewList(list);
                if (list.SequenceEqual(result))
                    break;
                list = result;
            }
            return list;
        }

        private static List<int> GetNewList(List<int> list)
        {
            string str = list.Aggregate("", (a, b) => a + " " + b).Trim();
            str = Regex.Replace(str, @"(?:\b(\w+)\b) (?:\1(?: |$))+", "|$0|");
            var rr = str.Split('|')
                .Where(x => x.Length > 0)
                .Select(x => 
                    x.Split(' ')
                    .Where(y => y.Length > 0)
                    .Select(y => int.Parse(y))
                    .Where(z => z!= 0)
                    .ToList());
            var result = rr.Select(x =>
                x.Any(o => o != x[0]) 
                ? x 
                : MergePairs(x))
               .SelectMany(newList => newList)
               .ToList();
            return result;
        }

        private static IEnumerable<int> MergePairs(List<int> list)
        {
            var result = list.Where((n, i) => i % 2 == 0 && i != list.Count - 1).Select(r => r * 2).ToList();
            if (list.Count % 2 != 0)
            {
                result.Add(list[0]);
            }
            return result;
        }
    }
}
