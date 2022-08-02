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
        //public int[] Counts { get; private set; } = new int[] { 0, 0, 0 };

        public double[] Ranges { get { return Instance.StationRanges; } }

        public HashSet<double> ForbiddenRanges { get; private set; } = new HashSet<double>();

        public double MaxAffordableDistance { get { return Instance.MaxAffordableDistance; }  }

        public Dictionary<Unit, Unit> __Warehouses { get; private set; } = new Dictionary<Unit, Unit>();
        //public List<Unit> Warehouses { get; private set; } = new List<Unit>();

        public List<Unit> Warehouses { get { return __Warehouses.Keys.ToList(); } }

        Unit __(Unit unit)
        {
            return __Warehouses[unit];
        }
       

        bool logEnabled = true;

        void initializeWarehouses(List<Unit> warehouses)
        {
            warehouses.ForEach(warehouse =>
            {
                var other = new Unit(warehouse);
                __Warehouses.Add(warehouse, other);
                //Warehouses.Add(other);
            });
        }

        void initializeWarehouses(Cost cost)
        {
            cost.__Warehouses.ToList().ForEach(pair => {
                var other = new Unit(pair.Value);
                __Warehouses[pair.Key] = other;
                //Warehouses.Add(other);
            }); //alert czy to działa?
        }

        public Cost(Instance instance)
        {
            this.Instance = instance;
            initializeWarehouses(Instance.Warehouses);
            //Counts = new int[]{ 1000, 1000, 1000}; //alert CRITICAL
        }

        public Cost(Cost cost, bool logEnabled = true)
        {
            this.Instance = cost.Instance; //alert
            initializeWarehouses(cost);
            ForbiddenRanges = new HashSet<double>(cost.ForbiddenRanges);
            
            this.logEnabled = logEnabled;
        }

        // <private>
        int __Index(double range) { return Instance.StationRanges.ToList().IndexOf(range); } //alert

        // </private>

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

        public int HowMuchCanGet(Unit warehouse, double range) //alert uwzględnić wszędzie maxaffordable distance, takze == 0
        {
            return ForbiddenRanges.Contains(range) ? 0 : __(warehouse).Counts[__Index(range)];
        }

        public int HowMuchCanGet(double range)
        {
            return Warehouses.Aggregate(0, (sum, warehouse) => sum + HowMuchCanGet(warehouse, range));
        }

        public int HowMuchCanGet(double range, MapObject mapObject)
        {
            var nearestWarehouses = mapObject.GetNearest(Warehouses);

            var taken = 0;
            foreach(var warehouse in nearestWarehouses)
            {
                if (mapObject.GetDistanceFrom(warehouse) > MaxAffordableDistance) break;
                taken += HowMuchCanGet(warehouse, range);
            }
            return taken;
        }

        public bool CanGet(Unit warehouse, double range, int count = 1)
        {
            return HowMuchCanGet(warehouse, range) >= count;
        }

        public bool CanGet(double range, int count = 1)
        {
            return HowMuchCanGet(range) >= count; 
        }

        public bool CanGet(double range, int count, MapObject mapObject)
        {
            return HowMuchCanGet(range, mapObject) >= count;
        }

        public int GetMost(Unit warehouse, double range)
        {
            var howMuch = HowMuchCanGet(warehouse, range);
            Get(warehouse, howMuch);
            return howMuch;
        }

        public int GetMost(Unit warehouse, double range, int count)
        {
            if (Get(warehouse, range, count)) return count;
            return GetMost(warehouse, range);
        }

        public int GetMost(double range)
        {
            var howMuch = HowMuchCanGet(range);
            Get(range, howMuch);
            return howMuch;
        }

        public int GetMost(double range, int count, MapObject mapObject)
        {
            var taken = 0;
            var nearestWarehouses = mapObject.GetNearest(Warehouses);
            foreach (var warehouse in nearestWarehouses)
            {
                if (mapObject.GetDistanceFrom(warehouse) > MaxAffordableDistance || taken == count) return taken;
                taken += GetMost(warehouse, range);
            }
            return taken;
        }

        public int GetMost(double range, MapObject mapObject)
        {
            var taken = 0;
            var nearestWarehouses = mapObject.GetNearest(Warehouses);
            foreach(var warehouse in nearestWarehouses)
            {
                if (mapObject.GetDistanceFrom(warehouse) > MaxAffordableDistance) return taken;
                taken += GetMost(warehouse, range);
            }
            return taken;
        }

        public bool Get(Unit warehouse, double range, int count = 1)
        {
            if (HowMuchCanGet(warehouse, range) < count) return false;
            __(warehouse).Counts[__Index(range)] -= count;
            return true;
        }

        public bool Get(double range, int count = 1)
        {
            if (!CanGet(range, count)) return false;
            var taken = 0;
            foreach (var warehouse in Warehouses)
            {
                if (taken == count) break;
                GetMost(warehouse, range, count - taken);
            }
            return taken == count;
        }

        public bool Get(double range, int count, MapObject mapObject)
        {
            if(!CanGet(range, count, mapObject)) return false;

            var nearest = mapObject.GetNearest(Warehouses);
            var taken = 0;
            foreach(var warehouse in nearest)
            {
                if(taken == count) break;
                taken += GetMost(warehouse, range, count - taken);
            }
            Debug.Assert(taken == count);
            return true;
        }

        public double? GetMax(int count = 1, double maxRange = double.MaxValue)
        {
            foreach(var range in Ranges.Reverse().Where(range => range <= maxRange && !ForbiddenRanges.Contains(range))) //alert gdzieindziej nie uwzględniałem
            {
                if (Get(range, count)) return range; 
            }
            return null;
        }

        public double? GetMax(int count = 1, MapObject mapObject = null, double maxRange = double.MaxValue)
        {
            if (mapObject == null) return GetMax(count, maxRange);
            foreach (var range in Ranges.Reverse().Where(range => range <= maxRange && !ForbiddenRanges.Contains(range))) //alert gdzieindziej nie uwzględniałem
            {
                if (Get(range, count, mapObject)) return range;
            }
            return null;
        }
        public double? GetMin(int count = 1, double minRange = 0.0)
        {
            foreach (var range in Ranges.Where(range => range >= minRange && !ForbiddenRanges.Contains(range))) //alert gdzieindziej nie uwzględniałem
            {
                if (Get(range, count)) return range;
            }
            return null;
        }

        public double? GetMin(int count = 1, MapObject mapObject = null, double minRange = 0.0)
        {
            if (mapObject == null) return GetMin(count, minRange);
            foreach (var range in Ranges.Where(range => range >= minRange && !ForbiddenRanges.Contains(range))) //alert gdzieindziej nie uwzględniałem
            {
                if (Get(range, count, mapObject)) return range;
            }
            return null;
        }
        public double? QueryMax(int count = 1, double maxRange = double.MaxValue)
        {
            foreach (var range in Ranges.Reverse().Where(range => range <= maxRange && !ForbiddenRanges.Contains(range))) //alert gdzieindziej nie uwzględniałem
            {
                if (CanGet(range, count)) return range;
            }
            return null;
        }

        public double? QueryMax(int count = 1, MapObject mapObject = null, double maxRange = double.MaxValue)
        {
            if (mapObject == null) return QueryMax(count, maxRange);
            foreach (var range in Ranges.Reverse().Where(range => range <= maxRange && !ForbiddenRanges.Contains(range))) //alert gdzieindziej nie uwzględniałem
            {
                if (CanGet(range, count, mapObject)) return range;
            }
            return null;
        }

        public double? QueryMin(int count = 1, double minRange = 0.0)
        {
            foreach (var range in Ranges.Where(range => range >= minRange && !ForbiddenRanges.Contains(range))) //alert gdzieindziej nie uwzględniałem
            {
                if (CanGet(range, count)) return range;
            }
            return null;
        }

        public double? QueryMin(int count = 1, MapObject mapObject = null, double minRange = 0.0)
        {
            if (mapObject == null) return QueryMin(count, minRange);
            foreach (var range in Ranges.Where(range => range >= minRange && !ForbiddenRanges.Contains(range))) //alert gdzieindziej nie uwzględniałem
            {
                if (CanGet(range, count, mapObject)) return range;
            }
            return null;
        }

        /*public double? GetMinCoveringRange(List<Station> stations)
        {
            var minCoveringRange = QueryMinCoveringRange(stations);
            if (minCoveringRange == null)
            {
                Log("GetMinCoveringRange null!");
                return null;
            }

            Counts[Instance.StationRanges.ToList().IndexOf(minCoveringRange.Value)]--;
            Log("GetMinCoveringRange " + minCoveringRange.ToString());
            return minCoveringRange.Value;
        }*/

        public double? GetMinCoveringRange(List<Station> stations)
        {
            var minCoveringRange = QueryMinCoveringRange(stations);
            if (minCoveringRange == null) return null;
            Get(minCoveringRange.Value);
            return minCoveringRange;
        }
        public double? GetMinCoveringRange(List<Station> stations, MapObject mapObject)
        {
            var minCoveringRange = QueryMinCoveringRange(stations, mapObject);
            if (minCoveringRange == null) return null;
            Get(minCoveringRange.Value);
            return minCoveringRange;
        }
        public double? QueryMinCoveringRange(List<Station> stations)
        {
            var minCoveringCircleRadius = MapObject.MinCoveringCircleRadius(stations);
            return QueryMin(1, minCoveringCircleRadius);
        }

        public double? QueryMinCoveringRange(List<Station> stations, MapObject mapObject)
        {
            var minCoveringCircleRadius = MapObject.MinCoveringCircleRadius(stations);
            return QueryMin(1, mapObject, minCoveringCircleRadius);
        }

/*        public double? QueryMinCoveringRange(List<Station> stations)
        {
            var minCoveringCircleRadius = MapObject.MinCoveringCircleRadius(stations);

            for (var i = 0; i < Counts.Count(); i++)
            {
                if (Instance.StationRanges[i] >= minCoveringCircleRadius && Counts[i] > 0 && !ForbiddenRanges.Contains(Instance.StationRanges[i]))
                {
                    return Instance.StationRanges[i];
                }
            }
            return null;
        }*/

/*        public bool CanGetAny()
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
*/
/*        public bool CanGetAny(int count)
        {
            var cost = new Cost(this, false);

            for(var i = 0; i < count; i++)
            {
                var range = cost.GetMin();
                if (range == null) return false;
            }
            return true;
        }*/
        public bool CanGetAny(int count = 1) //alert pamietaj, żeby zrobić wszędzie forbiddenDistance
        {
            return Ranges.Any(range => CanGet(range, count));
        }

        public bool CanGetAny(int count, MapObject mapObject)
        {
            return Ranges.Any(range => CanGet(range, count, mapObject));
        }

        public bool CanMakeBigger(Station station, double minRange = 0.0)
        {
        }
        public bool CanMakeBigger(Station station, MapObject mapObject, double minRange = 0.0)
        {
            //
        }

        public double? QueryMakeBigger(Station station, double minRange = 0.0)
        {

        }

        public double? QueryMakeBigger(Station station, MapObject mapObject, double minRange = 0.0)
        {

        }

        public double? MakeBigger(Station station, double minRange = 0.0)
        {

        }

        public double? MakeBigger(Station station, MapObject mapObject, double minRange = 0.0)
        {

        }
/*
        public bool MakeBigger(Station station) //alert niekonsekwentnie - zwracaj bool czy sie udało
        {
            var range = QueryMakeBigger(station);
            if (range == null)
            {
                Log("MakeBigger failed!");
                return false;
            }
            GiveBack(station.Range);
            Counts[Instance.StationRanges.ToList().IndexOf(range.Value)]--;
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
            Counts[Instance.StationRanges.ToList().IndexOf(range.Value)]--;
            station.Range = range.Value;
            Log("MakeSmaller ->" + range.ToString());
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
        }*/

        /*public bool CanChangeRange(Station station, double newRange)
        {
            if (station.Range == newRange) return true;
            if (!CanGet(newRange)) return false;
            return true;
        }

        public bool CanChangeRange(List<Station> stations, double newRange)
        {
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
*/
        public bool ChangeRange(Station station, double newRange)
        {

        }
        public bool ChangeRange(List<Station> station, List<double> ranges)
        {

        }
        /*
        public Station? QueryJoin(Station first, Station second, double tolerance = 0.1) //alert tolerance nie jest uwzlędniony symetrycznie - od strony bigger nie jest
        {
            if (MapObject.AreInRange(first, second)) return null;
            if(!CanGetAny()) return null;

            var smaller = new Station(first.Range < second.Range ? first : second);
            var bigger = new Station(second.Range > first.Range ? second : first);

            var middlePoint = MapObject.CenterOfGravity(new List<MapObject>() { smaller, bigger });
            var middlePointRange = QueryMin(1, (smaller.GetDistanceFrom(bigger) / 2.0) * (1.0 + tolerance / 2.0)); //alert czy na pewno przez 2?
            if(middlePointRange != null)
            {
                var middleStation = new Station(middlePoint, middlePointRange.Value);
                if (MapObject.AreInRange(smaller, middleStation) && MapObject.AreInRange(middleStation, bigger))
                {
                    Get(middlePointRange.Value);
                    return middleStation;
                }
            }
            var potentialJoiningStation = MapObject.GetNextFromTowards(smaller, bigger);
            if (!potentialJoiningStation.IsInRange(bigger)) return null;
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
            Counts[Instance.StationRanges.ToList().IndexOf(range)] += count;
            Log("GiveBack " + range.ToString() + " x" + count.ToString());
        }
        */

        public void GiveBack(Unit warehouse, double range, int count = 1)
        {
            __(warehouse).Counts[__Index(range)] += count;
        }
        

        void Log(string operationName)
        { //alert TODO
/*            if(logEnabled)
                Debug.WriteLine(operationName + "  | " + string.Join(",", Counts));
*/        }
    }
}
