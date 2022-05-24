using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class GreedySalesman
    {
        public List<IDistancable> Run(List<IDistancable> items)
        {
            var solution = new List<IDistancable>();

            var itemsLeft = new List<IDistancable>(items);

            if (items.Count == 0) return new List<IDistancable>();
            if(items.Count == 1)
            {
                solution.Add(items[0]);
                return solution;
            }
            var min = items.First();
            var val = items.First().GetDistance(items[1]); //alert!!!

            foreach (var f in items)
            {
                foreach (var s in items)
                {
                    if (f == s)
                    {
                        continue;
                    }

                    if (f.GetDistance(s) < val)
                    {
                        val = f.GetDistance(s);
                        min = f;
                    }
                }
            }

            var first = min;
            solution.Add(first);
            itemsLeft.Remove(first);

            while (itemsLeft.Count > 0)
            {
                var nearest = itemsLeft.Aggregate((item1, item2) => first.GetDistance(item1) < first.GetDistance(item2) ? item1 : item2);
                solution.Add(nearest);
                first = nearest;
                itemsLeft.Remove(nearest);
            }

            return solution;//alert
        }
    }
}
