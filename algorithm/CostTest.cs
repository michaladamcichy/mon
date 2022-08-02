using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    internal class CostTest
    {
        public static void Run()
        {
            //public void AddForbiddenRange(double range)
            //public void RemoveAllForbiddenRanges()
            {
                Instance instance = new Instance(new int[] { 1000, 1000, 1000 });
                Cost cost = new Cost(instance);
                var r = cost.GetMax();
                Debug.Assert(r == 50.0);
                cost.AddForbiddenRange(50.0);
                r = cost.GetMax();
                Debug.Assert((r == 30.0));
                cost.AddForbiddenRange(30.0);
                r = cost.GetMax();
                Debug.Assert(r == 20.0);
                cost.AddForbiddenRange(20.0);
                r = cost.GetMax();
                Debug.Assert(r == null);
                cost.RemoveAllForbiddenRanges();
                r = cost.GetMax();
                Debug.Assert(r == 50.0);
            }
            //public bool CanGet(double range, int count = 1)
            {
                Instance instance = new Instance(new int[] { 1000, 1000, 1000 });
                Cost cost = new Cost(instance);
                Debug.Assert(cost.CanGet(50.0, 1) == true);
                Debug.Assert(cost.CanGet(30.0, 1) == true);
                Debug.Assert(cost.CanGet(20.0, 1) == true);
                Debug.Assert(cost.CanGet(50.0, 1000) == true);
                Debug.Assert(cost.CanGet(50.0, 1001) == false);
            }
            //public bool Get(double range, int count = 1)
            {
                Instance instance = new Instance(new int[] { 1000, 1000, 1000 });
                Cost cost = new Cost(instance);
                Debug.Assert(cost.Get(50.0, 1) == true);
                Debug.Assert(cost.Get(30.0, 1) == true);
                Debug.Assert(cost.Get(20.0, 1) == true);
                Debug.Assert(cost.Get(50.0, 999) == true);
                Debug.Assert(cost.Get(50.0, 1) == false);
                Debug.Assert(cost.Get(50.0, 1001) == false);
                Debug.Assert(cost.Get(20.0, 1) == true);
                cost.AddForbiddenRange(20.0);
                Debug.Assert(cost.Get(20.0) == false);
                cost.AddForbiddenRange(30.0);
                Debug.Assert(cost.Get(30.0) == false);
                cost.AddForbiddenRange(50.0);
                Debug.Assert(cost.Get(50.0) == false);

            }
            //public double? GetMax(int count = 1, double maxRange = double.MaxValue)
            {
                Instance instance = new Instance(new int[] { 5, 5, 5 });
                Cost cost = new Cost(instance);
                Debug.Assert(cost.GetMax(3, 30.0) == 30.0);
                Debug.Assert(cost.GetMax(3, 30.0) == 20.0);
                Debug.Assert(cost.GetMin(5, 50.0) == 50.0);
                Debug.Assert(cost.GetMin(5, 50.0) == null);
            }
            {
                Instance instance = new Instance(new int[] { 5, 5, 5 });
                Cost cost = new Cost(instance);
                var group = new Group();
                
                for(var i = 0; i < 10; i++)
                {
                    group.Add(new Station(50.0));
                }



            }
            //public double? GetMin(int count = 1, double minRange = 0.0)
            //public double? QueryMax(int count = 1, double maxRange = double.MaxValue)
            //public double? QueryMin(int count = 1, double minRange = 0.0)
            //public double? GetMinCoveringRange(List<Station> stations)
            //public double? QueryMinCoveringRange(List<Station> stations)
            //public bool CanGetAny()
            //public bool CanGetAny(int count)
            //public bool CanMakeBigger(Station station)
            //public bool CanMakeSmaller(Station station)
            //public bool MakeBigger(Station station) //alert niekonsekwentnie - zwracaj bool czy sie udało
            //public bool MakeSmaller(Station station)
            //public double? QueryMakeBigger(Station station)
            //public double? QueryMakeSmaller(Station station)
            //public bool CanChangeRange(Station station, double newRange)
            //public bool CanChangeRange(List<Station> stations, double newRange)
            //public bool ChangeRange(Station station, double newRange)
            //void GiveBack(double range, int count = 1)
        }
    }
}