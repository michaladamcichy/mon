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
            List<int> list = (new int[] { 1, 2, 3 }).ToList();

            foreach(var i in list)
            {
                Console.WriteLine(i);
                list.Add(i);
            }
        }
    }
}
