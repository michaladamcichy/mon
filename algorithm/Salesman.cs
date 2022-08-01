using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
   /* public class Salesman
    {
        public static double Evaluate(List<MapObject> items)
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
        Dictionary<Tuple<MapObject, MapObject>, double> CreateMatrix(List<MapObject> items)
        {
            var matrix = new Dictionary<Tuple<MapObject, MapObject>, double>();

            foreach (var first in items)
            {
                foreach (var second in items)
                {
                    if (first == second) continue;

                    matrix[new Tuple<MapObject, MapObject>(first, second)] = first.GetDistance(second);
                }
            }

            return matrix; //alert
        }

        bool IsReady(List<MapObject> items, Dictionary<Tuple<MapObject, MapObject>, bool> mask)
        {
            var rows = new HashSet<MapObject>();
            var columns = new HashSet<MapObject>();

            foreach (var first in items)
            {
                foreach (var second in items)
                {
                    if (rows.Contains(first)) continue;
                    if (mask[new Tuple<MapObject, MapObject>(first, second)]) //ALERT SYMETRCZYNE TYLKO DZIAŁA
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

        void TraceEdges(List<Tuple<MapObject, MapObject>> edges)
        {
            var ids = new List<int>();
            edges.ForEach(item => { ids.Add(((Station)item.Item1).id); ids.Add(((Station)item.Item2).id); });

            foreach (var edge in edges)
            {
                Trace.WriteLine((((Station)edge.Item1).id - ids.Min()).ToString() + " -> " + (((Station)edge.Item2).id - ids.Min()).ToString());
            }
        }

        void TraceSchedule(List<MapObject> schedule)
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
        public List<MapObject> Run(List<MapObject> items)
        {
            var matrix = CreateMatrix(items);

            var tempMatrix = new Dictionary<Tuple<MapObject, MapObject>, double>(matrix);

            var itemsLeft = new List<MapObject>(items);

            var edges = new List<Tuple<MapObject, MapObject>>();

            var mask = new Dictionary<Tuple<MapObject, MapObject>, bool>();

            items.ForEach(first => items.ForEach(second => { mask[new Tuple<MapObject, MapObject>(first, second)] = false; }));

            var (praNodeFirst, praNodeSecond) = tempMatrix.Aggregate((first, second) => first.Value < second.Value ? first : second).Key; //alert

            while (!IsReady(items, mask))
            {
                (var first, var second) = tempMatrix.Aggregate((first, second) => first.Value < second.Value ? first : second).Key;

                tempMatrix.Remove(new Tuple<MapObject, MapObject>(first, second));
                mask[new Tuple<MapObject, MapObject>(first, second)] = true;

                edges.Add(new Tuple<MapObject, MapObject>(first, second));
            }

            var schedule = new List<MapObject>();

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

            //var unnecessaryEdges = new HashSet<Tuple<MapObject, MapObject>>();
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
                score += matrix[new Tuple<MapObject, MapObject>(schedule[i - 1], schedule[i])];
            }

            Trace.WriteLine(score);

            return items; //alert
        }
    }*/
}
