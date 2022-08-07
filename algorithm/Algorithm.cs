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
        public static bool IsConnected(List<Station> stations, int[] counts)
        {
            return new ConnectionCheck().Run(stations, counts);
        }

        public static List<Station> SimpleArrange(Instance instance)
        {
            return new SimpleArrange().Run(instance);
        }
        
        public static List<Station> PriorityArrange(Instance instance)
        {
            return new PriorityArrange().Run(instance);
        }
        public static List<Station> ArrangeWithExisting(Instance instance)
        {
            return new ArrangeWithExisting().Run(instance);
        }

        public static List<Station> SimpleOptimize(Instance instance)
        {
            return new SimpleOptimize().Run(instance);
        }

        //public static List<Station> SimpleArrangeAlgorithm(Instance instance)
        //{
        //    {
        //        var stations = new Greedy().Run(instance.Stations.Cast<MapObject>().ToList()).Cast<Station>().ToList();
        //        var score = Salesman.Evaluate(stations.Cast<MapObject>().ToList());
        //        //Trace.Write(stations.Count);
        //        Trace.WriteLine("Greedy: " + score.ToString());
        //        Trace.WriteLine("");
        //    }
        //    {
        //        Trace.WriteLine("Salesman: ");
        //        var stations = new Salesman().Run(instance.Stations.Cast<MapObject>().ToList()).Cast<Station>().ToList();
        //        var score = Salesman.Evaluate(stations.Cast<MapObject>().ToList());
        //        //Trace.Write(stations.Count);

        //        //Trace.WriteLine("");
        //        //Trace.Write(score);
        //        //Trace.WriteLine("\n\n");

        //        return stations;
        //    }
        //}
    }
}
