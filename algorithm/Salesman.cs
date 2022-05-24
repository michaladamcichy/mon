using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class Salesman
    {
        public static double Evaluate(List<IDistancable> items)
        {
            var score = 0.0;

            if (items.Count == 0)
            {
                return 0.0;
            }

            var first = items.First();

            foreach (var item in items)
            {
                if (item == first) continue;

                score += first.GetDistance(item);

                first = item;
            }

            return score;
        }
        Dictionary<Tuple<IDistancable, IDistancable>, double> CreateMatrix(List<IDistancable> items)
        {
            var matrix = new Dictionary<Tuple<IDistancable, IDistancable>, double>();

            foreach (var first in items)
            {
                foreach (var second in items)
                {
                    if (first == second) continue;

                    matrix[new Tuple<IDistancable, IDistancable>(first, second)] = first.GetDistance(second);
                }
            }

            return matrix; //alert
        }

        bool IsReady(List<IDistancable> items, Dictionary<Tuple<IDistancable, IDistancable>, bool> mask)
        {
            var rows = new HashSet<IDistancable>();
            var columns = new HashSet<IDistancable>();

            foreach (var first in items)
            {
                foreach (var second in items)
                {
                    if (rows.Contains(first)) continue;
                    if (mask[new Tuple<IDistancable, IDistancable>(first, second)]) //ALERT SYMETRCZYNE TYLKO DZIAŁA
                    {
                        rows.Add(first);
                        if (rows.Count >= items.Count) return true;
                        rows.Add(second);
                        if (rows.Count >= items.Count) return true;
                    }
                }
            }

            return rows.Count >= items.Count;  //alert tu robię bez minus 1, a usuwam gdzie indziej ostatnią krawędź
        }

        void TraceEdges(List<Tuple<IDistancable, IDistancable>> edges)
        {
            var ids = new List<int>();
            edges.ForEach(item => { ids.Add(((Station)item.Item1).id); ids.Add(((Station)item.Item2).id); });

            foreach (var edge in edges)
            {
                Trace.WriteLine((((Station)edge.Item1).id - ids.Min()).ToString() + " -> " + (((Station)edge.Item2).id - ids.Min()).ToString());
            }
        }

        void TraceSchedule(List<IDistancable> schedule)
        {
            var ids = new List<int>();
            schedule.ForEach(item => { ids.Add(((Station)item).id); });

            Trace.WriteLine("");
            foreach (var item in schedule)
            {
                Trace.Write((((Station)item).id - ids.Min()).ToString() + " -> ");
            }
            Trace.WriteLine("");
        }
        public List<IDistancable> Run(List<IDistancable> items)
        {
            var matrix = CreateMatrix(items);

            var tempMatrix = new Dictionary<Tuple<IDistancable, IDistancable>, double>(matrix);

            var itemsLeft = new List<IDistancable>(items);

            var edges = new List<Tuple<IDistancable, IDistancable>>();

            var mask = new Dictionary<Tuple<IDistancable, IDistancable>, bool>();

            items.ForEach(first => items.ForEach(second => { mask[new Tuple<IDistancable, IDistancable>(first, second)] = false; }));

            var (praNodeFirst, praNodeSecond) = tempMatrix.Aggregate((first, second) => first.Value < second.Value ? first : second).Key; //alert

            while (!IsReady(items, mask))
            {
                (var first, var second) = tempMatrix.Aggregate((first, second) => first.Value < second.Value ? first : second).Key;

                tempMatrix.Remove(new Tuple<IDistancable, IDistancable>(first, second));
                mask[new Tuple<IDistancable, IDistancable>(first, second)] = true;

                edges.Add(new Tuple<IDistancable, IDistancable>(first, second));
            }

            var schedule = new List<IDistancable>();

            schedule.Add(praNodeFirst);

            TraceEdges(edges);
            while (schedule.Count < items.Count)
            {
                foreach (var edge in edges)
                {
                    if (edge.Item1 == schedule.Last() && !schedule.Contains(edge.Item2))
                    {
                        schedule.Add(edge.Item2);
                        continue;
                    }

                    if (edge.Item2 == schedule.Last() && !schedule.Contains(edge.Item1))
                    {
                        schedule.Add(edge.Item1);
                        continue;
                    }
                }
            }

            //var unnecessaryEdges = new HashSet<Tuple<IDistancable, IDistancable>>();
            //foreach (var edge1 in edges)
            //{
            //    foreach (var edge2 in edges)
            //    {
            //        if (edge1 != edge2 && edge1.Item1 == edge2.Item2 && edge2.Item1 == edge1.Item2)
            //        {
            //            unnecessaryEdges.Add(matrix[edge1] > matrix[edge2] ? edge1 : edge2);
            //        }
            //    }
            //}

            //unnecessaryEdges.ToList().ForEach(item => edges.Remove(item));

            var score = 0.0;

            TraceSchedule(schedule);
            //Trace.WriteLine("");
            for (var i = 1; i < schedule.Count; i++)
            {
                //Trace.Write((((Station) schedule[i]).id - ids.Min()).ToString() + " -> ");
                score += matrix[new Tuple<IDistancable, IDistancable>(schedule[i - 1], schedule[i])];
            }

            Trace.WriteLine(score);

            return items; //alert
        }
    }
}
