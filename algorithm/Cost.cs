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

        HashSet<double> forbiddenRanges = new HashSet<double>();
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
        }

        public void AddForbiddenRange(double range)
        {
            forbiddenRanges.Add(range);
        }

        public void RemoveAllForbiddenRanges()
        {
            forbiddenRanges.Clear();
        }

        public bool CanGet(double range, int count = 1)
        {
            var index = Instance.StationRanges.ToList().IndexOf(range);
            if (Counts[index] - count < 0 || forbiddenRanges.Contains(range)) return false;
            return true;
        }

        public void Get(double range, int count = 1)
        {
            var index = Instance.StationRanges.ToList().IndexOf(range);
            if (Counts[index] - count < 0 || forbiddenRanges.Contains(range)) throw new Exception("Station range not available");
            Counts[index] -= count;
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
                if (Counts[i] - count < 0 || Instance.StationRanges[i] > maxRange || forbiddenRanges.Contains(maxRange)) continue;
                return Instance.StationRanges[i];
            }
            return null;
        }

        public double? QueryMin(int count = 1, double minRange = 0.0)
        {
            for (var i = 0; i < Instance.StationCounts.Count(); i++)
            {
                if (Counts[i] - count < 0 || Instance.StationRanges[i] < minRange || forbiddenRanges.Contains(minRange)) continue;
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
                if (Instance.StationRanges[i] >= minCoveringCircleRadius && Counts[i] > 0 && !forbiddenRanges.Contains(Instance.StationRanges[i]))
                {
                    return Instance.StationRanges[i];
                }
            }
            return null;
        }

        public bool CanGetAny()
        {
            for(var i = 0; i < Instance.StationRanges.Count(); i++)
            {
                if(Counts[i] > 0 && !forbiddenRanges.Contains(Instance.StationRanges[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanMakeBigger(Station station)
        {
            return QueryMakeBigger(station) != null;
        }

        public bool CanMakeSmaller(Station station)
        {
            return QueryMakeSmaller(station) != null;
        }

        public void MakeBigger(Station station) //alert niekonsekwentnie - zwracaj bool czy sie udało
        {
            var range = QueryMakeBigger(station);
            if (range == null) throw new Exception("Trying to make station bigger but lacks resources");
            GiveBack(station.Range);
            Counts[Instance.StationRanges.ToList().IndexOf(range.Value)]--;
            station.Range = range.Value;
        }
        public void MakeSmaller(Station station)
        {
            var range = QueryMakeSmaller(station);
            if (range == null) throw new Exception("Trying to make station smaller but lacks resources");
            GiveBack(station.Range);
            Counts[Instance.StationRanges.ToList().IndexOf(range.Value)]--;
            station.Range = range.Value;
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

        public void ChangeRange(Station station, double newRange)
        {
            if(!CanChangeRange(station, newRange)) return;
            GiveBack(station.Range);
            Get(newRange);
            station.Range = newRange;
        }

        void GiveBack(double range, int count = 1)
        {
            Counts[Instance.StationRanges.ToList().IndexOf(range)] += count;
        }
    }
}
