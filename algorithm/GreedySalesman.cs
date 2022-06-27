using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class GreedySalesman
    {
        public List<MapObject> Run(List<MapObject> items)
        {
            var solution = new List<MapObject>();

            var itemsLeft = new List<MapObject>(items);

            if (items.Count == 0) return new List<MapObject>();
            if(items.Count == 1)
            {
                solution.Add(items[0]);
                return solution;
            }
            var min = items.First();
            var val = items.First().GetDistanceFrom(items[1]); //alert!!!

            foreach (var f in items)
            {
                foreach (var s in items)
                {
                    if (f == s)
                    {
                        continue;
                    }

                    if (f.GetDistanceFrom(s) < val)
                    {
                        val = f.GetDistanceFrom(s);
                        min = f;
                    }
                }
            }

            var first = min;
            solution.Add(first);
            itemsLeft.Remove(first);

            while (itemsLeft.Count > 0)
            {
                var nearest = itemsLeft.Aggregate((item1, item2) => first.GetDistanceFrom(item1) < first.GetDistanceFrom(item2) ? item1 : item2);
                solution.Add(nearest);
                first = nearest;
                itemsLeft.Remove(nearest);
            }

            return solution;//alert
        }
    }
}
