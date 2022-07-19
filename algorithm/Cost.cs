using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class Cost
    {
        public Instance Instance { get; }
        public int[] Counts { get; private set; } = new int[] { 0, 0, 0 };

        public double[] Ranges { get { return Instance.StationRanges; } }

        public HashSet<double> ForbiddenRanges { get; private set; } = new HashSet<double>();
        //public List<Station> Stations { get; } = new List<Station>();
        public Cost(Instance instance)
        {
            this.Instance = instance;
            Counts = (int[]) Instance.StationCounts.Clone(); //alert czy to działa?
        }

        public Cost(Cost cost)
        {
            this.Instance = cost.Instance;
            Counts = (int[])(cost.Counts.Clone());
            ForbiddenRanges = new HashSet<double>(ForbiddenRanges);
        }

        public void AddForbiddenRange(double range)
        {
            ForbiddenRanges.Add(range);
        }

        public void RemoveAllForbiddenRanges()
        {
            ForbiddenRanges.Clear();
        }

        public bool CanGet(double range, int count = 1)
        {
            var index = Instance.StationRanges.ToList().IndexOf(range);
            if (Counts[index] - count < 0 || ForbiddenRanges.Contains(range)) return false;
            return true;
        }

        public bool Get(double range, int count = 1)
        {
            var index = Instance.StationRanges.ToList().IndexOf(range);
            if (Counts[index] - count < 0 || ForbiddenRanges.Contains(range)) return false;
            Counts[index] -= count;
            return true;
        }

        public double? GetMax(int count = 1, double maxRange = double.MaxValue)
        {
            var max = QueryMax(count, maxRange);
            if(max == null) return null;

            Counts[Instance.StationRanges.ToList().IndexOf(max.Value)] -= count;
            return max;
        }

        public double? GetMin(int count = 1, double minRange = 0.0)
        {
            var min = QueryMin(count, minRange);
            if (min == null) return null;

            Counts[Instance.StationRanges.ToList().IndexOf(min.Value)] -= count;
            return min;
        }

        public double? QueryMax(int count = 1, double maxRange = double.MaxValue)
        {
            for (var i = Instance.StationCounts.Count() - 1; i >= 0; i--)
            {
                if (Counts[i] - count < 0 || Instance.StationRanges[i] > maxRange || ForbiddenRanges.Contains(maxRange)) continue;
                return Instance.StationRanges[i];
            }
            return null;
        }

        public double? QueryMin(int count = 1, double minRange = 0.0)
        {
            for (var i = 0; i < Instance.StationCounts.Count(); i++)
            {
                if (Counts[i] - count < 0 || Instance.StationRanges[i] < minRange || ForbiddenRanges.Contains(minRange)) continue;
                return Instance.StationRanges[i];
            }
            return null;
        }

        public double? GetMinCoveringRange(List<Station> stations)
        {
            var minCoveringRange = QueryMinCoveringRange(stations);
            if (minCoveringRange == null) return null;

            Counts[Instance.StationRanges.ToList().IndexOf(minCoveringRange.Value)]--;
            return minCoveringRange.Value;
        }

        public double? QueryMinCoveringRange(List<Station> stations)
        {
            var minCoveringCircleRadius = MapObject.MinCoveringCircleRadius(stations);

            for (var i = 0; i < Instance.StationCounts.Count(); i++)
            {
                if (Instance.StationRanges[i] >= minCoveringCircleRadius && Counts[i] > 0 && !ForbiddenRanges.Contains(Instance.StationRanges[i]))
                {
                    return Instance.StationRanges[i];
                }
            }
            return null;
        }

        public bool CanGetAny()
        {
            for (var i = 0; i < Instance.StationRanges.Count(); i++)
            {
                if (Counts[i] > 0 && !ForbiddenRanges.Contains(Instance.StationRanges[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanGetAny(int count)
        {
            var cost = new Cost(this);

            for(var i = 0; i < count; i++)
            {
                var range = cost.GetMin();
                if (range == null) return false;
            }
            return true;
        }

        /*     public bool CanGetAny(int count = 1)
             {
                 int howManyLeft = count;

                 while(howManyLeft > 0)
                 {
                     var lastHowManyLeft = howManyLeft;
                     for (var i = 0; i < Instance.StationRanges.Count(); i++)
                     {
                         if (Counts[i] > 0 && !ForbiddenRanges.Contains(Instance.StationRanges[i]))
                         {
                             howManyLeft--;
                             if (howManyLeft == 0) return true;
                             break;
                         }
                     }
                     if(lastHowManyLeft == howManyLeft) return false;
                 }
                 return false;
             }*/

        public bool CanMakeBigger(Station station)
        {
            return QueryMakeBigger(station) != null;
        }

        public bool CanMakeSmaller(Station station)
        {
            return QueryMakeSmaller(station) != null;
        }

        public bool MakeBigger(Station station) //alert niekonsekwentnie - zwracaj bool czy sie udało
        {
            var range = QueryMakeBigger(station);
            if (range == null) return false;
            GiveBack(station.Range);
            Counts[Instance.StationRanges.ToList().IndexOf(range.Value)]--;
            station.Range = range.Value;
            return true;
        }
        public bool MakeSmaller(Station station)
        {
            var range = QueryMakeSmaller(station);
            if (range == null) return false;
            GiveBack(station.Range);
            Counts[Instance.StationRanges.ToList().IndexOf(range.Value)]--;
            station.Range = range.Value;
            return true;
        }

        public double? QueryMakeBigger(Station station)
        {
            for (var i = Instance.StationRanges.ToList().IndexOf(station.Range) + 1; i < Instance.StationRanges.Count(); i++)
            {
                if (CanGet(Instance.StationRanges[i]))
                {
                    return Instance.StationRanges[i];
                }
            }

            return null;
        }

        public double? QueryMakeSmaller(Station station)
        {
            for (var i = Instance.StationRanges.ToList().IndexOf(station.Range) - 1; i >= 0; i--)
            {
                if (CanGet(Instance.StationRanges[i]))
                {
                    return Instance.StationRanges[i];
                }
            }

            return null;
        }

        public bool CanChangeRange(Station station, double newRange)
        {
            if (station.Range == newRange) return true;
            if (!CanGet(newRange)) return false;
            return true;
        }

        public bool CanChangeRange(List<Station> stations, double newRange)
        {
            Cost cost = new Cost(this);
            foreach(var station in stations)
            {
                if (!cost.ChangeRange(station, newRange)) return false;
            }

            return true;
        }

        public bool ChangeRange(Station station, double newRange)
        {
            if(!CanChangeRange(station, newRange)) return false;
            GiveBack(station.Range);
            Get(newRange);
            station.Range = newRange;
            return true;
        }

        
        
        void GiveBack(double range, int count = 1)
        {
            Counts[Instance.StationRanges.ToList().IndexOf(range)] += count;
        }
    }
}
