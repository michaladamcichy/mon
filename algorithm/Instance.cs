using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class InstanceJSON
    {
        public double[] ranges { get; set; } = new double[0];
        public int[] counts { get; set; } = new int[0];

        public List<StationJSON> stations { get; set; } = new List<StationJSON>();
        public List<UnitJSON> units { get; set; } = new List<UnitJSON>();

        public bool optimized { get; set; } = false;

        public InstanceJSON(double[] ranges, int[] counts, List<StationJSON> stations, List<UnitJSON> units)
        {
            this.ranges = ranges;
            this.counts = counts;
            this.stations = stations;
            this.units = units;
            this.optimized = optimized;

        }
    }
    public class Instance
    {
        public double[] Ranges { get; set; } = new double[0];
        public int[] Counts { get; set; } = new int[0];
        public List<int> Priorities { get { return getPriorities(); } }

        public List<MapObject> MapObjects { get { return new List<MapObject>().Concat<MapObject>(Stations).Concat<MapObject>(Units).ToList(); } }

        public List<Station> Stations { get; set; } = new List<Station>();
        public List<Unit> Units { get; set; } = new List<Unit>();

        public bool Optimized { get; private set; } = false;

        public List<Station> CoreStations { get { return Stations.Where(station => station.IsCore).ToList(); } }
        public List<Station> PrivateStations { get { return Stations.Where(station => !station.IsCore).ToList(); } }
        public List<Station> StationaryStations { get { return Stations.Where(station => station.IsStationary).ToList(); } }
        public List<Station> MobileStations { get { return Stations.Where(station => !station.IsStationary).ToList(); } }

        public List<Station> MobileCoreStations { get { return Stations.Where(station => station.IsMobile && station.IsCore).ToList(); } }
        public Instance(InstanceJSON instanceJSON, bool initialize = true)
        {
            this.Ranges = instanceJSON.ranges; //alert!
            this.Counts = instanceJSON.counts;
            Stations = instanceJSON.stations.Select(item => new Station(item)).ToList();
            Units = instanceJSON.units.Select(item => new Unit(item)).ToList();
            Optimized = instanceJSON.optimized;

            CreateRelations();
            UpdateCounts(); //alert
        }

        public Instance(Instance instance)
        {
            Ranges = (double[]) instance.Ranges.Clone();
            Counts = (int[])instance.Counts.Clone();
            Stations = new List<Station>(instance.Stations);
            Units = new List<Unit>(instance.Units);
            CreateRelations();
    }

        public Instance(List<Station> stations, List<Unit> units, double[] ranges, int[] counts)
        {
            this.Ranges = (double[]) ranges.Clone();
            this.Counts = (int[]) counts.Clone(); 

            Stations = new List<Station>(stations);
            Units = new List<Unit>(units);
            CreateRelations();
            UpdateCounts(); //alert
        }

        public Instance(List<Station> stations, double[] ranges, int[] counts) : this(stations, new List<Unit>(), ranges, counts) {}

        public Instance(int[] counts, double[] ranges = null)
        {
            this.Counts = counts;
            this.Ranges = ranges;
        }

        public void CreateRelations(double tolerance = 0.0)
        {
            //RemoveRelations();
            foreach(var first in Stations)
            {
                foreach(var second in Stations)
                {
                    if (first == second) continue;

                    if(MapObject.AreInRange(first, second, tolerance))
                    {
                        first.AddNeighbor(second);
                        second.AddNeighbor(first);
                    }
                }
            }

            var unitToStations = new Dictionary<Unit, HashSet<Station>>();
            foreach(var unit in Units)
            {
                unitToStations[unit] = new HashSet<Station>();
                
                foreach(var station in Stations.Where(station => station.IsMobile))
                {
                    if(unit.Position.Equals(station.Position))
                    {
                        unitToStations[unit].Add(station);
                    }
                }
            }

            foreach(var (unit, stations) in unitToStations)
            {
                var notAttached = stations.Where(station => !station.IsAttached());
                if (notAttached.Count() == 0) continue;
                var smallestStation = notAttached.Aggregate((current, next) => next.Range < current.Range ? next : current);
                unit.Attach(smallestStation);
            }
        }

        public bool CanAcquireStation(double range, int howMany = 1) //alert stations czy station??
        {
            var index = Ranges.ToList().FindIndex(item => item == range);
            if (index == -1) return false;
            return Counts[index] >= howMany;
        }

        public Station? AquireStation(List<Station> stations)
        {
            double minCoveringRadius = MapObject.MinCoveringCircleRadius(stations);

            for(int i =0; i < Ranges.Count(); i++)
            {
                if(Ranges[i] <= minCoveringRadius && Counts[i] > 0)
                {
                    Counts[i]--;
                    return new Station(Ranges[i]);
                }
            }

            return null;
        }

        public Station? AquireMinStation() //alert walnąć jednolinijkowca
        {
            for (int i = 0; i < Ranges.Count(); i++)
            {
                if (Counts[i] > 0)
                {
                    Counts[i]--;
                    return new Station(Ranges[i]);
                }
            }

            return null;
        }

        public Station? AquireMaxStation()
        {
            for (int i = Ranges.Count() - 1; i >= 0; i--)
            {
                if (Counts[i] > 0)
                {
                    Counts[i]--;
                    return new Station(Ranges[i]);
                }
            }

            return null;
        }

        public void GiveBackStation(Station station)
        {
            for(int i=0; i < Ranges.Count(); i++)
            {
                if(Ranges[i] == station.Range)
                {
                    Counts[i]++;
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
            for(var i = 0; i < Counts.Count(); i++)
            {
                Counts[i] -= Stations.FindAll(station => station.Range == Ranges[i]).Count;
            }
        }

        public void RemoveRelations()
        {
            Stations.ForEach(station => { station.Neighbors.Clear(); if(station.Unit != null) station.Unit.RemoveAttachment(); }); //alert all czy tylko stations?

        }

    }
}
