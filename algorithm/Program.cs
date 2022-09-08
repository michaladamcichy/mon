using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace algorithm
{
    internal class Program
    {
        static void f(List<int> path)
        {
            path = new List<int> { 2 };
        }

        static void Main(string[] args)
        {
            var list = new List<int> { 1 };
            f(list);

            CostTest.Run();

            /*var d = new DoubleDictionary<int, double>();

            d[1, 2] = 2.0;
            d[2, 1] = 3.0;
            d[3, 1] = 3.0;

            Debug.Assert(d[1, 2] == d[2, 1]);*/

        }
    }
}
