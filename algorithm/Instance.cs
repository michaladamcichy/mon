using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class InstanceJSON
    {
        public double[] stationRanges { get; set; } = new double[0];
        public int[] stationCounts { get; set; } = new int[0];

        public List<StationJSON> stations { get; set; } = new List<StationJSON>();
        public List<UnitJSON> units { get; set;  } = new List<UnitJSON>();

        public InstanceJSON(double[] stationRanges, int[] stationCounts, List<StationJSON> stations, List<UnitJSON> units)
        {
            this.stationRanges = stationRanges;
            this.stationCounts = stationCounts;
            this.stations = stations;
            this.units = units;
        }
    }
    public class Instance
    {
        public double[] StationRanges { get; set; } = new double[0];
        public int[] StationCounts { get; set; } = new int[0];
        public List<int> Priorities { get { return getPriorities(); } }

        public List<MapObject> MapObjects { get; set; } = new List<MapObject>(); //alert! setter

        public List<Station> Stations { get { return MapObjects.FindAll(item => item is Station).Cast<Station>().ToList(); }
            set { MapObjects.AddRange(value); } }

        public List<Station> CoreStations { get { return Stations.Where(station => !station.IsAttached()).ToList(); } }
        public List<Station> PrivateStations { get { return Stations.Where(station => station.IsAttached()).ToList(); } }
        public List<Station> StationaryStations { get { return Stations.Where(station => station.IsStationary).ToList(); } }
        public List<Station> MobileStations { get { return Stations.Where(station => !station.IsStationary).ToList(); } }

        public List<Unit> Units { get { return MapObjects.FindAll(item => item is Unit).Cast<Unit>().ToList(); } }

        public Instance(InstanceJSON instanceJSON, bool initialize = true)
        {
            this.StationRanges = instanceJSON.stationRanges;
            this.StationCounts = instanceJSON.stationCounts;
            var stations = instanceJSON.stations.Select(item => new Station(item)).ToList();
            var units = instanceJSON.units.Select(item => new Unit(item)).ToList();

            this.MapObjects = prepareMapObjects(stations, units);
            UpdateCounts(); //alert
        }

        public Instance(List<Station> stations, List<Unit> units, int[] counts = null, bool initialize = true)
        {
            var ranges = new List<double>();
            stations.ForEach(item => ranges.Add(item.Range));

            this.StationRanges = new double[] {20.0, 30.0, 50.0 }; //alert 
            this.StationCounts = counts == null ? new int[] { 1000, 1000, 1000 } : counts;

            this.MapObjects = prepareMapObjects(stations, units);
            UpdateCounts(); //alert
        }

        public Instance(List<Station> stations, int[] counts) : this(stations, new List<Unit>(), counts) {}

        public Instance(int[] counts)
        {
            StationRanges = new double[] { 20.0, 30.0, 50.0 }; //alert
            this.StationCounts = counts;
        }

        public List<MapObject> prepareMapObjects(List<Station> stations, List<Unit> units) //alert
        {
            //RemoveRelations(); //alert

            var mapObjects = stations.Cast<MapObject>().Concat(units.Cast<MapObject>()).ToList();
            foreach(var first in stations)
            {
                foreach(var second in stations)
                {
                    if (first == second) continue;

                    if(MapObject.AreInRange(first, second))
                    {
                        first.AddNeighbor(second);
                        second.AddNeighbor(first);
                    }
                }
            }

            foreach(var station in stations)
            {
                if (station.IsStationary) continue;
                foreach(var unit in units)
                {
                    if(!unit.Position.Equals(station.Position)) continue;
                    if(!unit.HasAttachement() && !station.IsAttached())
                    {
                        unit.Attach(station);
                    } else if(unit.HasAttachement() && !station.IsAttached() && station.Range < unit.GetAttachment().Range)
                    {
                        unit.RemoveAttachment();
                        unit.Attach(station);
                    }
                }
            }


            return mapObjects;
        }

        public bool CanAcquireStation(double range, int howMany = 1) //alert stations czy station??
        {
            var index = StationRanges.ToList().FindIndex(item => item == range);
            if (index == -1) return false;
            return StationCounts[index] >= howMany;
        }

        public Station? AquireStation(List<Station> stations)
        {
            double minCoveringRadius = MapObject.MinCoveringCircleRadius(stations);

            for(int i =0; i < StationRanges.Count(); i++)
            {
                if(StationRanges[i] <= minCoveringRadius && StationCounts[i] > 0)
                {
                    StationCounts[i]--;
                    return new Station(StationRanges[i]);
                }
            }

            return null;
        }

        public Station? AquireMinStation() //alert walnąć jednolinijkowca
        {
            for (int i = 0; i < StationRanges.Count(); i++)
            {
                if (StationCounts[i] > 0)
                {
                    StationCounts[i]--;
                    return new Station(StationRanges[i]);
                }
            }

            return null;
        }

        public Station? AquireMaxStation()
        {
            for (int i = StationRanges.Count() - 1; i >= 0; i--)
            {
                if (StationCounts[i] > 0)
                {
                    StationCounts[i]--;
                    return new Station(StationRanges[i]);
                }
            }

            return null;
        }

        public void GiveBackStation(Station station)
        {
            for(int i=0; i < StationRanges.Count(); i++)
            {
                if(StationRanges[i] == station.Range)
                {
                    StationCounts[i]++;
                }
            }
        }

        public Dictionary<Station, double> SaveRangesSnapshot()
        {
            return SaveRangesSnapshot(Stations);
        }
        public static void RestoreRangesSnapshot(Dictionary<Station, double> snapshot)
        {
            snapshot.ToList().ForEach(keyValue => keyValue.Key.Range = keyValue.Value);
        }
        public static Dictionary<Station, double> SaveRangesSnapshot(List<Station> stations)
        {
            var snapshot = new Dictionary<Station, double>();
            stations.ForEach(station => snapshot[station] = station.Range);
            return snapshot;
        }

        List<int> getPriorities()
        {
            var priorities = Units.Select(unit => unit.Priority).ToHashSet().ToList();
            priorities.Sort();
            return priorities;
        }

        public void UpdateCounts() //alert brzydkie
        {
            for(var i = 0; i < StationCounts.Count(); i++)
            {
                StationCounts[i] -= Stations.FindAll(station => station.Range == StationRanges[i]).Count;
            }
        }

        public void RemoveRelations()
        {
            Stations.ForEach(station => station.Neighbors.Clear()); //alert all czy tylko stations?
        }

    }
}
