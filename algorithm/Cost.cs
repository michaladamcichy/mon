using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class Cost
    {
        public Instance Instance { get; }
        public int[] Counts { get; private set; } = new int[] { 0, 0, 0 };

        public double[] Ranges { get { return Instance.Ranges; } }

        public HashSet<double> ForbiddenRanges { get; private set; } = new HashSet<double>();
        //public List<Station> Stations { get; } = new List<Station>();
        bool logEnabled = false;
        public Cost(Instance instance)
        {
            this.Instance = instance;
            Counts = (int[]) Instance.Counts.Clone(); //alert czy to działa?
        }

        public Cost(Cost cost, bool logEnabled = true)
        {
            this.Instance = cost.Instance; //alert
            Counts = (int[])(cost.Counts.Clone());
            ForbiddenRanges = new HashSet<double>(cost.ForbiddenRanges);
            this.logEnabled = logEnabled;
            Log("new Cost");
        }

        public void AddForbiddenRange(double range)
        {
            ForbiddenRanges.Add(range);
            Log("add forbidden range");
        }

        public void RemoveAllForbiddenRanges()
        {
            ForbiddenRanges.Clear();
            Log("remove all forbidden ranges");
        }

        public bool CanGet(double range, int count = 1)
        {
            var index = Instance.Ranges.ToList().IndexOf(range);
            if (Counts[index] - count < 0 || ForbiddenRanges.Contains(range)) return false;
            return true;
        }

        public bool Get(double range, int count = 1)
        {
            var index = Instance.Ranges.ToList().IndexOf(range);
            if (Counts[index] - count < 0 || ForbiddenRanges.Contains(range))
            {
                Log("Get " + range.ToString() + " x" + count.ToString() + " failed");
                return false;
            }
            Counts[index] -= count;
            Log("Get " + range.ToString() + " x" + count.ToString());
            return true;
        }

        public double? GetMax(int count = 1, double maxRange = double.MaxValue)
        {
            var max = QueryMax(count, maxRange);
            if (max == null)
            {
                Log("GetMax null!");
                return null;
            }

            Counts[Instance.Ranges.ToList().IndexOf(max.Value)] -= count;
            Log("Get max " + max.ToString());
            return max;
        }

        public double? GetMin(int count = 1, double minRange = 0.0)
        {
            var min = QueryMin(count, minRange);
            if (min == null)
            {
                Log("GetMin null!");
                return null;
            }

            Counts[Instance.Ranges.ToList().IndexOf(min.Value)] -= count;
            Log("GetMin " + min.ToString());
            return min;
        }

        public double? QueryMax(int count = 1, double maxRange = double.MaxValue)
        {
            for (var i = Instance.Counts.Count() - 1; i >= 0; i--)
            {
                if (Counts[i] - count < 0 || Instance.Ranges[i] > maxRange || ForbiddenRanges.Contains(Instance.Ranges[i])) continue;
                return Instance.Ranges[i];
            }
            return null;
        }

        public double? QueryMin(int count = 1, double minRange = 0.0)
        {
            for (var i = 0; i < Instance.Counts.Count(); i++)
            {
                if (Counts[i] - count < 0 || Instance.Ranges[i] < minRange || ForbiddenRanges.Contains(Instance.Ranges[i])) continue;
                return Instance.Ranges[i];
            }
            return null;
        }

        public double? GetMinCoveringRange(List<Station> stations)
        {
            var minCoveringRange = QueryMinCoveringRange(stations);
            if (minCoveringRange == null)
            {
                Log("GetMinCoveringRange null!");
                return null;
            }

            Counts[Instance.Ranges.ToList().IndexOf(minCoveringRange.Value)]--;
            Log("GetMinCoveringRange " + minCoveringRange.ToString());
            return minCoveringRange.Value;
        }

        public double? QueryMinCoveringRange(List<Station> stations)
        {
            var minCoveringCircleRadius = MapObject.MinCoveringCircleRadius(stations);

            for (var i = 0; i < Instance.Counts.Count(); i++)
            {
                if (Instance.Ranges[i] >= minCoveringCircleRadius && Counts[i] > 0 && !ForbiddenRanges.Contains(Instance.Ranges[i]))
                {
                    return Instance.Ranges[i];
                }
            }
            return null;
        }

        public bool CanGetAny()
        {
            for (var i = 0; i < Instance.Ranges.Count(); i++)
            {
                if (Counts[i] > 0 && !ForbiddenRanges.Contains(Instance.Ranges[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanGetAny(int count)
        {
            var cost = new Cost(this, false);

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
            if (range == null)
            {
                Log("MakeBigger failed!");
                return false;
            }
            GiveBack(station.Range);
            Counts[Instance.Ranges.ToList().IndexOf(range.Value)]--;
            station.Range = range.Value;
            Log("MakeBigger ->" + range.Value.ToString());
            return true;
        }
        public bool MakeSmaller(Station station)
        {
            var range = QueryMakeSmaller(station);
            if (range == null)
            {
                Log("MakeSmaller failed!");
                return false;
            }

            GiveBack(station.Range);
            Counts[Instance.Ranges.ToList().IndexOf(range.Value)]--;
            station.Range = range.Value;
            Log("MakeSmaller ->" + range.ToString());
            return true;
        }

        public double? QueryMakeBigger(Station station)
        {
            if (station.IsStationary) return null; //alert!
            for (var i = Instance.Ranges.ToList().IndexOf(station.Range) + 1; i < Instance.Ranges.Count(); i++)
            {
                if (CanGet(Instance.Ranges[i]))
                {
                    return Instance.Ranges[i];
                }
            }

            return null;
        }

        public double? QueryMakeSmaller(Station station)
        {
            if(station.IsStationary) return null;
            for (var i = Instance.Ranges.ToList().IndexOf(station.Range) - 1; i >= 0; i--)
            {
                if (CanGet(Instance.Ranges[i]))
                {
                    return Instance.Ranges[i];
                }
            }

            return null;
        }

        public bool CanChangeRange(Station station, double newRange)
        {
            if (station.IsStationary) return false;
            if (station.Range == newRange) return true;
            if (!CanGet(newRange)) return false;
            return true;
        }

        public bool CanChangeRangeMin(Station station, double minRange = 0.0)
        {
            return QueryChangeRangeMin(station, minRange) != null;
        }

        public double? QueryChangeRangeMin(Station station, double minRange = 0.0)
        {
            foreach (var range in Ranges)
            {
                if (range < minRange) continue;
                if (CanChangeRange(station, range)) return range;
            }
            return null;
        }

        public bool ChangeRangeMin(Station station, double minRange = 0.0)
        {
            var range = QueryChangeRangeMin(station, minRange);
            if (range == null) return false;
            return ChangeRange(station, range.Value);
        }

        public bool CanChangeRange(List<Station> stations, double newRange)
        {
            if (stations.Any(station => station.IsStationary)) return false;
            Cost cost = new Cost(this, false);
            foreach(var station in stations)
            {
                if (!cost.ChangeRange(new Station(station), newRange)) return false;   
            }

            return true;
        }

        public bool ChangeRange(Station station, double newRange)
        {
            if (!CanChangeRange(station, newRange))
            {
                Log("ChangeRange failed!");
                return false;
            }
            GiveBack(station.Range);
            Get(newRange);
            station.Range = newRange;
            Log("ChangeRange ->" + newRange.ToString());
            return true;
        }

        public bool ChangeRange(List<Station> stations, double newRange)
        {
            if (!CanChangeRange(stations, newRange)) return false;
            
            foreach(var station in stations)
            {
                ChangeRange(station, newRange);
            }

            return true;
        }

        public Station? QueryJoin(Station first, Station second, double tolerance = 0.1) //alert tolerance nie jest uwzlędniony symetrycznie - od strony bigger nie jest
        {
            if (MapObject.AreInRange(first, second)) return null;
            if(!CanGetAny()) return null;

            var smaller = new Station(first.Range < second.Range ? first : second);
            var bigger = new Station(second.Range > first.Range ? second : first);

            var middlePoint = MapObject.CenterOfGravity(new List<MapObject>() { smaller, bigger });
            var middlePointRange = QueryMin(1, (smaller.GetDistanceFrom(bigger) / 2.0) * (1.0 + tolerance / 2.0)); //alert czy na pewno przez 2?
            if (middlePointRange != null)
            {
                var middleStation = new Station(middlePoint, middlePointRange.Value);
                if (MapObject.AreInRange(smaller, middleStation) && MapObject.AreInRange(middleStation, bigger))
                {
                    //Get(middlePointRange.Value);
                    return middleStation;
                }
            }

            if (!CanGetAny()) return null;
            var potentialJoiningStation = MapObject.GetNextFromTowards(smaller, bigger, QueryMax() ?? 0, tolerance); //ALERT źle działą
            if(!potentialJoiningStation.IsInRange(bigger)) return null;
            var minRange = Math.Max(smaller.Range, potentialJoiningStation.GetDistanceFrom(bigger) * (1.0 + tolerance / 2.0)); //alert na pewno przez 2?

            var range = QueryMin(1, minRange);
            return range == null ? null : new Station(potentialJoiningStation.Position, range.Value);
        }

        public Station? Join(Station first, Station second, double tolerance = 0.1)
        {
            var station = QueryJoin(first, second, tolerance);
            if(station == null) return null;
            Get(station.Range);
            return station;
        }
        
        public void GiveBack(double range, int count = 1)
        {
            Debug.Assert(Instance.Ranges.Contains(range)); //alert! alert!
            Counts[Instance.Ranges.ToList().IndexOf(range)] += count;
            Log("GiveBack " + range.ToString() + " x" + count.ToString());
        }

        void Log(string operationName)
        {
            if(logEnabled)
                Debug.WriteLine(operationName + "  | " + string.Join(",", Counts));
        }

        public static bool IsCheaperThan(List<Station> item1, List<Station> item2)
        {
            return CalculateCost(item1) < CalculateCost(item2);
        }
        public static List<Station> GetCheaper(List<Station> path1, List<Station> path2)
        {
            return CalculateCost(path1) < CalculateCost(path2) ? path1 : path2;
        }
        public static List<Station> GetCheapest(List<List<Station>> paths)
        {
            return paths.Aggregate((curMin, item) => GetCheaper(curMin, item));
        }

        public static double CalculateCost(List<Station> stations)
        {
            return stations.Aggregate(0.0, (sum, item) => sum + item.Range);
        }

        public static double CalculateCostPerStation(List<Station> stations)
        {
            return CalculateCost(stations) / stations.Count;
        }

        public bool CanMakeGroup(List<Station> _stations, MapObject center, double range)
        {
            var stations = _stations.Select(station => new Station(station)).ToList();
            Cost cost = new Cost(this);

            if(!cost.Get(range)) return false;
            
            foreach(var station in stations)
            {
                if (!cost.ChangeRangeMin(station, station.GetDistanceFrom(center))) return false;
            }
            return true;
        }

        public bool MakeGroup(List<Station> stations, MapObject center, double range)
        {
            if (!CanMakeGroup(stations, center, range)) return false;

            Get(range);

            foreach(var station in stations)
            {
                ChangeRangeMin(station, station.GetDistanceFrom(center));
            }

            return true;
        }
     }
}
