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

        public static List<Station> NaiveArrange(Instance instance)
        {
            return new NaiveArrange().Run(instance);
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
    }
}
