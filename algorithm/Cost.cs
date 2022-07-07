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

        public bool CanGet(double range, int count = 1)
        {
            var index = Instance.StationRanges.ToList().IndexOf(range);
            if (Counts[index] - count < 0) return false;
            return true;
        }

        public void Get(double range, int count = 1)
        {
            var index = Instance.StationRanges.ToList().IndexOf(range);
            if (Counts[index] - count < 0) throw new Exception("Station range not available");
            Counts[index] -= count;
        }

        public double? GetMax(int count = 1)
        {
            for (var i = Instance.StationCounts.Count() - 1; i >= 0; i--)
            {
                if (Counts[i] - count < 0) continue;
                Counts[i] -= count;
                return Instance.StationRanges[i];
            }
            return null;
        }

        public double? GetMin(int count = 1)
        {
            for (var i = 0; i < Instance.StationCounts.Count(); i++)
            {
                if (Counts[i] - count < 0) continue;
                Counts[i] -= count;
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
                if (Instance.StationRanges[i] >= minCoveringCircleRadius && Counts[i] > 0)
                {
                    return Instance.StationRanges[i];
                }
            }
            return null;
        }

        public bool CanGetAny()
        {
            return Counts.Any(item => item > 0);
        }

        public bool CanMakeBigger(Station station)
        {
            return QueryMakeBigger(station) != null;
        }

        public bool CanMakeSmaller(Station station)
        {
            return QueryMakeSmaller(station) != null;
        }

        public double? QueryMakeBigger(Station station)
        {
            for (var i = Instance.StationRanges.ToList().IndexOf(station.Range) + 1; i < Instance.StationRanges.Count(); i++)
            {
                if (CanGet(Counts[i]))
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
                if (CanGet(Counts[i]))
                {
                    return Instance.StationRanges[i];
                }
            }

            return null;
        }

        public void ChangeRange(Station station, double newRange)
        {
            if (station.Range == newRange) return;
            if (!CanGet(newRange)) throw new Exception("Cannot Change Range. Range not available.");
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
