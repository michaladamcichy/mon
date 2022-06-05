using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public interface IArrangeAlgorithm
    {
        List<Station> Run(Instance instance);
    }

    public class Algorithm
    {
        public static bool IsConnected(Instance instance)
        {
            return new ConnectionCheck().Run(instance);
        }
        public static bool IsConnected(List<Station> stations)
        {
            return new ConnectionCheck().Run(stations);
        }

        public static List<Station> NaiveArrange(Instance instance)
        {
            return new NaiveArrange().Run(instance);
        }

        public static List<Station> SimpleArrange(Instance instance)
        {
            return new SimpleArrange().Run(instance);
        }

        public static List<IDistancable> GreedySalesman(List<IDistancable> items)
        {
            return new GreedySalesman().Run(items);
        }

        //public static List<Station> SimpleArrangeAlgorithm(Instance instance)
        //{
        //    {
        //        var stations = new Greedy().Run(instance.Stations.Cast<IDistancable>().ToList()).Cast<Station>().ToList();
        //        var score = Salesman.Evaluate(stations.Cast<IDistancable>().ToList());
        //        //Trace.Write(stations.Count);
        //        Trace.WriteLine("Greedy: " + score.ToString());
        //        Trace.WriteLine("");
        //    }
        //    {
        //        Trace.WriteLine("Salesman: ");
        //        var stations = new Salesman().Run(instance.Stations.Cast<IDistancable>().ToList()).Cast<Station>().ToList();
        //        var score = Salesman.Evaluate(stations.Cast<IDistancable>().ToList());
        //        //Trace.Write(stations.Count);

        //        //Trace.WriteLine("");
        //        //Trace.Write(score);
        //        //Trace.WriteLine("\n\n");

        //        return stations;
        //    }
        //}
    }
}
