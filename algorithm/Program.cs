using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> list1 = (new int[] { 2, 3, 4 }).ToList();
            List<int> list2 = (new int[] { 2, 3, 4 }).ToList();

            var min = list1.Min(first => list2.Min(second => first * second));
            var max = list1.Max(first => list2.Max(second => first * second));

            Console.WriteLine(min);
            Console.WriteLine(max);

        }
    }
}
